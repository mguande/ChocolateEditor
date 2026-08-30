using System.Net;
using ChocolateEditor.Ai;
using ChocolateEditor.Controls;
using ChocolateEditor.Matribox;

using ChocolateEditor.Ui;

namespace ChocolateEditor.Forms;

public partial class FormAiPrompt : DarkForm
{
    private MatriboxPatchDraft? _draft;

    public FormAiPrompt()
    {
        InitializeComponent();
        cmbProvider.Items.AddRange(["ChatGPT", "Claude", "Gemini"]);
        var settings = AiSettingsStore.Load();
        cmbProvider.SelectedIndex = settings.LastProvider switch
        {
            AiProvider.Claude => 1,
            AiProvider.Gemini => 2,
            _ => 0
        };
        chkSendManual.Checked = settings.IncludeMatriboxManual;
        SyncManualCheckbox();
        ShowPlain("A resposta aparece aqui, como numa página do browser.");
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        txtPrompt.KeyDown += txtPrompt_KeyDown;
    }

    private void txtPrompt_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnSend_Click(btnSend, EventArgs.Empty);
        }
    }

    private AiProvider SelectedProvider() => cmbProvider.SelectedIndex switch
    {
        1 => AiProvider.Claude,
        2 => AiProvider.Gemini,
        _ => AiProvider.ChatGpt
    };

    private void btnInicio_Click(object sender, EventArgs e) => Close();

    private void btnSettings_Click(object sender, EventArgs e)
    {
        using var form = new FormAiSettings();
        form.ShowDialog(this);
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
        var settings = AiSettingsStore.Load();
        var provider = SelectedProvider();
        if (!settings.HasKey(provider))
        {
            MessageBox.Show(this,
                $"Falta a chave da API do {AiProviderNames.Display(provider)}.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            using var form = new FormAiSettings();
            form.ShowDialog(this);
            return;
        }

        settings.LastProvider = provider;
        settings.IncludeMatriboxManual = chkSendManual.Checked;
        AiSettingsStore.Save(settings);

        bool patch = chkPatchMatribox.Checked;
        bool manual = patch && chkSendManual.Checked;
        string question = txtPrompt.Text;
        string prompt = patch ? MatriboxPatchPrompt.WrapUserPrompt(question, manual) : question;
        string? system = patch ? MatriboxPatchPrompt.SystemInstruction(manual) : null;

        _draft = null;
        btnExportPrst.Enabled = false;
        SetBusy(true);
        ShowPlain("A pedir resposta…");
        try
        {
            string reply = await AiChatClient.AskAsync(settings, provider, prompt, system);
            if (patch && MatriboxPatchPrompt.TryParse(reply, out var draft))
            {
                draft.Question = question;
                draft.Answer = reply;
                _draft = draft;
                btnExportPrst.Enabled = true;
                ShowPatch(reply, draft);
            }
            else
            {
                ShowPlain(reply);
            }
        }
        catch (Exception ex)
        {
            ShowPlain("Erro:\n\n" + ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void btnExportPrst_Click(object sender, EventArgs e)
    {
        if (_draft is null)
        {
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Guardar patch Matribox",
            Filter = "Patch Matribox (*.prst)|*.prst",
            FileName = $"01-A {_draft.FileStem}.prst",
            InitialDirectory = MatriboxPrstImport.FirstExistingDefaultFolder()
                               ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            MatriboxPrstExport.Write(dlg.FileName, _draft);
            string txtPath = Path.ChangeExtension(dlg.FileName, ".txt");
            File.WriteAllText(txtPath, FormatSessionLog(_draft, SelectedProvider()));
            MessageBox.Show(this,
                $"Gravado:\n{dlg.FileName}\n{txtPath}\n\nCopia o .prst para a pasta Patchs do editor Sonicake.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Guardar .prst", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        btnSend.Enabled = !busy;
        txtPrompt.Enabled = !busy;
        cmbProvider.Enabled = !busy;
        btnSettings.Enabled = !busy;
        chkPatchMatribox.Enabled = !busy;
        SyncManualCheckbox();
        btnExportPrst.Enabled = !busy && _draft is not null;
        btnSend.Text = busy ? "A aguardar…" : "Enviar";
        btnSend.Invalidate();
    }

    private void chkPatchMatribox_CheckedChanged(object? sender, EventArgs e)
        => SyncManualCheckbox();

    private void chkSendManual_CheckedChanged(object? sender, EventArgs e)
    {
        var s = AiSettingsStore.Load();
        s.IncludeMatriboxManual = chkSendManual.Checked;
        AiSettingsStore.Save(s);
    }

    private void SyncManualCheckbox()
        => chkSendManual.Enabled = chkPatchMatribox.Enabled && chkPatchMatribox.Checked;

    private static string FormatSessionLog(MatriboxPatchDraft draft, AiProvider provider)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Chocolate Plus Editor — patch Matribox gerado por IA");
        sb.AppendLine("Data: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        sb.AppendLine("Modelo: " + AiProviderNames.Display(provider));
        sb.AppendLine("Patch: " + draft.Name);
        if (!string.IsNullOrWhiteSpace(draft.Style))
        {
            sb.AppendLine("Estilo: " + draft.Style);
        }

        sb.AppendLine();
        sb.AppendLine("=== Pergunta ===");
        sb.AppendLine(draft.Question.Trim());
        sb.AppendLine();
        sb.AppendLine("=== Resposta ===");
        sb.AppendLine(draft.Answer.Trim());
        sb.AppendLine();
        sb.AppendLine("=== Cadeia ===");
        foreach (var slot in draft.Chain.Where(s => !s.Empty))
        {
            string alg = MatriboxAlgCatalog.Name(slot.Algorithm);
            sb.Append(slot.Position + 1).Append("  ")
                .Append(HotoneSysex.ModuleTypeNames[slot.ModuleType]).Append("  ")
                .Append(alg).Append("  ").Append(slot.On ? "On" : "Off");
            if (slot.Parameters.Count > 0)
            {
                sb.Append("  ").AppendJoin(", ", slot.Parameters.Select(p => $"{p.Name}={p.Value}"));
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private void ShowPatch(string reply, MatriboxPatchDraft draft)
    {
        webReply.Visible = false;
        grpChain.Visible = true;
        lblPatchTitle.Text = draft.Name;
        lblPatchMeta.Text = string.Join(" · ", new[] { draft.Style, draft.Note }.Where(s => !string.IsNullOrWhiteSpace(s)));
        lblPatchNote.Text = MatriboxPatchPrompt.ReplyWithoutJson(reply);
        flowChain.SuspendLayout();
        flowChain.Controls.Clear();
        foreach (var slot in draft.Chain.Where(s => !s.Empty))
        {
            string alg = MatriboxAlgCatalog.Name(slot.Algorithm);
            if (string.IsNullOrWhiteSpace(alg))
            {
                alg = slot.EffectName;
            }

            var card = new PatchGearCard(
                PatchGearCard.KindOf(slot.ModuleType),
                HotoneSysex.ModuleTypeNames[slot.ModuleType],
                alg,
                slot.On,
                slot.Parameters);
            flowChain.Controls.Add(card);
        }

        flowChain.ResumeLayout();
        btnExportPrst.Enabled = true;
    }

    private void ShowPlain(string text)
    {
        grpChain.Visible = false;
        webReply.Visible = true;
        ShowHtml("<pre>" + WebUtility.HtmlEncode(text) + "</pre>", preWrap: true);
    }

    private void ShowHtml(string inner, bool preWrap)
    {
        string html = $$"""
            <!DOCTYPE html>
            <html>
            <head>
            <meta charset="utf-8">
            <style>
              html, body { margin: 0; padding: 0; background: #121214; }
              body {
                font-family: "Segoe UI", Arial, sans-serif;
                font-size: 15px;
                line-height: 1.5;
                color: #ececf0;
                padding: 20px 24px;
                {{(preWrap ? "white-space: pre-wrap;" : "")}}
                word-wrap: break-word;
              }
              h1 { font-size: 22px; margin: 0 0 6px; color: #fff; }
              .meta { color: #a8a8b0; margin: 0 0 16px; }
              table { border-collapse: collapse; width: 100%; max-width: 720px; margin: 12px 0 20px; }
              th, td { border: 1px solid #484850; padding: 6px 10px; text-align: left; }
              th { background: #28282e; }
              .hint { color: #c8c8d0; }
              pre { white-space: pre-wrap; font-family: "Segoe UI", Arial, sans-serif; color: #ececf0; }
            </style>
            </head>
            <body>
            {{inner}}
            </body></html>
            """;
        webReply.DocumentText = html;
    }
}
