using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ChocolateEditor.Ai;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Ficheiros .prst exportados pelo editor Sonicake (pasta Patchs).
/// Nome no ficheiro; cadeia e on/off no payload LZO.
/// </summary>
public static class MatriboxPrstImport
{
    public static readonly string[] DefaultFolders =
    [
        @"C:\Programas\Sonicake\Matribox 2 Pro\Patchs\meus",
        @"C:\Programas\Sonicake\Matribox 2 Pro\Patchs",
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Sonicake", "Matribox 2 Pro", "Patchs")
    ];

    private static readonly Regex FileNameRx = new(
        @"^(?<bank>\d{1,2})-(?<slot>[A-Da-d])(?:\s+(?<name>.+))?\.prst$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public static string? FirstExistingDefaultFolder()
    {
        string custom = ConfiguredFolder();
        if (custom.Length > 0 && Directory.Exists(custom))
        {
            return custom;
        }

        return DefaultFolders.FirstOrDefault(Directory.Exists);
    }

    public static IEnumerable<string> TemplateFolders()
    {
        string custom = ConfiguredFolder();
        if (custom.Length > 0)
        {
            yield return custom;
            yield break;
        }

        foreach (string folder in DefaultFolders)
        {
            yield return folder;
        }
    }

    public static string ConfiguredFolder()
        => AiSettingsStore.Load().PrstFolder.Trim();

    public static int ImportFolder(string folder, List<MatriboxPresetInfo> into)
    {
        if (!Directory.Exists(folder) || into.Count < HotoneSysex.PresetCount)
        {
            return 0;
        }

        int got = 0;
        foreach (string path in Directory.EnumerateFiles(folder, "*.prst"))
        {
            if (!TryRead(path, out var preset))
            {
                continue;
            }

            if (preset.Index < 0 || preset.Index >= into.Count)
            {
                continue;
            }

            into[preset.Index] = MatriboxPresetStore.MergeSlot(into[preset.Index], preset);
            got++;
        }

        return got;
    }

    public static bool TryRead(string path, out MatriboxPresetInfo preset)
    {
        preset = new MatriboxPresetInfo();
        var match = FileNameRx.Match(Path.GetFileName(path));
        if (!match.Success)
        {
            return false;
        }

        int bank = int.Parse(match.Groups["bank"].Value);
        int slot = "ABCD".IndexOf(char.ToUpperInvariant(match.Groups["slot"].Value[0]));
        if (bank is < 1 or > MatriboxMidi.BankCount || slot < 0)
        {
            return false;
        }

        string name = match.Groups["name"].Success
            ? match.Groups["name"].Value.Trim()
            : "";
        string style = "";
        List<MatriboxChainSlot>? chain = null;
        if (TryReadPayload(path, out string payloadName, out style, out chain)
            && MatriboxPresetStore.HasName(payloadName))
        {
            name = payloadName;
        }

        preset = new MatriboxPresetInfo
        {
            Index = (bank - 1) * MatriboxMidi.PresetsPerBank + slot,
            Name = name,
            Style = style,
            Chain = chain
        };
        return MatriboxPresetStore.HasName(name) || bank > 0;
    }

    private static bool TryReadPayload(
        string path, out string name, out string style, out List<MatriboxChainSlot>? chain)
    {
        name = "";
        style = "";
        chain = null;
        if (!TryLoadCompressed(path, out byte[] compressed)
            || !MatriboxLzo.TryDecompress(compressed, out byte[] raw))
        {
            return false;
        }

        int[]? seq = null, mods = null, algs = null, states = null;
        int offset = 0;
        while (offset + 12 <= raw.Length)
        {
            int size = BitConverter.ToInt32(raw, offset + 4);
            int type = (BitConverter.ToInt32(raw, offset + 8) >> 16) & 0xFF;
            int payloadLen = size - 4;
            if (payloadLen < 0 || offset + 12 + payloadLen > raw.Length)
            {
                break;
            }

            var payload = raw.AsSpan(offset + 12, payloadLen);
            switch (type)
            {
                case 1 when payload.Length >= 4 + HotoneSysex.NameSize:
                    name = HotoneSysex.AsciiZ(payload.Slice(4, HotoneSysex.NameSize));
                    if (payload.Length >= 4 + 17 + 20 + 50 + 10)
                    {
                        style = HotoneSysex.AsciiZ(payload.Slice(4 + 17 + 20 + 50, 10));
                    }

                    break;
                case 4 when payload.Length >= 72:
                    seq = ReadBytes(payload, 0, 12);
                    mods = ReadBytes(payload, 12, 12);
                    algs = new int[12];
                    for (int i = 0; i < 12; i++)
                    {
                        algs[i] = BitConverter.ToInt32(payload.Slice(24 + i * 4, 4));
                    }

                    break;
                case 5 when payload.Length >= 732:
                    states = ReadBytes(payload, 720, 12);
                    break;
            }

            offset += 12 + payloadLen;
        }

        if (seq is null || mods is null || algs is null)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        states ??= new int[12];
        chain = [];
        for (int pos = 0; pos < 12; pos++)
        {
            int slotId = seq[pos];
            if (slotId is < 0 or > 11)
            {
                chain.Add(new MatriboxChainSlot { Position = pos, ModuleType = -1 });
                continue;
            }

            int module = mods[slotId];
            chain.Add(new MatriboxChainSlot
            {
                Position = pos,
                ModuleType = module is >= 0 and <= 15 ? module : -1,
                Algorithm = algs[slotId],
                On = slotId < states.Length && states[slotId] != 0
            });
        }

        return true;
    }

    private static int[] ReadBytes(ReadOnlySpan<byte> data, int start, int count)
    {
        var a = new int[count];
        for (int i = 0; i < count && start + i < data.Length; i++)
        {
            a[i] = data[start + i];
        }

        return a;
    }

    private static bool TryLoadCompressed(string path, out byte[] compressed)
    {
        compressed = [];
        try
        {
            string text = File.ReadAllText(path).Trim();
            byte[] jsonBytes = text.StartsWith('[')
                ? Encoding.UTF8.GetBytes(text)
                : Convert.FromBase64String(text);
            int[]? nums = JsonSerializer.Deserialize<int[]>(jsonBytes);
            if (nums is null || nums.Length < 8)
            {
                return false;
            }

            compressed = [.. nums.Select(n => (byte)n)];
            return true;
        }
        catch
        {
            return false;
        }
    }
}
