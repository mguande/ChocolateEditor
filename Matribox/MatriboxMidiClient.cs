using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using ChocolateEditor.Models;
using ChocolateEditor.Protocol;

namespace ChocolateEditor.Matribox;

public sealed class MatriboxMidiClient : IDisposable
{
    private static readonly string[] NameHints =
    [
        "matribox", "sonicake", "sonica", "matrixbox"
    ];

    private OutputDevice? _output;
    private InputDevice? _input;
    private readonly object _gate = new();
    private readonly List<byte> _sysexAcc = [];
    private TaskCompletionSource<bool>? _presetWaiter;
    private int _waitingPresetIndex = -1;
    private List<MatriboxPresetInfo> _presets = [];
    private bool _scanning;
    private int _incomingBankMsb;
    private int _lastBankMsb = -1;

    public int Channel { get; set; } = 1;
    public bool IsConnected => _output is not null;
    public string? ConnectedName { get; private set; }
    public event EventHandler<string>? Log;
    public event EventHandler? SnapshotChanged;
    public event EventHandler<(int Current, int Total, string Message)>? Progress;
    public IReadOnlyList<MatriboxPresetInfo> Presets => _presets;
    public IReadOnlyList<MatriboxChainSlot> CurrentChain { get; private set; } = [];
    public int CurrentPresetIndex { get; private set; } = -1;

    public static IReadOnlyList<MidiPortInfo> ListPorts()
        => ChocolateUsbClient.ListPorts();

    public static bool LooksLikeMatribox(string name)
        => NameHints.Any(h => name.Contains(h, StringComparison.OrdinalIgnoreCase));

    public void Connect(string deviceName)
    {
        Disconnect();
        _output = OpenOutput(deviceName) ?? throw new InvalidOperationException($"Saída MIDI '{deviceName}' não encontrada.");
        _input = OpenInput(deviceName);
        if (_input is not null)
        {
            _input.EventReceived += OnEventReceived;
            _input.StartEventsListening();
        }

        ConnectedName = _output.Name;
        CurrentChain = HotoneSysex.EmptyChain();
        RaiseLog(_input is null
            ? "Matribox MIDI OUT: " + ConnectedName + " (sem MIDI IN — a lista de nomes não chega)"
            : "Matribox MIDI IN/OUT: " + ConnectedName);
    }

    public void Disconnect()
    {
        if (_input is not null)
        {
            try
            {
                _input.EventReceived -= OnEventReceived;
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
        CurrentChain = HotoneSysex.EmptyChain();
        CurrentPresetIndex = -1;
        _lastBankMsb = -1;
    }

    public void Send(IEnumerable<MidiCommand> commands)
    {
        if (_output is null)
        {
            throw new InvalidOperationException("Matribox não está conectada (MIDI OUT).");
        }

        foreach (var cmd in commands)
        {
            var ch = (FourBitNumber)Math.Clamp(cmd.Channel - 1, 0, 15);
            MidiEvent ev = cmd.Type switch
            {
                MidiCommandType.ControlChange => new ControlChangeEvent(
                    (SevenBitNumber)cmd.Data1, (SevenBitNumber)cmd.Data2)
                {
                    Channel = ch
                },
                MidiCommandType.ProgramChange => new ProgramChangeEvent((SevenBitNumber)cmd.Data1)
                {
                    Channel = ch
                },
                _ => throw new NotSupportedException(cmd.Type.ToString())
            };
            _output.SendEvent(ev);
            RaiseLog("TX Matribox " + cmd.Summary);
        }
    }

    /// <summary>
    /// Percorre os 240 patches (muda o som). O SysEx !%MP só confirma a mudança — não traz nomes.
    /// </summary>
    public async Task<bool> DumpPresetListAsync(CancellationToken cancellationToken = default)
    {
        if (_output is null)
        {
            return false;
        }

        EnsurePresetSlots();
        _scanning = true;
        _lastBankMsb = -1;
        int got = 0;
        try
        {
            RaiseLog("A percorrer os 240 patches com pausas (a pedaleira satura se receber CC+PC muito rápido).");
            for (int i = 0; i < HotoneSysex.PresetCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string label = MatriboxMidi.PresetLabel(i / 4 + 1, i % 4);
                ReportProgress(i + 1, HotoneSysex.PresetCount, label);
                RaiseLog($"[{i + 1}/{HotoneSysex.PresetCount}] {label}");
                if (await RequestPresetAsync(i, cancellationToken))
                {
                    got++;
                    RaiseLog($"  ACK {label}");
                }
                else
                {
                    RaiseLog($"  sem ACK em {label} — continuo sem insistir");
                }

                await Task.Delay(350, cancellationToken);
                if (i == 119)
                {
                    RaiseLog("Meio da lista (banco 31 → 32): pausa extra.");
                    await Task.Delay(1500, cancellationToken);
                }
                else if (i % 16 == 15)
                {
                    RaiseLog("Pausa para a pedaleira não saturar…");
                    await Task.Delay(1200, cancellationToken);
                }
            }
        }
        finally
        {
            _scanning = false;
        }

        RaiseLog($"Terminado: {got} / {HotoneSysex.PresetCount} ACKs. Esta porta MIDI não manda nomes nem a cadeia.");
        return got > 0;
    }

    public void ReplacePresets(IReadOnlyList<MatriboxPresetInfo> presets)
    {
        EnsurePresetSlots();
        for (int i = 0; i < _presets.Count && i < presets.Count; i++)
        {
            _presets[i] = MatriboxPresetStore.MergeSlot(_presets[i], presets[i]);
        }
    }

    public void RenamePreset(int index, string name)
    {
        EnsurePresetSlots();
        if (index < 0 || index >= _presets.Count)
        {
            return;
        }

        var prev = _presets[index];
        _presets[index] = new MatriboxPresetInfo
        {
            Index = index,
            Name = name.Trim(),
            Style = prev.Style,
            Chain = prev.Chain
        };
    }

    public async Task<bool> RequestPresetAsync(int index0, CancellationToken cancellationToken = default)
    {
        if (_output is null || index0 < 0)
        {
            return false;
        }

        var waiter = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        _waitingPresetIndex = index0;
        _presetWaiter = waiter;
        int bank = index0 / 4 + 1;
        int slot = index0 % 4;
        var (msb, _) = MatriboxMidi.Address(bank, slot);
        if (_lastBankMsb != msb)
        {
            Send(MatriboxMidi.SelectPreset(bank, slot, Channel, lastBankMsb: null));
            _lastBankMsb = msb;
            if (_scanning)
            {
                await Task.Delay(250, cancellationToken);
            }
        }
        else
        {
            Send(MatriboxMidi.SelectPreset(bank, slot, Channel, lastBankMsb: msb));
        }

        using var timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(_scanning ? 800 : 1200));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        try
        {
            await using (linked.Token.Register(() => waiter.TrySetResult(false)))
            {
                return await waiter.Task;
            }
        }
        finally
        {
            if (ReferenceEquals(_presetWaiter, waiter))
            {
                _presetWaiter = null;
            }

            _waitingPresetIndex = -1;
        }
    }

    public void Dispose() => Disconnect();

    private void SendEditor(int address, byte[] payload)
    {
        if (_output is null)
        {
            return;
        }

        byte[] inner = HotoneSysex.Build(address, HotoneSysex.FlagRequest, payload);
        byte[] dryWet = new byte[inner.Length + 1];
        Buffer.BlockCopy(inner, 0, dryWet, 0, inner.Length);
        dryWet[^1] = 0xF7;
        _output.SendEvent(new NormalSysExEvent(dryWet));
        RaiseLog($"TX editor 0x{address:X8}  {payload.Length} bytes");
    }

    private void OnEventReceived(object? sender, MidiEventReceivedEventArgs e)
    {
        if (e.Event is ControlChangeEvent cc && cc.ControlNumber == 0)
        {
            _incomingBankMsb = cc.ControlValue == 1 ? 1 : 0;
            return;
        }

        if (e.Event is ProgramChangeEvent pc)
        {
            int program = pc.ProgramNumber;
            if (program is >= 1 and <= 120)
            {
                CurrentPresetIndex = _incomingBankMsb * 120 + (program - 1);
            }

            return;
        }

        if (e.Event is not SysExEvent sysEx)
        {
            return;
        }

        byte[] data = sysEx.Data ?? [];
        byte[] full;
        lock (_gate)
        {
            if (e.Event is EscapeSysExEvent)
            {
                _sysexAcc.AddRange(data);
                return;
            }

            if (_sysexAcc.Count > 0)
            {
                _sysexAcc.AddRange(data);
                full = [.. _sysexAcc];
                _sysexAcc.Clear();
            }
            else
            {
                full = data;
            }
        }

        if (!HotoneSysex.TryParse(full, out _, out int address, out byte[] payload)
            && !HotoneSysex.TryParse([0xF0, .. full], out _, out address, out payload))
        {
            HandleLiveSysex(full);
            return;
        }

        RaiseLog($"RX editor 0x{address:X8}  {payload.Length} bytes");
        HandleEditor(address, payload);
    }

    private void HandleLiveSysex(byte[] full)
    {
        SaveLastSysex(full);
        byte[] inner = HotoneSysex.Strip(full);

        if (HotoneSysex.TryParseMpNotify(full, out int index1))
        {
            RaiseLog("RX !%MP  " + (index1 > 0
                ? MatriboxMidi.PresetLabel((index1 - 1) / 4 + 1, (index1 - 1) % 4)
                : HotoneSysex.Hex(inner)));

            if (index1 > 0)
            {
                CurrentPresetIndex = index1 - 1;
            }

            _presetWaiter?.TrySetResult(true);
            return;
        }

        RaiseLog("RX SysEx " + full.Length + "  " + HotoneSysex.Hex(inner));

        if (!HotoneSysex.TryParseLiveDump(full, out string name, out var chain))
        {
            string ascii = HotoneSysex.BestAscii(full);
            if (ascii.Length >= 2 && !HotoneSysex.IsHeaderNoise(ascii))
            {
                RaiseLog("  texto no SysEx: " + ascii);
                name = ascii;
            }
            else
            {
                return;
            }
        }

        if (HotoneSysex.IsHeaderNoise(name))
        {
            name = "";
        }

        int index = _waitingPresetIndex;
        if (index < 0)
        {
            index = CurrentPresetIndex >= 0 ? CurrentPresetIndex : 0;
        }

        ApplySnapshot(index, name, chain);
        _presetWaiter?.TrySetResult(true);
        if (!_scanning)
        {
            SnapshotChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ApplySnapshot(int index, string name, List<MatriboxChainSlot> chain)
    {
        EnsurePresetSlots();
        if (index >= 0 && index < _presets.Count)
        {
            _presets[index] = MatriboxPresetStore.MergeSlot(_presets[index], new MatriboxPresetInfo
            {
                Index = index,
                Name = name,
                Style = _presets[index].Style,
                Chain = chain
            });
        }

        CurrentPresetIndex = index;
        if (chain.Count > 0)
        {
            CurrentChain = chain;
        }

        string shown = _presets.ElementAtOrDefault(index)?.Display ?? index.ToString();
        if (chain.Count > 0)
        {
            RaiseLog($"{shown}: " + string.Join(" → ", chain.Select(s => HotoneSysex.ModuleName(s.ModuleType) + (s.On ? "" : " off"))));
        }
        else if (!string.IsNullOrWhiteSpace(name))
        {
            RaiseLog(shown);
        }
    }

    private void EnsurePresetSlots()
    {
        if (_presets.Count == HotoneSysex.PresetCount)
        {
            return;
        }

        _presets = Enumerable.Range(0, HotoneSysex.PresetCount)
            .Select(i => new MatriboxPresetInfo { Index = i })
            .ToList();
    }

    private static void SaveLastSysex(byte[] data)
    {
        try
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ChocolateEditor");
            Directory.CreateDirectory(dir);
            File.WriteAllBytes(Path.Combine(dir, "last-matribox-sysex.bin"), data);
        }
        catch
        {
            // ignore
        }
    }

    private void HandleEditor(int address, byte[] payload)
    {
        int innerAddr = address;
        byte[] body = payload;
        if (address == HotoneSysex.AddrPresetSignal && payload.Length >= 4)
        {
            innerAddr = HotoneSysex.ReadLe32(payload);
            body = payload[4..];
        }

        if (innerAddr == HotoneSysex.AddrPresetList)
        {
            if (TryParsePresetList(body, out var list))
            {
                ReplacePresets(list);
                RaiseLog($"Lista da Matribox: {list.Count(MatriboxPresetStore.HasUsefulData)} com nome/cadeia.");
            }

            return;
        }

        if (address == HotoneSysex.AddrPreset || innerAddr == HotoneSysex.AddrPreset)
        {
            if (body.Length >= 4)
            {
                int index = HotoneSysex.ReadLe32(body);
                byte[] rest = body[4..];
                if (TryParseChain(rest, out var chain) || TryParseChain(body, out chain))
                {
                    CurrentPresetIndex = index >= 0 ? index : _waitingPresetIndex;
                    CurrentChain = chain;
                    RaiseLog("Cadeia: " + string.Join(" → ", chain.Select(s => s.Label + (s.On ? " On" : " Off"))));
                    _presetWaiter?.TrySetResult(true);
                }
            }
        }
    }

    private static bool TryParsePresetList(byte[] data, out List<MatriboxPresetInfo> list)
    {
        list = [];
        int n = HotoneSysex.PresetCount;
        int need = n * (4 + HotoneSysex.NameSize + HotoneSysex.TypeSize);
        if (data.Length < need)
        {
            return false;
        }

        for (int i = 0; i < n; i++)
        {
            int idx = HotoneSysex.ReadLe32(data, i * 4);
            var name = data.AsSpan(n * 4 + i * HotoneSysex.NameSize, HotoneSysex.NameSize);
            var type = data.AsSpan(n * 4 + n * HotoneSysex.NameSize + i * HotoneSysex.TypeSize, HotoneSysex.TypeSize);
            list.Add(new MatriboxPresetInfo
            {
                Index = idx >= 0 && idx < n ? idx : i,
                Name = HotoneSysex.AsciiZ(name),
                Style = HotoneSysex.AsciiZ(type)
            });
        }

        return list.Count > 0;
    }

    /// <summary>
    /// readEffectData: 12 sequence + 12 module + 12×4 alg + 12 state.
    /// </summary>
    private static bool TryParseChain(byte[] data, out List<MatriboxChainSlot> chain)
    {
        chain = [];
        const int slots = HotoneSysex.SlotCount;
        int need = slots + slots + slots * 4 + slots;
        if (data.Length < need)
        {
            return false;
        }

        int o = 0;
        byte[] sequence = data[o..(o + slots)];
        o += slots;
        byte[] modules = data[o..(o + slots)];
        o += slots;
        int[] algs = new int[slots];
        for (int i = 0; i < slots; i++)
        {
            algs[i] = HotoneSysex.ReadLe32(data, o + i * 4);
        }

        o += slots * 4;
        byte[] states = data[o..(o + slots)];

        for (int pos = 0; pos < slots; pos++)
        {
            int slotId = sequence[pos];
            if (slotId < 0 || slotId >= slots)
            {
                slotId = pos;
            }

            chain.Add(new MatriboxChainSlot
            {
                Position = pos,
                ModuleType = modules[slotId],
                Algorithm = algs[slotId],
                On = states[slotId] != 0
            });
        }

        return true;
    }

    private static OutputDevice? OpenOutput(string name)
    {
        try
        {
            return OutputDevice.GetByName(name);
        }
        catch
        {
            // try similar
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
            // try similar
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
        => a.Equals(b, StringComparison.OrdinalIgnoreCase)
           || a.Contains(b, StringComparison.OrdinalIgnoreCase)
           || b.Contains(a, StringComparison.OrdinalIgnoreCase);

    private void ReportProgress(int current, int total, string message)
        => Progress?.Invoke(this, (current, total, message));

    private void RaiseLog(string line) => Log?.Invoke(this, line);
}
