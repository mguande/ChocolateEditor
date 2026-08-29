using System.Text.Json.Serialization;

namespace ChocolateEditor.Models;

public sealed class MidiCommand
{
    public MidiCommandType Type { get; set; } = MidiCommandType.ProgramChange;

    /// <summary>Canal MIDI 1–16.</summary>
    public int Channel { get; set; } = 1;

    /// <summary>PC, CC number ou nota.</summary>
    public int Data1 { get; set; }

    /// <summary>Valor CC ou velocity.</summary>
    public int Data2 { get; set; } = 127;

    /// <summary>Bytes SysEx em hex, com ou sem F0/F7.</summary>
    public string SysExHex { get; set; } = "";

    [JsonIgnore]
    public string Summary
    {
        get
        {
            return Type switch
            {
                MidiCommandType.ProgramChange => $"PC {Data1}  ch{Channel}",
                MidiCommandType.ControlChange => $"CC {Data1} = {Data2}  ch{Channel}",
                MidiCommandType.NoteOn => $"Note On {Data1} vel {Data2}  ch{Channel}",
                MidiCommandType.NoteOff => $"Note Off {Data1} vel {Data2}  ch{Channel}",
                MidiCommandType.SysEx => string.IsNullOrWhiteSpace(SysExHex) ? "SysEx (vazio)" : $"SysEx {SysExHex}",
                _ => Type.ToString()
            };
        }
    }

    public override string ToString() => Summary;

    public MidiCommand Clone()
    {
        return new MidiCommand
        {
            Type = Type,
            Channel = Channel,
            Data1 = Data1,
            Data2 = Data2,
            SysExHex = SysExHex
        };
    }

    public static MidiCommand ProgramChange(int program, int channel = 1)
    {
        return new MidiCommand
        {
            Type = MidiCommandType.ProgramChange,
            Channel = channel,
            Data1 = Math.Clamp(program, 0, 127)
        };
    }

    public static MidiCommand ControlChange(int cc, int value, int channel = 1)
    {
        return new MidiCommand
        {
            Type = MidiCommandType.ControlChange,
            Channel = channel,
            Data1 = Math.Clamp(cc, 0, 127),
            Data2 = Math.Clamp(value, 0, 127)
        };
    }
}
