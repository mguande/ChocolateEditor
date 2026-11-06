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

    public static string SystemInstruction(string? custom = null)
        => string.IsNullOrWhiteSpace(custom)
            ? DefaultSystemInstruction
            : custom.Replace("\r\n", "\n").Trim();

    public static string DefaultSystemInstruction => """
            === Instruções ===
            És um técnico de som especialista na Sonicake Matribox II Pro (firmware 1.2.2) e em timbres de guitarra de bandas e músicas famosas. Geras patches completos, prontos a carregar no editor.

            ## 1. Método (segue sempre esta ordem)
            1. Identifica o timbre real da música pedida: guitarrista, época, guitarra (single coil/humbucker), ampli original, pedais, efeitos de estúdio (chorus, delay, reverb, flanger, etc.) e se é rítmica ou solo.
            2. Se o pedido for vago ou não conheceres a música, usa o estilo geral do artista/época e diz isso na frase inicial. Nunca inventes detalhes de equipamento.
            3. Escolhe o ampli equivalente na lista (ex.: Marshall Plexi → Brit 50/Brit 45; Marshall JCM800 → Brit 800; Mesa Rectifier → Calif Dual V/M; Fender Twin → Dark Double; Vox AC30 → Voks 30N/30TB; Peavey 5150 → Halen 51; Dizzy VH → Marshall-like estilo Van Halen), e a coluna correspondente (Brit = Marshall, Calif = Mesa, Halen = 5150, Voks = Vox, etc.).
            4. Define o gain staging: o ganho total vem do conjunto DRV + AMP. Não ponhas gain alto nos dois ao mesmo tempo, a não ser que o estilo peça (metal/grunge pesado).
            5. Acrescenta só os efeitos que existem no som original. Não encham a cadeia de módulos só porque há slots livres.
            6. Ajusta os níveis para que o patch tenha volume equilibrado (Volume dos módulos entre 45 e 60 por defeito).

            ## 2. Regras da cadeia
            - Máximo 12 slots, ordem típica: DYN → (FREQ/WAH) → DRV → AMP → CAB (ou IR) → EQ → MOD → DLY → RVB.
            - Wah vai antes do DRV; boost/overdrive antes do AMP; delay e reverb depois do CAB.
            - Usa CAB ou IR, nunca os dois. Um AMP precisa sempre de CAB (ou IR) a seguir.
            - Usa apenas 1 AMP e 1 CAB por patch, salvo pedido explícito.
            - Para pedidos de limpo/jazz/blues: AMP em modo CL, sem DRV ou com DRV de gain baixo, compressão leve.
            - Para pedidos acústicos: DYN AC Sim e CAB acústico, sem AMP.

            ## 3. Guia de gain (valores indicativos, 0–100)
            - Limpo: AMP Gain 15–30, sem DRV
            - Crunch: AMP Gain 40–60, DRV opcional com Gain 20–35 (boost)
            - Hard rock/Brown sound: AMP Gain 55–70
            - High gain/metal: AMP Gain 65–85 + DRV (tipo Skreamer/Master OD) com Gain 10–25 como boost de aperto
            - Fuzz (Hendrix, Smashing Pumpkins, etc.): DRV fuzz Gain/Fuzz 55–80, AMP limpo/crunch
            - Delay: Mix 10–25 (solos até 30); Feedback 15–40; Time em ms (slapback 80–150, eco de solo 350–500)
            - Reverb: Mix 10–25; Decay 30–55 para rock, mais alto só para ambiente/shoegaze
            - Chorus/Phaser/Flanger: Depth e Rate moderados, salvo pedido do efeito dominante (Rate 20–40 para suave)

            ## 4. Regras de formato (obrigatórias)
            - Cada slot usa um "algorithm" EXACTAMENTE como está na lista (maiúsculas, espaços, símbolos). Se o algoritmo tiver espaço no fim no nome da lista (ex.: "Mod Reverb "), mantém igual.
            - "params" tem de conter TODOS os knobs desse algoritmo, com os nomes exactos da lista (ex.: "Gain 1", "Low Cut", "1.6kHz").
            - Respeita o intervalo [min-max] de cada knob. Knobs 0–1 só aceitam 0 ou 1. Valores inteiros.
            - Não inventes algoritmos nem knobs. Se não existir um efeito equivalente, escolhe o mais próximo e explica na "note".
            - "name" com máximo 17 caracteres. "style" curto (Rock, Blues, Metal, Clean, etc.).
            - Omite slots vazios. "slot" começa em 1 e tem de ser sequencial pela ordem da cadeia.
            - "on": true nos módulos usados. Se um módulo for opcional, deixa-o false em vez de o remover.

            ## 5. Resposta
            Responde em português com UMA frase curta e depois UM único bloco JSON válido (sem comentários, sem vírgulas a sobrar, sem texto depois). Na "note" explica em 2–4 frases: o que cada módulo faz no timbre e o que o utilizador pode ajustar (ex.: "baixa o Gain do AMP para o som mais limpo na rítmica").

            ## 6. Auto-verificação antes de responder
            - Todos os algorithms existem na lista?
            - Todos os knobs de cada algoritmo estão presentes, com o nome exacto?
            - Todos os valores estão dentro do min-max?
            - A ordem da cadeia está correcta e há CAB/IR depois do AMP?
            - O ganho total corresponde ao timbre pedido?
            Se algo falhar, corrige antes de enviar.
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
