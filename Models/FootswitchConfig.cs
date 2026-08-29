namespace ChocolateEditor.Models;

public sealed class FootswitchConfig
{
    public FootswitchId Id { get; set; }
    public SwitchBehavior Behavior { get; set; } = SwitchBehavior.ShortAndLong;
    public List<MidiCommand> ShortPress { get; set; } = [];
    public List<MidiCommand> LongPress { get; set; } = [];

    /// <summary>Modo custom simples: número CC 0–127 (ou função especial).</summary>
    public int CustomCc { get; set; }

    /// <summary>false = momentâneo, true = latching.</summary>
    public bool CustomLatching { get; set; }

    public FootswitchConfig Clone()
    {
        return new FootswitchConfig
        {
            Id = Id,
            Behavior = Behavior,
            CustomCc = CustomCc,
            CustomLatching = CustomLatching,
            ShortPress = ShortPress.Select(m => m.Clone()).ToList(),
            LongPress = LongPress.Select(m => m.Clone()).ToList()
        };
    }

    public void CopyFrom(FootswitchConfig source, bool copyShort, bool copyLong)
    {
        Behavior = source.Behavior;
        CustomCc = source.CustomCc;
        CustomLatching = source.CustomLatching;
        if (copyShort)
        {
            ShortPress = source.ShortPress.Select(m => m.Clone()).ToList();
        }

        if (copyLong)
        {
            LongPress = source.LongPress.Select(m => m.Clone()).ToList();
        }
    }
}
