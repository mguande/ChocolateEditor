using System.Text;
using System.Text.Json;

namespace ChocolateEditor.Matribox;

public readonly record struct MatriboxAlgEntry(int ModuleIndex, string ModuleName, int Fxid, string Name);

public sealed class MatriboxWidget
{
    public string Name { get; init; } = "";
    public int Id { get; init; }
    public int Default { get; init; }
    public int Min { get; init; }
    public int Max { get; init; } = 100;
}

/// <summary>
/// Nomes de algoritmos do alg_data.json do editor Sonicake (fxid → "COMP 1").
/// </summary>
public static class MatriboxAlgCatalog
{
    private static readonly string[] JsonPaths =
    [
        @"C:\Program Files\Sonicake\Matribox II PRO\data\flutter_assets\assets\alg\alg_data.json",
        Path.Combine(AppContext.BaseDirectory, "alg_data.json")
    ];

    private static Dictionary<int, string>? _byFxid;
    private static List<MatriboxAlgEntry>? _entries;
    private static Dictionary<int, List<MatriboxWidget>>? _widgets;

    public static string Name(int fxid)
    {
        Ensure();
        return _byFxid is not null && _byFxid.TryGetValue(fxid, out string? name) ? name : "";
    }

    public static IReadOnlyList<MatriboxAlgEntry> Entries
    {
        get
        {
            Ensure();
            return _entries ?? [];
        }
    }

    public static IReadOnlyList<MatriboxWidget> Widgets(int fxid)
    {
        Ensure();
        return _widgets is not null && _widgets.TryGetValue(fxid, out var list) ? list : [];
    }

    public static IReadOnlyList<MatriboxParamValue> BindParams(int fxid, IReadOnlyDictionary<string, int>? requested)
    {
        var widgets = Widgets(fxid);
        if (widgets.Count == 0)
        {
            return [];
        }

        return widgets.Select(w =>
        {
            int value = w.Default;
            if (requested is not null)
            {
                var hit = requested.FirstOrDefault(kv =>
                    string.Equals(kv.Key, w.Name, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(hit.Key))
                {
                    value = hit.Value;
                }
            }

            return new MatriboxParamValue
            {
                Name = w.Name,
                Id = w.Id,
                Value = Math.Clamp(value, w.Min, w.Max),
                Min = w.Min,
                Max = w.Max
            };
        }).ToList();
    }

    public static string PromptWidgets()
    {
        Ensure();
        var sb = new StringBuilder();
        foreach (var e in Entries)
        {
            var w = Widgets(e.Fxid);
            if (w.Count == 0)
            {
                continue;
            }

            sb.Append(e.ModuleName).Append(' ').Append(e.Name).Append(": ");
            sb.AppendJoin(", ", w.Select(x => $"{x.Name}={x.Default} ({x.Min}-{x.Max})"));
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    public static string PromptCatalog()
    {
        Ensure();
        var sb = new StringBuilder();
        foreach (var group in Entries.GroupBy(e => e.ModuleName))
        {
            sb.Append(group.Key).Append(": ");
            sb.AppendJoin(", ", group.Select(e => e.Name));
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    public static bool TryResolve(string? typeName, string? algorithm, out int moduleIndex, out int fxid, out string resolvedName)
    {
        Ensure();
        moduleIndex = -1;
        fxid = 0;
        resolvedName = "";
        if (string.IsNullOrWhiteSpace(algorithm) || _entries is null || _entries.Count == 0)
        {
            return false;
        }

        string alg = Norm(algorithm);
        if (alg is "--" or "none" or "vazio" or "empty")
        {
            return false;
        }

        int wantType = IndexOfType(typeName);
        IEnumerable<MatriboxAlgEntry> pool = _entries;
        if (wantType >= 0)
        {
            var typed = _entries.Where(e => e.ModuleIndex == wantType).ToList();
            if (typed.Count > 0)
            {
                pool = typed;
            }
        }

        var list = pool.ToList();
        if (!TryPick(list, alg, out var hit) && wantType >= 0)
        {
            TryPick(_entries, alg, out hit);
        }

        if (string.IsNullOrEmpty(hit.Name))
        {
            return false;
        }

        moduleIndex = hit.ModuleIndex;
        fxid = hit.Fxid;
        resolvedName = hit.Name;
        return true;
    }

    private static bool TryPick(IEnumerable<MatriboxAlgEntry> list, string alg, out MatriboxAlgEntry hit)
    {
        hit = default;
        foreach (var e in list)
        {
            if (Norm(e.Name) == alg)
            {
                hit = e;
                return true;
            }
        }

        foreach (var e in list)
        {
            string n = Norm(e.Name);
            if (n.StartsWith(alg) || alg.StartsWith(n))
            {
                hit = e;
                return true;
            }
        }

        foreach (var e in list)
        {
            string n = Norm(e.Name);
            if (n.Contains(alg) || alg.Contains(n))
            {
                hit = e;
                return true;
            }
        }

        return false;
    }

    public static int IndexOfType(string? typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            return -1;
        }

        string n = Norm(typeName);
        for (int i = 0; i < HotoneSysex.ModuleTypeNames.Length; i++)
        {
            if (Norm(HotoneSysex.ModuleTypeNames[i]) == n)
            {
                return i;
            }
        }

        return n switch
        {
            "clone" => 11,
            "fx loop" or "fx_loop" => 12,
            "snd" => 13,
            "rtn" => 14,
            "volume" => 15,
            _ => -1
        };
    }

    private static string Norm(string value)
        => value.Trim().ToUpperInvariant().Replace('_', ' ');

    private static void Ensure()
    {
        if (_byFxid is not null)
        {
            return;
        }

        _byFxid = [];
        _entries = [];
        _widgets = [];
        string? path = JsonPaths.FirstOrDefault(File.Exists);
        if (path is null)
        {
            return;
        }

        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("Modules", out var modules))
            {
                return;
            }

            foreach (var module in modules.EnumerateArray())
            {
                string moduleName = module.TryGetProperty("name", out var mn) ? mn.GetString() ?? "" : "";
                int moduleIndex = module.TryGetProperty("index", out var mi) && mi.TryGetInt32(out int idx)
                    ? idx
                    : IndexOfType(moduleName);
                if (!module.TryGetProperty("alg", out var algs))
                {
                    continue;
                }

                foreach (var alg in algs.EnumerateArray())
                {
                    if (!alg.TryGetProperty("fxid", out var fx) || !fx.TryGetInt32(out int id))
                    {
                        continue;
                    }

                    string name = alg.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = alg.TryGetProperty("fxtitle", out var t) ? t.GetString() ?? "" : "";
                    }

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        continue;
                    }

                    _byFxid[id] = name;
                    _entries.Add(new MatriboxAlgEntry(moduleIndex, moduleName, id, name));
                    if (alg.TryGetProperty("widget", out var widgets) && widgets.ValueKind == JsonValueKind.Array)
                    {
                        var list = new List<MatriboxWidget>();
                        foreach (var w in widgets.EnumerateArray())
                        {
                            string wn = w.TryGetProperty("name", out var wname) ? wname.GetString() ?? "" : "";
                            if (string.IsNullOrWhiteSpace(wn))
                            {
                                continue;
                            }

                            list.Add(new MatriboxWidget
                            {
                                Name = wn,
                                Id = ReadInt(w, "ID", list.Count),
                                Default = ReadInt(w, "defaultValue", 50),
                                Min = ReadInt(w, "min", 0),
                                Max = ReadInt(w, "max", 100)
                            });
                        }

                        if (list.Count > 0)
                        {
                            _widgets[id] = list;
                        }
                    }
                }
            }
        }
        catch
        {
            // catálogo em falta — a UI fica só com DYN/AMP/…
        }
    }

    private static int ReadInt(JsonElement el, string name, int fallback)
    {
        if (!el.TryGetProperty(name, out var p))
        {
            return fallback;
        }

        if (p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out int n))
        {
            return n;
        }

        return int.TryParse(p.GetString(), out n) ? n : fallback;
    }
}
