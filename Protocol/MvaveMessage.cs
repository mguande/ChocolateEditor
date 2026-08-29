namespace ChocolateEditor.Protocol;

public abstract record MvaveMessage
{
    public const byte CmdAck = 0x00;
    public const byte CmdNameVersion = 0x11;
    public const byte CmdErase = 0x21;
    public const byte CmdWrite = 0x22;
    public const byte CmdRead = 0x23;

    public abstract byte[] ToSysex();

    public static MvaveMessage Discover() => new NameVersionRequest();

    public static MvaveMessage WriteMemory(byte memType, uint address, byte[] data)
    {
        return new WriteMemoryMessage(memType, address, data);
    }

    public static MvaveMessage ReadMemory(byte memType, uint address, uint length)
    {
        return new ReadMemoryRequest(memType, address, length);
    }

    public static MvaveMessage? ParseSysex(byte[] sysex)
    {
        byte[] clear;
        try
        {
            clear = MvaveCodec.UnwrapSysex(NormalizeSysex(sysex));
        }
        catch (InvalidDataException)
        {
            return null;
        }

        if (!MvaveCodec.TryParseClear(clear, out byte command, out byte[] payload))
        {
            return null;
        }

        return command switch
        {
            CmdAck => new AckMessage(payload.Length > 0 && payload[0] != 0),
            CmdNameVersion when payload.Length == 0 => new NameVersionRequest(),
            CmdNameVersion => ParseNameVersion(payload),
            CmdWrite when payload.Length >= 8 => ParseWrite(payload),
            CmdRead when payload.Length >= 8 && payload.Length == 8 => ParseReadRequest(payload),
            CmdRead when payload.Length >= 8 => ParseReadContent(payload),
            _ => new UnknownMessage(command, payload)
        };
    }

    private static NameVersionResponse ParseNameVersion(byte[] payload)
    {
        int nameLen = Math.Min(16, payload.Length);
        string name = System.Text.Encoding.ASCII.GetString(payload, 0, nameLen).TrimEnd('\0', ' ');
        byte[] rest = payload.Length > nameLen ? payload[nameLen..] : [];
        return new NameVersionResponse(name, rest);
    }

    private static WriteMemoryMessage ParseWrite(byte[] payload)
    {
        SplitMem(payload, out byte mem, out uint addr, out uint _, out byte[] data);
        return new WriteMemoryMessage(mem, addr, data);
    }

    private static ReadMemoryRequest ParseReadRequest(byte[] payload)
    {
        SplitMem(payload, out byte mem, out uint addr, out uint len, out _);
        return new ReadMemoryRequest(mem, addr, len);
    }

    private static MemoryContentMessage ParseReadContent(byte[] payload)
    {
        SplitMem(payload, out byte mem, out uint addr, out uint len, out byte[] data);
        return new MemoryContentMessage(mem, addr, len, data);
    }

    private static void SplitMem(byte[] payload, out byte mem, out uint addr, out uint len, out byte[] data)
    {
        mem = payload[0];
        addr = MvaveCodec.ReadU32Le(payload.AsSpan(1, 4));
        len = MvaveCodec.ReadU24(payload.AsSpan(5, 3));
        data = payload.Length > 8 ? payload[8..] : [];
    }

    protected static byte[] MemoryPayload(byte memType, uint address, uint length, byte[]? data)
    {
        int extra = data?.Length ?? 0;
        var payload = new byte[8 + extra];
        payload[0] = memType;
        MvaveCodec.WriteU32Le(payload.AsSpan(1, 4), address);
        MvaveCodec.WriteU24(payload.AsSpan(5, 3), length);
        if (extra > 0 && data is not null)
        {
            Buffer.BlockCopy(data, 0, payload, 8, extra);
        }

        return payload;
    }

    /// <summary>
    /// DryWetMidi entrega o payload sem o F0. Não inventamos F7 — pacotes
    /// incompletos são juntados em ChocolateUsbClient.
    /// </summary>
    public static byte[] NormalizeSysex(byte[] data)
    {
        if (data.Length == 0)
        {
            return data;
        }

        if (data[0] == MvaveCodec.SysexStart)
        {
            return data;
        }

        var full = new byte[data.Length + 1];
        full[0] = MvaveCodec.SysexStart;
        Buffer.BlockCopy(data, 0, full, 1, data.Length);
        return full;
    }
}

public sealed record AckMessage(bool Ok) : MvaveMessage
{
    public override byte[] ToSysex()
        => MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdAck, [Ok ? (byte)1 : (byte)0]));
}

public sealed record NameVersionRequest : MvaveMessage
{
    public override byte[] ToSysex()
        => MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdNameVersion, []));
}

public sealed record NameVersionResponse(string Name, byte[] Extra) : MvaveMessage
{
    public override byte[] ToSysex()
        => MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdNameVersion, []));
}

public sealed record WriteMemoryMessage(byte MemType, uint Address, byte[] Data) : MvaveMessage
{
    public override byte[] ToSysex()
    {
        byte[] payload = MemoryPayload(MemType, Address, (uint)Data.Length, Data);
        return MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdWrite, payload));
    }
}

public sealed record ReadMemoryRequest(byte MemType, uint Address, uint Length) : MvaveMessage
{
    public override byte[] ToSysex()
    {
        byte[] payload = MemoryPayload(MemType, Address, Length, null);
        return MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdRead, payload));
    }
}

public sealed record MemoryContentMessage(byte MemType, uint Address, uint Length, byte[] Data) : MvaveMessage
{
    public override byte[] ToSysex()
    {
        byte[] payload = MemoryPayload(MemType, Address, Length, Data);
        return MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(CmdRead, payload));
    }
}

public sealed record UnknownMessage(byte Command, byte[] Payload) : MvaveMessage
{
    public override byte[] ToSysex()
        => MvaveCodec.WrapSysex(MvaveCodec.BuildClearMessage(Command, Payload));
}
