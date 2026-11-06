using System.Text;
using System.Text.Json;

namespace ChocolateEditor.Matribox;

public static class MatriboxPrstExport
{
    public static void Write(string path, MatriboxPatchDraft draft)
    {
        if (!TryLoadTemplate(out byte[] raw))
        {
            string custom = MatriboxPrstImport.ConfiguredFolder();
            string where = custom.Length > 0
                ? custom
                : "uma das pastas padrão do Sonicake";
            throw new InvalidOperationException(
                "Não encontrei um .prst de modelo em:\n" + where
                + "\n\nEm Configurar, aba Patches, aponta a pasta Patchs do editor Sonicake.");
        }

        PatchRaw(raw, draft);
        if (!MatriboxLzo.TryCompress(raw, out byte[] compressed) || compressed.Length < 8)
        {
            throw new InvalidOperationException(
                "Não consegui comprimir o .prst (HTUSBTools.dll do editor Sonicake).");
        }

        int[] nums = [.. compressed.Select(b => (int)b)];
        byte[] json = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(nums));
        File.WriteAllText(path, Convert.ToBase64String(json));
    }

    private static bool TryLoadTemplate(out byte[] raw)
    {
        raw = [];
        foreach (string folder in MatriboxPrstImport.TemplateFolders().Where(Directory.Exists))
        {
            foreach (string file in Directory.EnumerateFiles(folder, "*.prst"))
            {
                if (TryLoadRaw(file, out raw))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool TryLoadRaw(string path, out byte[] raw)
    {
        raw = [];
        try
        {
            string text = File.ReadAllText(path).Trim();
            byte[] jsonBytes = text.StartsWith('[')
                ? Encoding.UTF8.GetBytes(text)
                : Convert.FromBase64String(text);
            int[]? nums = JsonSerializer.Deserialize<int[]>(jsonBytes);
            if (nums is null || nums.Length < 8)
            {
                return false;
            }

            byte[] compressed = [.. nums.Select(n => (byte)n)];
            return MatriboxLzo.TryDecompress(compressed, out raw);
        }
        catch
        {
            return false;
        }
    }

    private static void PatchRaw(byte[] raw, MatriboxPatchDraft draft)
    {
        byte[] name = AsciiField(draft.Name, HotoneSysex.NameSize);
        byte[] style = AsciiField(draft.Style, HotoneSysex.TypeSize);
        byte[] seq = new byte[12];
        byte[] mods = new byte[12];
        byte[] algs = new byte[48];
        byte[] states = new byte[12];
        byte[] paramBlock = new byte[720];
        Array.Fill(seq, (byte)255);
        Array.Fill(mods, (byte)255);
        int nextId = 0;
        for (int pos = 0; pos < 12; pos++)
        {
            var slot = pos < draft.Chain.Count ? draft.Chain[pos] : new MatriboxChainSlot { Position = pos, ModuleType = -1 };
            if (slot.Empty || nextId > 11)
            {
                continue;
            }

            int id = nextId++;
            seq[pos] = (byte)id;
            mods[id] = (byte)slot.ModuleType;
            byte[] fx = BitConverter.GetBytes(slot.Algorithm);
            Buffer.BlockCopy(fx, 0, algs, id * 4, 4);
            states[id] = (byte)(slot.On ? 1 : 0);
            foreach (var p in slot.Parameters)
            {
                int off = id * 60 + p.Id * 4;
                if (off < 0 || off + 4 > paramBlock.Length)
                {
                    continue;
                }

                byte[] fl = BitConverter.GetBytes((float)p.Value);
                Buffer.BlockCopy(fl, 0, paramBlock, off, 4);
            }
        }

        int offset = 0;
        while (offset + 12 <= raw.Length)
        {
            int size = BitConverter.ToInt32(raw, offset + 4);
            int type = (BitConverter.ToInt32(raw, offset + 8) >> 16) & 0xFF;
            int payloadLen = size - 4;
            if (payloadLen < 0 || offset + 12 + payloadLen > raw.Length)
            {
                break;
            }

            int payloadAt = offset + 12;
            switch (type)
            {
                case 1 when payloadLen >= 4 + HotoneSysex.NameSize:
                    Buffer.BlockCopy(name, 0, raw, payloadAt + 4, name.Length);
                    if (payloadLen >= 4 + 17 + 20 + 50 + 10)
                    {
                        Buffer.BlockCopy(style, 0, raw, payloadAt + 4 + 17 + 20 + 50, style.Length);
                    }

                    break;
                case 4 when payloadLen >= 72:
                    Buffer.BlockCopy(seq, 0, raw, payloadAt, 12);
                    Buffer.BlockCopy(mods, 0, raw, payloadAt + 12, 12);
                    Buffer.BlockCopy(algs, 0, raw, payloadAt + 24, 48);
                    break;
                case 5 when payloadLen >= 732:
                    Buffer.BlockCopy(paramBlock, 0, raw, payloadAt, Math.Min(720, payloadLen));
                    if (payloadLen >= 732)
                    {
                        Buffer.BlockCopy(states, 0, raw, payloadAt + 720, 12);
                    }

                    break;
            }

            offset += 12 + payloadLen;
        }
    }

    private static byte[] AsciiField(string text, int length)
    {
        var buf = new byte[length];
        byte[] src = Encoding.ASCII.GetBytes(text ?? "");
        Buffer.BlockCopy(src, 0, buf, 0, Math.Min(src.Length, length - 1));
        return buf;
    }
}
