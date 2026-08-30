using System.Text.Json;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Lista local dos 240 patches (nomes e cadeia). Evita reler a pedaleira a cada sessão.
/// </summary>
public static class MatriboxPresetStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string FilePath => AppConfig.File("matribox-presets.json");

    public static List<MatriboxPresetInfo> Load()
    {
        var list = EmptyList();
        try
        {
            if (!File.Exists(FilePath))
            {
                return list;
            }

            string text = File.ReadAllText(FilePath);
            var file = JsonSerializer.Deserialize<PresetFile>(text, JsonOptions);
            if (file?.Presets is null)
            {
                return list;
            }

            foreach (var row in file.Presets)
            {
                if (row.Index < 0 || row.Index >= list.Count)
                {
                    continue;
                }

                List<MatriboxChainSlot>? chain = null;
                if (row.Chain is { Count: > 0 })
                {
                    chain = row.Chain.Select(s => new MatriboxChainSlot
                    {
                        Position = s.Position,
                        ModuleType = s.ModuleType,
                        Algorithm = s.Algorithm,
                        On = s.On
                    }).ToList();
                }

                list[row.Index] = new MatriboxPresetInfo
                {
                    Index = row.Index,
                    Name = row.Name ?? "",
                    Style = row.Style ?? "",
                    Chain = chain
                };
            }
        }
        catch
        {
            // ficheiro em falta ou inválido — começa vazio
        }

        return list;
    }

    public static bool HasName(string? name)
        => !string.IsNullOrWhiteSpace(name) && !HotoneSysex.IsHeaderNoise(name);

    public static bool HasChain(IReadOnlyList<MatriboxChainSlot>? chain)
        => chain is { Count: > 0 } && chain.Any(s => !s.Empty);

    public static bool HasUsefulData(MatriboxPresetInfo p)
        => HasName(p.Name) || HasChain(p.Chain) || !string.IsNullOrWhiteSpace(p.Style);

    public static MatriboxPresetInfo MergeSlot(MatriboxPresetInfo keep, MatriboxPresetInfo incoming)
        => new()
        {
            Index = incoming.Index >= 0 ? incoming.Index : keep.Index,
            Name = HasName(incoming.Name) ? incoming.Name : keep.Name,
            Style = string.IsNullOrWhiteSpace(incoming.Style) ? keep.Style : incoming.Style,
            Chain = HasChain(incoming.Chain) ? incoming.Chain : keep.Chain
        };

    /// <summary>
    /// Grava sem apagar nomes/cadeias já no disco. Não cria ficheiro só com slots vazios.
    /// </summary>
    public static void Save(IReadOnlyList<MatriboxPresetInfo> presets)
    {
        var merged = EmptyList();
        var onDisk = File.Exists(FilePath) ? Load() : merged;
        for (int i = 0; i < merged.Count; i++)
        {
            var keep = i < onDisk.Count ? onDisk[i] : merged[i];
            var incoming = i < presets.Count ? presets[i] : keep;
            merged[i] = MergeSlot(keep, incoming);
        }

        if (!merged.Any(HasUsefulData) && !onDisk.Any(HasUsefulData))
        {
            return;
        }

        string dir = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(dir);
        var file = new PresetFile
        {
            SavedAt = DateTime.Now,
            Presets = merged.Select(p => new PresetRow
            {
                Index = p.Index,
                Name = p.Name,
                Style = p.Style,
                Chain = p.Chain?.Select(s => new ChainRow
                {
                    Position = s.Position,
                    ModuleType = s.ModuleType,
                    Algorithm = s.Algorithm,
                    On = s.On
                }).ToList() ?? []
            }).ToList()
        };

        string tmp = FilePath + ".tmp";
        string bak = Path.Combine(dir, "matribox-presets.bak.json");
        File.WriteAllText(tmp, JsonSerializer.Serialize(file, JsonOptions));
        if (File.Exists(FilePath))
        {
            File.Replace(tmp, FilePath, bak, ignoreMetadataErrors: true);
        }
        else
        {
            File.Move(tmp, FilePath);
        }
    }

    public static List<MatriboxPresetInfo> EmptyList()
        => Enumerable.Range(0, HotoneSysex.PresetCount)
            .Select(i => new MatriboxPresetInfo { Index = i })
            .ToList();

    private sealed class PresetFile
    {
        public DateTime SavedAt { get; set; }
        public List<PresetRow> Presets { get; set; } = [];
    }

    private sealed class PresetRow
    {
        public int Index { get; set; }
        public string? Name { get; set; }
        public string? Style { get; set; }
        public List<ChainRow> Chain { get; set; } = [];
    }

    private sealed class ChainRow
    {
        public int Position { get; set; }
        public int ModuleType { get; set; } = -1;
        public int Algorithm { get; set; }
        public bool On { get; set; }
    }
}
