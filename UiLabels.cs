using ChocolateEditor.Models;

namespace ChocolateEditor;

internal static class UiLabels
{
    public static string Mode(DeviceMode mode) => mode switch
    {
        DeviceMode.ProgramChangeA => "1. Program Change A (PC 0–127)",
        DeviceMode.ProgramChangeB => "2. Program Change B (CC)",
        DeviceMode.Custom => "3. Custom (CC toggle)",
        DeviceMode.AdvancedCustom => "4. Advanced Custom 1",
        DeviceMode.AdvancedCustom2 => "5. Advanced Custom 2 (E/F bancos)",
        DeviceMode.ManufacturerControl => "6. Controle do fabricante",
        DeviceMode.Touchscreen => "7. Touchscreen",
        DeviceMode.Video => "8. Vídeo",
        DeviceMode.KeyboardA => "9. Teclado A",
        DeviceMode.KeyboardB => "10. Teclado B",
        DeviceMode.Multimedia => "11. Multimídia",
        DeviceMode.CustomKeyboard => "12. Teclado custom",
        _ => mode.ToString()
    };

    public static string Behavior(SwitchBehavior behavior) => behavior switch
    {
        SwitchBehavior.ShortTap => "Single step (1 grupo)",
        SwitchBehavior.ShortTapToggle => "Single step (alterna 2 grupos)",
        SwitchBehavior.PressRelease => "Pressionar / soltar (2 grupos)",
        SwitchBehavior.LongPress => "Long step",
        SwitchBehavior.ShortAndLong => "Step short or step long",
        _ => behavior.ToString()
    };

    public static string CommandType(MidiCommandType type) => type switch
    {
        MidiCommandType.ProgramChange => "Program Change (PC)",
        MidiCommandType.ControlChange => "Control Change (CC)",
        MidiCommandType.NoteOn => "Note On",
        MidiCommandType.NoteOff => "Note Off",
        MidiCommandType.SysEx => "SysEx",
        _ => type.ToString()
    };
}
