using ChocolateEditor.Models;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Protocolo do editor Sonicake/Hotone (HTUSBTools): SysEx com magia ASCII "!%~GP-2".
/// Pedidos vão sem LZO; a resposta da lista de presets é 240 × (índice 4 + nome 17 + tipo 10).
/// </summary>
public static class HotoneSysex
{
    public static readonly byte[] Magic = [0x21, 0x25, 0x7E, 0x47, 0x50, 0x2D, 0x32];

    /// <summary>Cabeçalho do SysEx que a Matribox manda ao mudar de patch (não é o nome).</summary>
    public static readonly byte[] MagicMp = [0x21, 0x25, 0x4D, 0x50];

    public const int FlagRequest = 0x11;
    public const int FlagSend = 0x12;

    public const int AddrSoftwareState = unchecked((int)0x12000000);
    public const int AddrPreset = 0x10000000;
    public const int AddrPresetSignal = 0x10000001;
    public const int AddrPresetList = 0x010A0000;
    public const int AddrPresetIndex = 0x010A0001;
    public const int AddrHeartTick = 0x05000010;

    public const int PresetCount = 240;
    public const int NameSize = 17;
    public const int TypeSize = 10;
    public const int SlotCount = 12;

    public static readonly string[] ModuleTypeNames =
    [
        "DYN", "FREQ", "WAH", "DRV", "AMP", "CAB", "IR", "EQ",
        "MOD", "DLY", "RVB", "Clone", "FXLOOP", "FXSND", "FXRTN", "VOL"
    ];

    public static byte[] Le32(int value) =>
        [(byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24)];

    public static byte[] Be32(int value) =>
        [(byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value];

    public static int ReadLe32(ReadOnlySpan<byte> data, int offset = 0)
        => data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24);

    public static int ReadBe32(ReadOnlySpan<byte> data, int offset = 0)
        => (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];

    public static byte[] Build(int address, int flag, byte[] payload)
        => [.. Magic, (byte)flag, .. Be32(address), .. payload];

    public static bool TryParse(byte[] sysex, out int flag, out int address, out byte[] payload)
    {
        flag = 0;
        address = 0;
        payload = [];
        byte[] inner = Strip(sysex);
        if (inner.Length < Magic.Length + 5)
        {
            return false;
        }

        if (!inner.AsSpan(0, Magic.Length).SequenceEqual(Magic))
        {
            return false;
        }

        flag = inner[Magic.Length];
        address = ReadBe32(inner, Magic.Length + 1);
        payload = inner[(Magic.Length + 5)..];
        return true;
    }

    public static byte[] Strip(byte[] sysex)
    {
        int start = sysex.Length > 0 && sysex[0] == 0xF0 ? 1 : 0;
        int end = sysex.Length > 0 && sysex[^1] == 0xF7 ? sysex.Length - 1 : sysex.Length;
        return start >= end ? [] : sysex[start..end];
    }

    public static string AsciiZ(ReadOnlySpan<byte> data)
    {
        var chars = new List<char>(data.Length);
        foreach (byte b in data)
        {
            if (b == 0)
            {
                break;
            }

            if (b is >= 32 and < 127)
            {
                chars.Add((char)b);
            }
        }

        return new string([.. chars]).Trim();
    }

    public static string ModuleName(int type)
        => type >= 0 && type < ModuleTypeNames.Length
            ? ModuleTypeNames[type]
            : $"Mod {type}";

    public static string Hex(byte[] data, int max = 80)
    {
        int n = Math.Min(data.Length, max);
        string h = string.Join(" ", data.Take(n).Select(b => b.ToString("X2")));
        return data.Length > max ? h + "…" : h;
    }

    /// <summary>Maior sequência ASCII imprimível no pacote (nome do patch).</summary>
    public static string BestAscii(byte[] data)
    {
        string best = "";
        int i = 0;
        byte[] inner = Strip(data);
        while (i < inner.Length)
        {
            if (inner[i] is < 32 or >= 127)
            {
                i++;
                continue;
            }

            int start = i;
            while (i < inner.Length && inner[i] is >= 32 and < 127)
            {
                i++;
            }

            string s = System.Text.Encoding.ASCII.GetString(inner, start, i - start).Trim();
            if (IsHeaderNoise(s))
            {
                continue;
            }

            if (s.Length >= 2 && s.Any(char.IsLetter) && s.Length > best.Length)
            {
                best = s;
            }
        }

        return best;
    }

    public static byte[] MaybeNibbleDecode(byte[] inner)
    {
        byte[] d = inner;
        if (d.Length > 0 && d[^1] == 0xF7)
        {
            d = d[..^1];
        }

        if (d.Length < 8 || d.Length % 2 != 0)
        {
            return inner;
        }

        // Só nibble-decode se for mesmo 0–15. Um header 00 20 32 no MIDI class
        // não é GP-2 nibble-encoded.
        if (d.Any(b => b > 15))
        {
            return inner;
        }

        var decoded = new byte[d.Length / 2];
        for (int i = 0; i < decoded.Length; i++)
        {
            decoded[i] = (byte)((d[i * 2] << 4) | (d[i * 2 + 1] & 0x0F));
        }

        return decoded;
    }

    public static bool IsHeaderNoise(string text)
        => text.StartsWith("!%", StringComparison.Ordinal)
           || text.Contains("GP-2", StringComparison.Ordinal);

    /// <summary>
    /// SysEx !%MP: a pedaleira só avisa que o patch mudou. Não traz nome nem cadeia.
    /// O índice 1–240 vem nibble-encoded (~offset 38).
    /// </summary>
    public static bool TryParseMpNotify(byte[] raw, out int presetIndex1)
    {
        presetIndex1 = 0;
        byte[] inner = Strip(raw);
        if (inner.Length < 40 || !inner.AsSpan(0, MagicMp.Length).SequenceEqual(MagicMp))
        {
            return false;
        }

        byte hi = inner[38];
        byte lo = inner[39];
        if (hi > 15 || lo > 15)
        {
            return true;
        }

        int value = (hi << 4) | lo;
        if (value is >= 1 and <= PresetCount)
        {
            presetIndex1 = value;
        }

        return true;
    }

    public static List<MatriboxChainSlot> EmptyChain()
        => Enumerable.Range(0, SlotCount)
            .Select(i => new MatriboxChainSlot { Position = i, ModuleType = -1 })
            .ToList();

    public static bool TryParseLiveDump(byte[] raw, out string name, out List<MatriboxChainSlot> chain)
    {
        name = "";
        chain = [];
        byte[] inner = Strip(raw);
        if (inner.Length >= MagicMp.Length && inner.AsSpan(0, MagicMp.Length).SequenceEqual(MagicMp))
        {
            return false;
        }

        var buffers = new List<byte[]> { inner };
        byte[] nib = MaybeNibbleDecode(inner);
        if (!nib.AsSpan().SequenceEqual(inner))
        {
            buffers.Add(nib);
        }

        // ID de fabricante de 3 bytes (00 xx xx) ou universal 7E/7F.
        if (inner.Length > 8 && inner[0] == 0)
        {
            buffers.Add(inner[3..]);
        }

        if (inner.Length > 8 && inner[0] is 0x7E or 0x7F)
        {
            buffers.Add(inner[Math.Min(5, inner.Length - 1)..]);
        }

        LiveDumpCandidate? best = null;
        foreach (byte[] buf in buffers)
        {
            foreach (var parsed in TryLayouts(buf))
            {
                if (best is null || parsed.Score > best.Score)
                {
                    best = parsed;
                }
            }

            var heuristic = HeuristicDump(buf);
            if (heuristic is not null && (best is null || heuristic.Score > best.Score))
            {
                best = heuristic;
            }
        }

        if (best is null)
        {
            name = BestAscii(inner);
            return name.Length >= 2;
        }

        name = string.IsNullOrWhiteSpace(best.Name) ? BestAscii(inner) : best.Name;
        chain = best.ChainOk ? best.Chain : [];
        return name.Length >= 2 || chain.Count == SlotCount;
    }

    private sealed class LiveDumpCandidate
    {
        public int Score { get; init; }
        public string Name { get; init; } = "";
        public List<MatriboxChainSlot> Chain { get; init; } = [];
        public bool ChainOk { get; init; }
    }

    private static IEnumerable<LiveDumpCandidate> TryLayouts(byte[] buf)
    {
        int n = buf.Length;
        int[] nameLens = [17, 16];
        foreach (int nameLen in nameLens)
        {
            if (n >= nameLen + 36)
            {
                yield return ScoreLayout(buf, nameAt: 0, nameLen, seqAt: nameLen, modAt: nameLen + 12, onAt: nameLen + 24);
                yield return ScoreLayout(buf, nameAt: 0, nameLen, seqAt: -1, modAt: nameLen, onAt: nameLen + 24, algAt: nameLen + 12);
            }
        }

        if (n >= 53)
        {
            yield return ScoreLayout(buf, nameAt: 36, 17, seqAt: 0, modAt: 12, onAt: 24);
        }

        if (n >= 48)
        {
            yield return ScoreLayout(buf, nameAt: -1, 0, seqAt: 0, modAt: 12, onAt: 36, algAt: 24);
        }

        if (n >= 41)
        {
            yield return ScoreLayout(buf, nameAt: 0, 17, seqAt: 17, modAt: 29, onAt: -1);
        }
    }

    private static LiveDumpCandidate ScoreLayout(
        byte[] buf, int nameAt, int nameLen, int seqAt, int modAt, int onAt, int algAt = -1)
    {
        const int slots = SlotCount;
        string name = "";
        if (nameAt >= 0 && nameLen > 0 && nameAt + 2 <= buf.Length)
        {
            int take = Math.Min(nameLen, buf.Length - nameAt);
            name = AsciiZ(buf.AsSpan(nameAt, take));
        }

        byte[] seq = seqAt >= 0 ? Slice(buf, seqAt, slots) : Enumerable.Range(0, slots).Select(i => (byte)i).ToArray();
        byte[] mod = Slice(buf, modAt, slots);
        byte[] on = onAt >= 0 ? Slice(buf, onAt, slots) : Enumerable.Repeat((byte)1, slots).ToArray();

        int score = 0;
        if (name.Length >= 2 && name.Any(char.IsLetter))
        {
            score += 10 + Math.Min(8, name.Length);
        }

        bool modsOk = mod.Length == slots && mod.All(b => b <= 16) && mod.Distinct().Count() >= 3;
        if (modsOk)
        {
            score += 8;
        }

        int seqHits = seq.Count(b => b < slots);
        bool seqOk = seqHits >= 10 && seq.Take(slots).Distinct().Count() >= 8;
        if (seqOk)
        {
            score += 6;
        }

        var chain = BuildChain(seq, mod, on, buf, algAt);
        bool chainOk = modsOk && (seqAt < 0 || seqOk);
        return new LiveDumpCandidate { Score = score, Name = name, Chain = chain, ChainOk = chainOk };
    }

    private static LiveDumpCandidate? HeuristicDump(byte[] buf)
    {
        if (buf.Length < SlotCount)
        {
            return null;
        }

        string name = BestAscii(buf);
        int score = name.Length >= 2 && name.Any(char.IsLetter) ? 10 + Math.Min(8, name.Length) : 0;

        int bestSeqAt = -1, bestSeqScore = 0;
        int bestModAt = -1, bestModScore = 0;
        int bestOnAt = -1, bestOnScore = 0;
        for (int i = 0; i + SlotCount <= buf.Length; i++)
        {
            var w = buf.AsSpan(i, SlotCount);
            int seqHits = 0;
            var seen = new HashSet<int>();
            bool mods = true;
            bool ons = true;
            int onHi = 0;
            for (int k = 0; k < SlotCount; k++)
            {
                byte b = w[k];
                if (b < SlotCount)
                {
                    seqHits++;
                    seen.Add(b);
                }

                if (b > 16)
                {
                    mods = false;
                }

                if (b is not (0 or 1 or 64 or 127))
                {
                    ons = false;
                }

                if (b >= 64)
                {
                    onHi++;
                }
            }

            int seqScore = seqHits >= 10 ? seqHits + seen.Count * 2 : 0;
            if (seqScore > bestSeqScore)
            {
                bestSeqScore = seqScore;
                bestSeqAt = i;
            }

            int modScore = mods ? w.ToArray().Distinct().Count() : 0;
            if (modScore >= 3 && modScore > bestModScore)
            {
                bestModScore = modScore;
                bestModAt = i;
            }

            int onScore = ons ? 4 + onHi : 0;
            if (onScore > bestOnScore)
            {
                bestOnScore = onScore;
                bestOnAt = i;
            }
        }

        if (bestSeqScore > 0)
        {
            score += 6;
        }

        if (bestModScore >= 3)
        {
            score += 8;
        }

        byte[] seq = bestSeqAt >= 0
            ? Slice(buf, bestSeqAt, SlotCount)
            : Enumerable.Range(0, SlotCount).Select(i => (byte)i).ToArray();
        byte[] mod = bestModAt >= 0 ? Slice(buf, bestModAt, SlotCount) : new byte[SlotCount];
        byte[] on = bestOnAt >= 0 ? Slice(buf, bestOnAt, SlotCount) : Enumerable.Repeat((byte)1, SlotCount).ToArray();
        bool chainOk = bestModAt >= 0 && bestModScore >= 3;
        return new LiveDumpCandidate
        {
            Score = score,
            Name = name,
            Chain = chainOk ? BuildChain(seq, mod, on, buf, -1) : [],
            ChainOk = chainOk
        };
    }

    private static List<MatriboxChainSlot> BuildChain(
        byte[] seq, byte[] mod, byte[] on, byte[] buf, int algAt)
    {
        const int slots = SlotCount;
        bool midiOn = on.Any(b => b >= 64);
        var chain = new List<MatriboxChainSlot>(slots);
        for (int pos = 0; pos < slots; pos++)
        {
            int slotId = seq[pos];
            if (slotId < 0 || slotId >= slots)
            {
                slotId = pos;
            }

            int alg = 0;
            if (algAt >= 0 && algAt + (slotId + 1) * 4 <= buf.Length)
            {
                alg = ReadLe32(buf, algAt + slotId * 4);
            }
            else if (algAt >= 0 && algAt + slotId < buf.Length)
            {
                alg = buf[algAt + slotId];
            }

            byte onB = on[slotId];
            chain.Add(new MatriboxChainSlot
            {
                Position = pos,
                ModuleType = mod[slotId],
                Algorithm = alg,
                On = midiOn ? onB >= 64 : onB != 0
            });
        }

        return chain;
    }

    private static byte[] Slice(byte[] buf, int offset, int length)
    {
        var a = new byte[length];
        if (offset < 0)
        {
            return a;
        }

        int n = Math.Max(0, Math.Min(length, buf.Length - offset));
        if (n > 0)
        {
            Buffer.BlockCopy(buf, offset, a, 0, n);
        }

        return a;
    }
}

public sealed class MatriboxPresetInfo
{
    public int Index { get; init; }
    public string Name { get; init; } = "";
    public string Style { get; init; } = "";
    public IReadOnlyList<MatriboxChainSlot>? Chain { get; init; }

    public int Bank1 => Index / 4 + 1;
    public int Slot0 => Index % 4;
    public string SlotLabel => MatriboxMidi.PresetLabel(Bank1, Slot0);
    public string Display => string.IsNullOrWhiteSpace(Name) || HotoneSysex.IsHeaderNoise(Name)
        ? SlotLabel
        : $"{SlotLabel}  {Name}";
}

public sealed class MatriboxChainSlot
{
    public int Position { get; init; }
    public int ModuleType { get; init; } = -1;
    public int Algorithm { get; init; }
    public bool On { get; init; }
    public bool Empty => ModuleType < 0 || ModuleType > 15;
    public string Label
    {
        get
        {
            if (Empty)
            {
                return $"--  (CC {43 + Position})";
            }

            string alg = MatriboxAlgCatalog.Name(Algorithm);
            string head = string.IsNullOrWhiteSpace(alg)
                ? HotoneSysex.ModuleName(ModuleType)
                : $"{HotoneSysex.ModuleName(ModuleType)}  {alg}";
            return $"{head}  {(On ? "On" : "Off")}  (CC {43 + Position})";
        }
    }

    public string NameLabel
    {
        get
        {
            if (Empty)
            {
                return $"--  (CC {43 + Position})";
            }

            string alg = MatriboxAlgCatalog.Name(Algorithm);
            string head = string.IsNullOrWhiteSpace(alg)
                ? HotoneSysex.ModuleName(ModuleType)
                : $"{HotoneSysex.ModuleName(ModuleType)}  {alg}";
            return $"{head}  (CC {43 + Position})";
        }
    }
}
