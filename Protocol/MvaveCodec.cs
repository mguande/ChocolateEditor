using System.Buffers.Binary;

namespace ChocolateEditor.Protocol;

/// <summary>
/// Codec SysEx M-VAVE/CUVAVE: empacota a mensagem como inteiro little-endian em base-128 (7 bits).
/// </summary>
public static class MvaveCodec
{
    public const byte SysexStart = 0xF0;
    public const byte SysexEnd = 0xF7;
    public static readonly byte[] Header = [0x00, 0x59];

    public static byte[] Encode(byte[] source)
    {
        var dest = new List<byte>(source.Length + 4);
        uint bitNum = 0;
        uint accum = 0;
        foreach (byte b in source)
        {
            accum |= (uint)b << (int)(bitNum & 0x1F);
            bitNum += 1;
            while (true)
            {
                dest.Add((byte)(accum & 0x7F));
                accum >>= 7;
                if (bitNum < 7)
                {
                    break;
                }

                bitNum -= 7;
            }
        }

        dest.Add((byte)(accum & 0x7F));
        return [.. dest];
    }

    public static byte[] Decode(byte[] source)
    {
        var dest = new List<byte>(source.Length);
        int bitNum = 0;
        byte lastVal = 0;
        foreach (byte b in source)
        {
            uint accum = bitNum == 0
                ? b
                : (uint)(b & ((1 << bitNum) - 1));

            if (bitNum > 0)
            {
                dest.Add(unchecked((byte)((lastVal & 0x7F) | (int)(accum << (8 - bitNum)))));
            }

            lastVal = (byte)(b >> bitNum);
            bitNum = (bitNum + 1) % 8;
        }

        if (lastVal > 0)
        {
            dest.Add(lastVal);
        }

        return [.. dest];
    }

    public static byte Checksum(ReadOnlySpan<byte> data)
    {
        byte sum = 0;
        foreach (byte b in data)
        {
            sum = unchecked((byte)(sum + b));
        }

        return (byte)~sum;
    }

    public static byte[] WrapSysex(byte[] clearMessage)
    {
        byte[] encoded = Encode(clearMessage);
        var packet = new byte[encoded.Length + 2];
        packet[0] = SysexStart;
        Buffer.BlockCopy(encoded, 0, packet, 1, encoded.Length);
        packet[^1] = SysexEnd;
        return packet;
    }

    public static byte[] UnwrapSysex(byte[] sysex)
    {
        if (sysex.Length < 3 || sysex[0] != SysexStart || sysex[^1] != SysexEnd)
        {
            throw new InvalidDataException("SysEx inválido (faltam F0/F7).");
        }

        var inner = new byte[sysex.Length - 2];
        Buffer.BlockCopy(sysex, 1, inner, 0, inner.Length);
        return Decode(inner);
    }

    public static byte[] BuildClearMessage(byte command, byte[] payload)
    {
        var data = new byte[2 + 1 + 3 + payload.Length + 1];
        data[0] = Header[0];
        data[1] = Header[1];
        data[2] = command;
        WriteU24(data.AsSpan(3, 3), (uint)payload.Length);
        Buffer.BlockCopy(payload, 0, data, 6, payload.Length);
        data[^1] = Checksum(payload);
        return data;
    }

    public static bool TryParseClear(byte[] clear, out byte command, out byte[] payload)
    {
        command = 0;
        payload = [];
        if (clear.Length < 7 || clear[0] != Header[0] || clear[1] != Header[1])
        {
            return false;
        }

        command = clear[2];
        uint declared = ReadU24(clear.AsSpan(3, 3));
        int payloadLen = (int)declared;
        // Não aceitar pacote truncado: o WinMM corta SysEx longo e o dump
        // ficava com ~1088 bytes, desalinhado (só o B Bank do grupo 1).
        if (payloadLen < 0 || 6 + payloadLen + 1 > clear.Length)
        {
            return false;
        }

        payload = new byte[payloadLen];
        Buffer.BlockCopy(clear, 6, payload, 0, payloadLen);
        return true;
    }

    public static void WriteU24(Span<byte> dest, uint value)
    {
        dest[0] = (byte)(value & 0xFF);
        dest[1] = (byte)((value >> 8) & 0xFF);
        dest[2] = (byte)((value >> 16) & 0xFF);
    }

    public static uint ReadU24(ReadOnlySpan<byte> src)
    {
        return src[0] | ((uint)src[1] << 8) | ((uint)src[2] << 16);
    }

    public static void WriteU32Le(Span<byte> dest, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(dest, value);
    }

    public static uint ReadU32Le(ReadOnlySpan<byte> src)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(src);
    }
}
