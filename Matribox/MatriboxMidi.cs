using ChocolateEditor.Models;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Mapa MIDI da Matribox II Pro (firmware 1.2.2, manual oficial).
/// 60 bancos × 4 presets (A–D) = 240 slots.
/// CC 0 + PC escolhem o preset; CC 43–54 ligam/desligam os 12 módulos da cadeia.
/// </summary>
public static class MatriboxMidi
{
    public const int BankCount = 60;
    public const int PresetsPerBank = 4;
    public static readonly string[] Slots = ["A", "B", "C", "D"];

    public static readonly string[] ModuleNames =
    [
        "1  Slot 1 (CC 43)",
        "2  Slot 2 (CC 44)",
        "3  Slot 3 (CC 45)",
        "4  Slot 4 (CC 46)",
        "5  Slot 5 (CC 47)",
        "6  Slot 6 (CC 48)",
        "7  Slot 7 (CC 49)",
        "8  Slot 8 (CC 50)",
        "9  Slot 9 (CC 51)",
        "10 Slot 10 (CC 52)",
        "11 Slot 11 (CC 53)",
        "12 Slot 12 (CC 54)"
    ];

    public static int ModuleCc(int moduleIndex0) => 43 + moduleIndex0;

    public static string PresetLabel(int bank1, int slot0)
        => $"{bank1:00}-{Slots[slot0]}";

    /// <summary>
    /// CC0 0/1 escolhe a página USB; PC é o índice 0–119 dessa página
    /// (MIDI program 0 = primeiro preset). O manual escreve 1–120.
    /// </summary>
    public static (byte BankMsb, byte Program) Address(int bank1, int slot0)
    {
        int index = (bank1 - 1) * PresetsPerBank + slot0;
        byte msb = (byte)(index < 120 ? 0 : 1);
        byte pc = (byte)(index % 120);
        return (msb, pc);
    }

    public static List<MidiCommand> SelectPreset(int bank1, int slot0, int channel, int? lastBankMsb = null)
    {
        var (msb, pc) = Address(bank1, slot0);
        var list = new List<MidiCommand>();
        if (lastBankMsb is null || lastBankMsb != msb)
        {
            list.Add(MidiCommand.ControlChange(0, msb, channel));
        }

        list.Add(MidiCommand.ProgramChange(pc, channel));
        return list;
    }

    public static MidiCommand ModuleOnOff(int moduleIndex0, bool on, int channel)
        => MidiCommand.ControlChange(ModuleCc(moduleIndex0), on ? 127 : 0, channel);

    public static MidiCommand StompMode(int channel)
        => MidiCommand.ControlChange(29, 127, channel);

    public static bool TryParseSelectPreset(
        IEnumerable<MidiCommand> commands, out int bank1, out int slot0)
    {
        bank1 = 1;
        slot0 = 0;
        int msb = 0;
        int? program = null;
        foreach (var cmd in commands)
        {
            if (cmd.Type == MidiCommandType.ControlChange && cmd.Data1 == 0)
            {
                msb = cmd.Data2 == 1 ? 1 : 0;
            }
            else if (cmd.Type == MidiCommandType.ProgramChange && cmd.Data1 is >= 0 and <= 119)
            {
                program = cmd.Data1;
            }
        }

        if (program is null)
        {
            return false;
        }

        int index = msb * 120 + program.Value;
        if (index < 0 || index >= HotoneSysex.PresetCount)
        {
            return false;
        }

        bank1 = index / PresetsPerBank + 1;
        slot0 = index % PresetsPerBank;
        return true;
    }

    /// <summary>0 = sem default (vazio), 1 = Off, 2 = On no preset .prst.</summary>
    public static int PresetEffectChoice(IReadOnlyList<MatriboxChainSlot>? chain, int moduleIndex0)
    {
        if (chain is null || moduleIndex0 < 0 || moduleIndex0 >= chain.Count || chain[moduleIndex0].Empty)
        {
            return 0;
        }

        return chain[moduleIndex0].On ? 2 : 1;
    }

    /// <summary>0 = não enviar, 1 = Off, 2 = On.</summary>
    public static int EffectChoice(IEnumerable<MidiCommand> commands, int moduleIndex0)
    {
        int cc = ModuleCc(moduleIndex0);
        foreach (var cmd in commands.Reverse())
        {
            if (cmd.Type == MidiCommandType.ControlChange && cmd.Data1 == cc)
            {
                return cmd.Data2 >= 64 ? 2 : 1;
            }
        }

        return 0;
    }

    public static bool HasStomp(IEnumerable<MidiCommand> commands)
        => commands.Any(c => c.Type == MidiCommandType.ControlChange && c.Data1 == 29 && c.Data2 >= 64);
}
