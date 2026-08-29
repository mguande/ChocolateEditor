using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using ChocolateEditor.Models;

namespace ChocolateEditor.Protocol;

public sealed class MidiPortInfo
{
    public required string Name { get; init; }
    public bool IsInput { get; init; }
    public bool IsOutput { get; init; }
    public bool LooksLikeChocolate { get; init; }

    public override string ToString() => Name;
}

public sealed class ChocolateUsbClient : IDisposable
{
    private static readonly string[] NameHints =
    [
        "sinco", "footctrl", "foot ctrl", "chocolate", "m-vave", "mvave", "cuvave", "m vave"
    ];

    private const int MemoryChunk = 128;

    private InputDevice? _input;
    private OutputDevice? _output;
    private readonly List<byte[]> _incoming = [];
    private readonly List<byte> _sysexAcc = [];
    private readonly object _gate = new();

    public bool IsConnected => _input is not null && _output is not null;
    public string? ConnectedName { get; private set; }
    public event EventHandler<string>? Log;

    public static IReadOnlyList<MidiPortInfo> ListPorts()
    {
        var map = new Dictionary<string, MidiPortInfo>(StringComparer.OrdinalIgnoreCase);

        void Add(string name, bool input, bool output)
        {
            if (!map.TryGetValue(name, out var info))
            {
                info = new MidiPortInfo
                {
                    Name = name,
                    LooksLikeChocolate = NameHints.Any(h => name.Contains(h, StringComparison.OrdinalIgnoreCase))
                };
                map[name] = info;
            }

            map[name] = new MidiPortInfo
            {
                Name = name,
                IsInput = info.IsInput || input,
                IsOutput = info.IsOutput || output,
                LooksLikeChocolate = info.LooksLikeChocolate
            };
        }

        foreach (var d in InputDevice.GetAll())
        {
            Add(d.Name, true, false);
            d.Dispose();
        }

        foreach (var d in OutputDevice.GetAll())
        {
            Add(d.Name, false, true);
            d.Dispose();
        }

        return map.Values
            .OrderByDescending(p => p.LooksLikeChocolate)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public void Connect(string deviceName)
    {
        Disconnect();
        _output = OpenOutput(deviceName) ?? throw new InvalidOperationException($"Saída MIDI '{deviceName}' não encontrada.");
        _input = OpenInput(deviceName) ?? throw new InvalidOperationException($"Entrada MIDI '{deviceName}' não encontrada. O Windows precisa ver o Chocolate como MIDI IN e OUT.");
        _input.EventReceived += OnEventReceived;
        _input.StartEventsListening();
        ConnectedName = deviceName;
        RaiseLog($"Conectado: IN={_input.Name}  OUT={_output.Name}");
    }

    private static OutputDevice? OpenOutput(string name)
    {
        try
        {
            return OutputDevice.GetByName(name);
        }
        catch
        {
            // tenta nome parecido
        }

        foreach (var device in OutputDevice.GetAll())
        {
            if (NamesMatch(device.Name, name))
            {
                return device;
            }

            device.Dispose();
        }

        return null;
    }

    private static InputDevice? OpenInput(string name)
    {
        try
        {
            return InputDevice.GetByName(name);
        }
        catch
        {
            // tenta nome parecido
        }

        foreach (var device in InputDevice.GetAll())
        {
            if (NamesMatch(device.Name, name))
            {
                return device;
            }

            device.Dispose();
        }

        return null;
    }

    private static bool NamesMatch(string a, string b)
    {
        return a.Equals(b, StringComparison.OrdinalIgnoreCase)
               || a.Contains(b, StringComparison.OrdinalIgnoreCase)
               || b.Contains(a, StringComparison.OrdinalIgnoreCase);
    }

    public void Disconnect()
    {
        if (_input is not null)
        {
            try
            {
                _input.EventReceived -= OnEventReceived;
                _input.StopEventsListening();
                _input.Dispose();
            }
            catch
            {
                // ignore
            }

            _input = null;
        }

        _output?.Dispose();
        _output = null;
        ConnectedName = null;
        lock (_gate)
        {
            _incoming.Clear();
            _sysexAcc.Clear();
        }
    }

    public async Task<string?> HandshakeAsync(CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        Drain();
        Send(MvaveMessage.Discover());
        var reply = await WaitForAsync(
            m => m is NameVersionResponse or AckMessage or UnknownMessage,
            TimeSpan.FromSeconds(2),
            cancellationToken);

        return reply switch
        {
            NameVersionResponse nv => nv.Name,
            _ => ConnectedName
        };
    }

    public async Task<DeviceConfiguration> ReadConfigurationAsync(CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        await HandshakeAsync(cancellationToken);
        byte[] dump = ChocolateMemoryMap.CreateEmptyDump();
        int addr = 0;
        int failed = 0;
        int consecutiveFail = 0;
        while (addr < ChocolateMemoryMap.TotalSize)
        {
            int len = Math.Min(MemoryChunk, ChocolateMemoryMap.TotalSize - addr);
            bool pastOfficial = addr >= ChocolateMemoryMap.CubeSuiteSize;
            var wait = pastOfficial ? TimeSpan.FromSeconds(1) : TimeSpan.FromSeconds(4);
            try
            {
                var content = await ReadMemoryContentAsync(
                    ChocolateMemoryMap.MemoryType,
                    (uint)addr,
                    (uint)len,
                    cancellationToken,
                    wait);

                consecutiveFail = 0;
                if (content.Data.Length > len)
                {
                    int dest = (int)content.Address;
                    if (dest < 0 || dest >= dump.Length)
                    {
                        dest = addr;
                    }

                    int copy = Math.Min(content.Data.Length, dump.Length - dest);
                    Buffer.BlockCopy(content.Data, 0, dump, dest, copy);
                    addr = dest + copy;
                    continue;
                }

                int dest2 = content.Address == (uint)addr ? (int)content.Address : addr;
                int n = Math.Min(Math.Min(content.Data.Length, len), dump.Length - dest2);
                Buffer.BlockCopy(content.Data, 0, dump, dest2, n);
            }
            catch (TimeoutException)
            {
                failed++;
                consecutiveFail++;
                RaiseLog($"Read 0x{addr:X4}  timeout");
                if (pastOfficial && consecutiveFail >= 3)
                {
                    RaiseLog("A parar a leitura extra: o pedal não tem mais memória mapeada.");
                    break;
                }

                if (!pastOfficial)
                {
                    throw;
                }
            }

            addr += len;
            await Task.Delay(20, cancellationToken);
        }

        RaiseLog($"Dump: {dump.Length} bytes, {failed} blocos sem resposta. Modo EEPROM 0x{dump[0]:X2}");

        var dumpPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ChocolateEditor",
            "last-dump.bin");
        Directory.CreateDirectory(Path.GetDirectoryName(dumpPath)!);
        File.WriteAllBytes(dumpPath, dump);
        RaiseLog("Dump salvo em " + dumpPath);

        var config = ChocolateMemoryMap.ParseDump(dump);
        for (int g = 0; g < ChocolateMemoryMap.MaxGroupsInDump; g++)
        {
            var bank = config.GetBank(g);
            foreach (var sw in bank.Switches)
            {
                if (sw.ShortPress.Count + sw.LongPress.Count == 0)
                {
                    continue;
                }

                RaiseLog($"Grupo {g + 1} Switch {sw.Id}: A Bank {sw.ShortPress.Count}  B Bank {sw.LongPress.Count}");
                foreach (var cmd in sw.ShortPress.Take(16))
                {
                    RaiseLog("    A  " + cmd.Summary);
                }

                foreach (var cmd in sw.LongPress.Take(16))
                {
                    RaiseLog("    B  " + cmd.Summary);
                }
            }
        }

        RaiseLog($"Total: {ChocolateMemoryMap.CountMessages(config)} mensagens MIDI.");
        return config;
    }

    public async Task WriteConfigurationAsync(DeviceConfiguration config, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        config.EnsureBankCount();
        byte[] dump = config.RawMemory is { Length: >= ChocolateMemoryMap.TotalSize }
            ? (byte[])config.RawMemory.Clone()
            : ChocolateMemoryMap.CreateEmptyDump();

        if (dump.Length < ChocolateMemoryMap.TotalSize)
        {
            Array.Resize(ref dump, ChocolateMemoryMap.TotalSize);
        }

        ChocolateMemoryMap.ApplyConfiguration(dump, config);
        config.RawMemory = dump;

        await HandshakeAsync(cancellationToken);

        int writeLen = ChocolateMemoryMap.UsedDumpLength(dump);
        await WriteMemoryAsync(ChocolateMemoryMap.MemoryType, 0, dump[..writeLen], cancellationToken);
        await WriteMemoryAsync(ChocolateMemoryMap.MemoryType, ChocolateMemoryMap.AddrMode, [(byte)config.Mode], cancellationToken);
        await WriteMemoryAsync(ChocolateMemoryMap.MemoryType, ChocolateMemoryMap.AddrTrs, [(byte)config.TrsMode], cancellationToken);

        RaiseLog("Configuração gravada no Chocolate.");
    }

    public async Task<byte[]> ReadMemoryAsync(byte memType, uint address, uint length, CancellationToken cancellationToken)
    {
        var content = await ReadMemoryContentAsync(memType, address, length, cancellationToken, TimeSpan.FromSeconds(4));
        byte[] data = content.Data.Length <= (int)length
            ? content.Data
            : content.Data[..(int)length];
        return data;
    }

    private async Task<MemoryContentMessage> ReadMemoryContentAsync(
        byte memType,
        uint address,
        uint length,
        CancellationToken cancellationToken,
        TimeSpan timeout)
    {
        Drain();
        Send(MvaveMessage.ReadMemory(memType, address, length));
        var reply = await WaitForAsync(
            m => m is MemoryContentMessage content
                 && content.Address == address
                 && content.Data.Length >= (int)length,
            timeout,
            cancellationToken);

        if (reply is not MemoryContentMessage content)
        {
            throw new TimeoutException($"O Chocolate não devolveu {length} bytes em 0x{address:X}.");
        }

        RaiseLog($"Read 0x{content.Address:X4}  {content.Data.Length} bytes (pedido {length} em 0x{address:X4})");
        return content;
    }

    public async Task WriteMemoryAsync(byte memType, uint address, byte[] data, CancellationToken cancellationToken)
    {
        for (int i = 0; i < data.Length; i += MemoryChunk)
        {
            int n = Math.Min(MemoryChunk, data.Length - i);
            byte[] slice = data.AsSpan(i, n).ToArray();
            Drain();
            Send(MvaveMessage.WriteMemory(memType, address + (uint)i, slice));
            var ack = await WaitForAsync(m => m is AckMessage, TimeSpan.FromSeconds(3), cancellationToken);
            RaiseLog(ack is null
                ? $"Write 0x{address + (uint)i:X4}  {n} bytes (sem ACK)"
                : $"Write 0x{address + (uint)i:X4}  {n} bytes");
            await Task.Delay(25, cancellationToken);
        }
    }

    public void Dispose() => Disconnect();

    private void Send(MvaveMessage message)
    {
        EnsureConnected();
        byte[] sysex = message.ToSysex();
        byte[] dryWet = sysex[1..]; // sem F0
        _output!.SendEvent(new NormalSysExEvent(dryWet));
        RaiseLog("TX " + FormatHex(sysex));
    }

    private void OnEventReceived(object? sender, MidiEventReceivedEventArgs e)
    {
        if (e.Event is not SysExEvent sysEx)
        {
            return;
        }

        byte[] data = sysEx.Data ?? [];
        lock (_gate)
        {
            if (e.Event is EscapeSysExEvent)
            {
                _sysexAcc.AddRange(data);
            }
            else
            {
                _sysexAcc.Clear();
                _sysexAcc.Add(MvaveCodec.SysexStart);
                int i = 0;
                if (data.Length > 0 && data[0] == MvaveCodec.SysexStart)
                {
                    i = 1;
                }

                for (; i < data.Length; i++)
                {
                    _sysexAcc.Add(data[i]);
                }
            }

            if (_sysexAcc.Count < 2 || _sysexAcc[^1] != MvaveCodec.SysexEnd)
            {
                return;
            }

            byte[] packet = _sysexAcc.ToArray();
            _sysexAcc.Clear();
            _incoming.Add(packet);
            RaiseLog("RX " + FormatHex(packet));
        }
    }

    private static string FormatHex(byte[] packet)
    {
        if (packet.Length <= 48)
        {
            return BitConverter.ToString(packet).Replace("-", " ");
        }

        return BitConverter.ToString(packet.AsSpan(0, 24).ToArray()).Replace("-", " ")
               + $" ... ({packet.Length} bytes) ... "
               + BitConverter.ToString(packet.AsSpan(packet.Length - 8, 8).ToArray()).Replace("-", " ");
    }

    private async Task<MvaveMessage?> WaitForAsync(Func<MvaveMessage, bool> match, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var deadline = DateTime.UtcNow + timeout;
        while (DateTime.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MvaveMessage? found = null;
            lock (_gate)
            {
                for (int i = 0; i < _incoming.Count; i++)
                {
                    var parsed = MvaveMessage.ParseSysex(_incoming[i]);
                    if (parsed is not null && match(parsed))
                    {
                        found = parsed;
                        _incoming.RemoveAt(i);
                        break;
                    }
                }
            }

            if (found is not null)
            {
                return found;
            }

            await Task.Delay(30, cancellationToken);
        }

        return null;
    }

    private void Drain()
    {
        lock (_gate)
        {
            _incoming.Clear();
            _sysexAcc.Clear();
        }
    }

    private void EnsureConnected()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("Chocolate não está conectado. Use USB-C na posição U.");
        }
    }

    private void RaiseLog(string line)
    {
        Log?.Invoke(this, line);
    }
}
