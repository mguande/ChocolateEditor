namespace ChocolateEditor.Models;

/// <summary>
/// Byte 0 da EEPROM = número do modo no CubeSuite menos 1
/// (manual Chocolate Plus: modos 1–12).
/// </summary>
public enum DeviceMode : byte
{
    ProgramChangeA = 0x00,
    ProgramChangeB = 0x01,
    Custom = 0x02,
    AdvancedCustom = 0x03,
    AdvancedCustom2 = 0x04,
    ManufacturerControl = 0x05,
    Touchscreen = 0x06,
    Video = 0x07,
    KeyboardA = 0x08,
    KeyboardB = 0x09,
    Multimedia = 0x0A,
    CustomKeyboard = 0x0B
}

public enum MidiCommandType
{
    ProgramChange,
    ControlChange,
    NoteOn,
    NoteOff,
    SysEx
}

public enum SwitchBehavior
{
    ShortTap,
    ShortTapToggle,
    PressRelease,
    LongPress,
    ShortAndLong
}

public enum FootswitchId
{
    A = 0,
    B = 1,
    C = 2,
    D = 3
}

public enum TrsJackMode : byte
{
    Expression = 0,
    Midi = 1
}
