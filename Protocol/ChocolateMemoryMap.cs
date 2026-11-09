using ChocolateEditor.Models;

namespace ChocolateEditor.Protocol;

/// <summary>
/// EEPROM 0x04: cada switch (A–D) ocupa 0x1A1 bytes (161 de listas + 256 padding).
/// Cada grupo tem os 4 switches em sequência; o grupo seguinte começa 4 × 0x1A1 depois.
/// </summary>
public static class ChocolateMemoryMap
{
    public const byte MemoryType = 0x04;
    public const int CubeSuiteSize = 0x742;
    public const int SwitchStride = 0x1A1;
    public const int SwitchCount = 4;
    public const int GroupStride = SwitchStride * SwitchCount;
    public const int MaxGroupsInDump = 8;
    public const int AddrMode = 0x00;
    public const int AddrTrs = 0x0C;
    public const int GroupBase = 0x0D;
    public const int BankAOffset = 0x5E;
    public const int BankBOffset = 0xAE;
    public const int ListBytes = 0x50;
    public const int MessageSize = 5;
    public const int MessagesPerList = 16;

    /// <summary>8 grupos × 4 switches, até ao B Bank do Switch D do grupo 8.</summary>
    public const int TotalSize = 0x3380;

    public static byte[] CreateEmptyDump()
    {
        var dump = new byte[TotalSize];
        dump[AddrMode] = (byte)DeviceMode.AdvancedCustom;
        dump[AddrTrs] = 1;
        return dump;
    }

    public static void ApplyConfiguration(byte[] dump, DeviceConfiguration config)
    {
        dump = Pad(dump);
        config.EnsureBankCount();
        dump[AddrMode] = (byte)config.Mode;
        dump[AddrTrs] = (byte)config.TrsMode;

        int groups = Math.Min(config.BankCount, MaxGroupsInDump);
        for (int g = 0; g < groups; g++)
        {
            var bank = config.GetBank(g);
            if (g == 0 && GroupBase < dump.Length)
            {
                dump[GroupBase] = (byte)bank.A.Behavior;
            }

            int s = 0;
            foreach (var sw in bank.Switches)
            {
                if (TryOffsets(g, s, dump.Length, out int aOff, out int bOff))
                {
                    int behOff = aOff - 1;
                    if (behOff >= 0)
                    {
                        dump[behOff] = (byte)sw.Behavior;
                    }

                    PackList(Slice(dump, aOff), sw.ShortPress);
                    PackList(Slice(dump, bOff), sw.LongPress);
                }

                s++;
            }
        }
    }

    public static DeviceConfiguration ParseDump(byte[] dump)
    {
        dump = Pad(dump);
        var config = new DeviceConfiguration
        {
            RawMemory = dump,
            Mode = dump[AddrMode] <= 0x0B ? (DeviceMode)dump[AddrMode] : DeviceMode.AdvancedCustom,
            TrsMode = dump[AddrTrs] == 0 ? TrsJackMode.Expression : TrsJackMode.Midi,
            BankCount = MaxGroupsInDump
        };
        config.EnsureBankCount();

        var fallbackBehavior = GroupBase < dump.Length
            ? (SwitchBehavior)Math.Clamp(dump[GroupBase], (byte)0, (byte)4)
            : SwitchBehavior.ShortTapToggle;

        for (int g = 0; g < MaxGroupsInDump; g++)
        {
            var bank = config.GetBank(g);
            bank.Name = $"Grupo {g + 1}";
            int s = 0;
            foreach (var sw in bank.Switches)
            {
                sw.Behavior = fallbackBehavior;
                sw.ShortPress.Clear();
                sw.LongPress.Clear();
                if (TryOffsets(g, s, dump.Length, out int aOff, out int bOff))
                {
                    int behOff = aOff - 1;
                    if (behOff >= 0 && behOff < dump.Length && dump[behOff] <= (byte)SwitchBehavior.ShortAndLong)
                    {
                        sw.Behavior = (SwitchBehavior)dump[behOff];
                    }

                    sw.ShortPress = UnpackAt(dump, aOff);
                    sw.LongPress = UnpackAt(dump, bOff);
                }

                s++;
            }
        }

        return config;
    }

    public static int CountMessages(DeviceConfiguration config)
    {
        return config.Banks.Take(config.BankCount).Sum(b =>
            b.Switches.Sum(sw => sw.ShortPress.Count + sw.LongPress.Count));
    }

    public static int UsedDumpLength(byte[] dump)
    {
        int last = CubeSuiteSize;
        int n = Math.Min(dump.Length, TotalSize);
        for (int i = n - 1; i >= CubeSuiteSize; i--)
        {
            if (dump[i] != 0)
            {
                last = i + 1;
                break;
            }
        }

        return last;
    }

    private static bool TryOffsets(int group, int switchIndex, int dumpLength, out int aOff, out int bOff)
    {
        aOff = BankAOffset + group * GroupStride + switchIndex * SwitchStride;
        bOff = BankBOffset + group * GroupStride + switchIndex * SwitchStride;
        return switchIndex is >= 0 and < SwitchCount
               && aOff >= 0
               && aOff + MessageSize <= dumpLength;
    }

    private static byte[] Pad(byte[] dump)
    {
        if (dump.Length >= TotalSize)
        {
            return dump;
        }

        var grown = new byte[TotalSize];
        Buffer.BlockCopy(dump, 0, grown, 0, dump.Length);
        return grown;
    }

    private static Span<byte> Slice(byte[] dump, int off)
    {
        int n = Math.Max(0, Math.Min(ListBytes, dump.Length - off));
        return dump.AsSpan(off, n);
    }

    private static void PackList(Span<byte> dest, List<MidiCommand> commands)
    {
        dest.Clear();
        int n = Math.Min(commands.Count, dest.Length / MessageSize);
        for (int i = 0; i < n; i++)
        {
            var cmd = commands[i];
            int o = i * MessageSize;
            dest[o] = 1;
            dest[o + 1] = (byte)Math.Clamp(cmd.Channel - 1, 0, 15);
            dest[o + 2] = (byte)cmd.Type;
            dest[o + 3] = (byte)Math.Clamp(cmd.Data1, 0, 127);
            dest[o + 4] = (byte)Math.Clamp(cmd.Data2, 0, 127);
        }
    }

    private static List<MidiCommand> UnpackAt(byte[] dump, int off)
    {
        if (off < 0 || off >= dump.Length)
        {
            return [];
        }

        int n = Math.Min(ListBytes, dump.Length - off);
        if (n < MessageSize)
        {
            return [];
        }

        return UnpackPacked(dump.AsSpan(off, n));
    }

    private static List<MidiCommand> UnpackPacked(ReadOnlySpan<byte> src)
    {
        var list = new List<MidiCommand>();
        for (int i = 0; i + MessageSize <= src.Length && list.Count < MessagesPerList; i += MessageSize)
        {
            if (src[i] == 0)
            {
                break;
            }

            byte channel0 = src[i + 1];
            byte type = src[i + 2];
            if (channel0 > 15 || type > (byte)MidiCommandType.SysEx)
            {
                break;
            }

            list.Add(new MidiCommand
            {
                Type = (MidiCommandType)type,
                Channel = channel0 + 1,
                Data1 = src[i + 3],
                Data2 = src[i + 4]
            });
        }

        return list;
    }
}
