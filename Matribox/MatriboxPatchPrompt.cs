using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ChocolateEditor.Matribox;

public sealed class MatriboxPatchDraft
{
    public string Name { get; set; } = "AI Patch";
    public string Style { get; set; } = "";
    public string Note { get; set; } = "";
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public List<MatriboxChainSlot> Chain { get; set; } = HotoneSysex.EmptyChain();

    public string FileStem
    {
        get
        {
            string n = string.Concat(Name.Where(c => !Path.GetInvalidFileNameChars().Contains(c))).Trim();
            return string.IsNullOrWhiteSpace(n) ? "AI Patch" : n;
        }
    }
}

public static class MatriboxPatchPrompt
{
    private static readonly Regex JsonFence = new(
        @"```(?:json)?\s*(\{[\s\S]*?\})\s*```",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public static string SystemInstruction(bool includeManual)
        => includeManual
            ? """
              És um técnico da Sonicake Matribox II Pro (firmware 1.2.2). Usa o manual e a lista de knobs abaixo.
              Cadeia: até 12 módulos, ordem típica DYN → (FREQ/WAH) → DRV → AMP → CAB → EQ → MOD → DLY → RVB.
              Tipos: DYN, FREQ, WAH, DRV, AMP, CAB, IR, EQ, MOD, DLY, RVB, Clone, FXLOOP, FXSND, FXRTN, VOL.
              Ajusta o patch COMPLETO: escolhe algoritmos reais da lista e preenche TODOS os knobs desse algoritmo (nomes exactos, valores dentro do min-max).
              Som Nirvana/grunge: AMP saturado, DRV com gain alto, CAB 4x12, DLY/RVB curtos, DYN suave.
              Responde em português com uma frase curta e DEPOIS um único bloco JSON:
              {
                "name": "máx 17 caracteres",
                "style": "Rock",
                "note": "porque estes efeitos e knobs",
                "slots": [
                  {"slot": 1, "type": "DRV", "algorithm": "Skreamer", "on": true, "params": {"Gain": 72, "Tone": 45, "Volume": 55}}
                ]
              }
              Omite slots vazios. algorithm tem de ser um nome exacto da lista. params tem de incluir todos os knobs listados para esse algoritmo.
              """
            : """
              És um técnico da Sonicake Matribox II Pro (firmware 1.2.2).
              Cadeia: até 12 módulos, ordem típica DYN → (FREQ/WAH) → DRV → AMP → CAB → EQ → MOD → DLY → RVB.
              Tipos: DYN, FREQ, WAH, DRV, AMP, CAB, IR, EQ, MOD, DLY, RVB, Clone, FXLOOP, FXSND, FXRTN, VOL.
              Ajusta o patch COMPLETO: escolhe algoritmos reais da lista e knobs típicos 0–100 (Gain, Tone, Volume, Mix, Time, Feedback…).
              Som Nirvana/grunge: AMP saturado, DRV com gain alto, CAB 4x12, DLY/RVB curtos, DYN suave.
              Responde em português com uma frase curta e DEPOIS um único bloco JSON:
              {
                "name": "máx 17 caracteres",
                "style": "Rock",
                "note": "porque estes efeitos e knobs",
                "slots": [
                  {"slot": 1, "type": "DRV", "algorithm": "Skreamer", "on": true, "params": {"Gain": 72, "Tone": 45, "Volume": 55}}
                ]
              }
              Omite slots vazios. algorithm tem de ser um nome exacto da lista.
              """;

    public static string WrapUserPrompt(string question, bool includeManual)
    {
        var sb = new StringBuilder();
        sb.AppendLine(question.Trim());
        sb.AppendLine();
        sb.AppendLine(includeManual
            ? "Usa o manual e a lista de knobs. Devolve o JSON do patch já todo ajustado."
            : "Devolve o JSON do patch já todo ajustado. Knobs típicos 0–100 (Gain, Tone, Volume, Mix, Time, Feedback…).");
        sb.AppendLine();
        sb.AppendLine("Tabela da cadeia:");
        sb.AppendLine("| Slot | Tipo | Algoritmo | On/Off | Parâmetros |");
        for (int i = 1; i <= 12; i++)
        {
            sb.AppendLine($"| {i} |  |  |  |  |");
        }

        sb.AppendLine();
        sb.AppendLine("=== Algoritmos ===");
        sb.AppendLine(MatriboxAlgCatalog.PromptCatalog());
        if (includeManual)
        {
            sb.AppendLine();
            sb.AppendLine("=== Manual Matribox II Pro (firmware 1.2.2) ===");
            sb.AppendLine(HardwareManual());
            sb.AppendLine();
            sb.AppendLine("=== Knobs por algoritmo (preenche todos) ===");
            sb.AppendLine(MatriboxAlgCatalog.PromptWidgets());
        }

        return sb.ToString();
    }

    public static string HardwareManual()
        => """
           Sonicake Matribox II Pro: processador multi-efeitos com 60 bancos × 4 presets (A/B/C/D) = 240 patches.
           Cada patch tem uma cadeia de até 12 módulos em série. A ordem dos slots é o sinal (entrada → saída).
           Módulos: DYN (compressores), FREQ (filtros), WAH, DRV (overdrive/distorção), AMP (amplificadores),
           CAB (colunas), IR (impulse response), EQ, MOD (chorus/flanger/phaser/tremolo), DLY (delay),
           RVB (reverb), Clone, FXLOOP / FXSND / FXRTN, VOL.
           MIDI USB (firmware 1.2.2): CC0 0 ou 1 + Program Change escolhe o preset; CC 43–54 ligam/desligam
           os 12 slots; CC 29 Stomp Mode. Não enviar dezenas de CCs iguais ao estado do patch (system overload).
           Knobs do editor: valores 0–100 (por vezes sync on/off). Gain/Drive alto = mais distorção;
           Tone/Treble alto = mais agudos; Mix no delay/reverb = quantidade de efeito; Feedback = número de repeats;
           Time/Decay = duração. AMP+CAB juntos soam a guitarra real; IR substitui CAB. Para rock/grunge:
           DRV + AMP saturado + CAB 4x12, delay curto, reverb pequeno, compressão leve.
           Manual online: https://sonicake.oss-accelerate.aliyuncs.com/res/JIyz39IVZe/Matribox_II_PRO_Online_Manual_EN_Firmware_V1.2.2.pdf
           """;

    public static bool TryParse(string reply, out MatriboxPatchDraft draft)
    {
        draft = new MatriboxPatchDraft();
        if (string.IsNullOrWhiteSpace(reply))
        {
            return false;
        }

        string? json = null;
        var fence = JsonFence.Match(reply);
        if (fence.Success)
        {
            json = fence.Groups[1].Value;
        }
        else
        {
            int start = reply.IndexOf('{');
            int end = reply.LastIndexOf('}');
            if (start >= 0 && end > start)
            {
                json = reply[start..(end + 1)];
            }
        }

        if (json is null)
        {
            return false;
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            string name = root.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
            if (name.Length > HotoneSysex.NameSize)
            {
                name = name[..HotoneSysex.NameSize].Trim();
            }

            draft.Name = string.IsNullOrWhiteSpace(name) ? "AI Patch" : name.Trim();
            draft.Style = root.TryGetProperty("style", out var st) ? st.GetString() ?? "" : "";
            draft.Note = root.TryGetProperty("note", out var note) ? note.GetString() ?? "" : "";
            var chain = HotoneSysex.EmptyChain();
            if (root.TryGetProperty("slots", out var slots) && slots.ValueKind == JsonValueKind.Array)
            {
                foreach (var row in slots.EnumerateArray())
                {
                    int slot = 0;
                    if (row.TryGetProperty("slot", out var s))
                    {
                        slot = s.ValueKind == JsonValueKind.Number ? s.GetInt32() : 0;
                    }

                    int pos = slot is >= 1 and <= 12 ? slot - 1 : -1;
                    if (pos < 0)
                    {
                        continue;
                    }

                    string type = row.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";
                    string alg = row.TryGetProperty("algorithm", out var a) ? a.GetString() ?? "" : "";
                    if (string.IsNullOrWhiteSpace(alg) && row.TryGetProperty("alg", out var a2))
                    {
                        alg = a2.GetString() ?? "";
                    }

                    bool on = true;
                    if (row.TryGetProperty("on", out var o))
                    {
                        on = o.ValueKind == JsonValueKind.True
                             || (o.ValueKind == JsonValueKind.String
                                 && (o.GetString() ?? "").StartsWith("on", StringComparison.OrdinalIgnoreCase));
                    }

                    if (!MatriboxAlgCatalog.TryResolve(type, alg, out int module, out int fxid, out string resolved))
                    {
                        continue;
                    }

                    chain[pos] = new MatriboxChainSlot
                    {
                        Position = pos,
                        ModuleType = module,
                        Algorithm = fxid,
                        On = on,
                        Parameters = MatriboxAlgCatalog.BindParams(fxid, ReadParams(row))
                    };
                    _ = resolved;
                }
            }

            if (chain.All(c => c.Empty))
            {
                return false;
            }

            draft.Chain = chain;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Dictionary<string, int>? ReadParams(JsonElement row)
    {
        if (!row.TryGetProperty("params", out var p) && !row.TryGetProperty("parameters", out p))
        {
            return null;
        }

        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        if (p.ValueKind == JsonValueKind.Object)
        {
            foreach (var prop in p.EnumerateObject())
            {
                if (TryInt(prop.Value, out int v))
                {
                    map[prop.Name] = v;
                }
            }
        }
        else if (p.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in p.EnumerateArray())
            {
                string name = item.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                if (item.TryGetProperty("value", out var val) && TryInt(val, out int v))
                {
                    map[name] = v;
                }
            }
        }

        return map.Count == 0 ? null : map;
    }

    private static bool TryInt(JsonElement el, out int value)
    {
        if (el.ValueKind == JsonValueKind.Number && el.TryGetInt32(out value))
        {
            return true;
        }

        return int.TryParse(el.GetString(), out value);
    }

    public static string ReplyWithoutJson(string reply)
    {
        string text = JsonFence.Replace(reply, "").Trim();
        int start = text.IndexOf('{');
        int end = text.LastIndexOf('}');
        if (start >= 0 && end > start && text[start..].Contains("\"slots\"", StringComparison.OrdinalIgnoreCase))
        {
            text = (text[..start] + text[(end + 1)..]).Trim();
        }

        return text;
    }
}
