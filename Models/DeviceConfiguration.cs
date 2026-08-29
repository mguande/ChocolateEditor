using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChocolateEditor.Models;

public sealed class DeviceConfiguration
{
    public const int MaxBanks = 16;
    public const string FileFilter = "Banco Chocolate (*.choco.json)|*.choco.json|JSON (*.json)|*.json|Todos (*.*)|*.*";

    public string DeviceName { get; set; } = "M-VAVE Chocolate Plus";
    public DeviceMode Mode { get; set; } = DeviceMode.AdvancedCustom;
    public int BankCount { get; set; } = MaxBanks;
    public TrsJackMode TrsMode { get; set; } = TrsJackMode.Expression;
    public List<Bank> Banks { get; set; } = [];

    /// <summary>Dump bruto da EEPROM (memória 0x04), se lido do USB.</summary>
    public byte[]? RawMemory { get; set; }

    public static DeviceConfiguration CreateStarter()
    {
        var config = new DeviceConfiguration
        {
            Mode = DeviceMode.AdvancedCustom,
            BankCount = 8
        };

        for (int i = 0; i < DeviceConfiguration.MaxBanks; i++)
        {
            var bank = new Bank
            {
                Index = i,
                Name = $"Banco {i + 1}"
            };

            int basePc = i * 4;
            bank.A.ShortPress.Add(MidiCommand.ProgramChange(basePc));
            bank.B.ShortPress.Add(MidiCommand.ProgramChange(basePc + 1));
            bank.C.ShortPress.Add(MidiCommand.ProgramChange(basePc + 2));
            bank.D.ShortPress.Add(MidiCommand.ProgramChange(basePc + 3));

            bank.A.LongPress.Add(MidiCommand.ControlChange(80, 127));
            bank.B.LongPress.Add(MidiCommand.ControlChange(81, 127));
            bank.C.LongPress.Add(MidiCommand.ControlChange(82, 127));
            bank.D.LongPress.Add(MidiCommand.ControlChange(83, 127));

            bank.A.CustomCc = basePc;
            bank.B.CustomCc = basePc + 1;
            bank.C.CustomCc = basePc + 2;
            bank.D.CustomCc = basePc + 3;

            config.Banks.Add(bank);
        }

        return config;
    }

    public Bank GetBank(int index)
    {
        EnsureBankCount();
        index = Math.Clamp(index, 0, MaxBanks - 1);
        return Banks[index];
    }

    public void CopyBank(int fromIndex, int toIndex, bool copyA, bool copyB, bool copyC, bool copyD, bool copyShort, bool copyLong)
    {
        EnsureBankCount();
        fromIndex = Math.Clamp(fromIndex, 0, MaxBanks - 1);
        toIndex = Math.Clamp(toIndex, 0, MaxBanks - 1);
        if (fromIndex == toIndex)
        {
            return;
        }

        Banks[toIndex].CopyFrom(Banks[fromIndex], copyA, copyB, copyC, copyD, copyShort, copyLong);
        if (string.IsNullOrWhiteSpace(Banks[toIndex].Name) || Banks[toIndex].Name.StartsWith("Banco ", StringComparison.Ordinal))
        {
            Banks[toIndex].Name = Banks[fromIndex].Name + " (cópia)";
        }
    }

    public void CopyBankToRange(int fromIndex, int toStart, int toEnd, bool copyA, bool copyB, bool copyC, bool copyD, bool copyShort, bool copyLong)
    {
        int start = Math.Min(toStart, toEnd);
        int end = Math.Max(toStart, toEnd);
        for (int i = start; i <= end; i++)
        {
            if (i != fromIndex)
            {
                CopyBank(fromIndex, i, copyA, copyB, copyC, copyD, copyShort, copyLong);
            }
        }
    }

    public void EnsureBankCount()
    {
        BankCount = Math.Clamp(BankCount, 1, MaxBanks);
        while (Banks.Count < MaxBanks)
        {
            int i = Banks.Count;
            Banks.Add(new Bank { Index = i, Name = $"Banco {i + 1}" });
        }

        for (int i = 0; i < Banks.Count; i++)
        {
            Banks[i].Index = i;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, JsonOptions);
    }

    public static DeviceConfiguration FromJson(string json)
    {
        var config = JsonSerializer.Deserialize<DeviceConfiguration>(json, JsonOptions) ?? CreateStarter();
        config.EnsureBankCount();
        return config;
    }

    public void SaveToFile(string path)
    {
        File.WriteAllText(path, ToJson());
    }

    public static DeviceConfiguration LoadFromFile(string path)
    {
        return FromJson(File.ReadAllText(path));
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };
}
