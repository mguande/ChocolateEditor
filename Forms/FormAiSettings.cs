using ChocolateEditor.Ai;
using ChocolateEditor.Matribox;

namespace ChocolateEditor.Forms;

public partial class FormAiSettings : Form
{
    private readonly bool _firstRun;

    public FormAiSettings() : this(firstRun: false)
    {
    }

    public FormAiSettings(bool firstRun)
    {
        _firstRun = firstRun;
        InitializeComponent();
        var s = AiSettingsStore.Load();
        txtOpenAiKey.Text = s.OpenAiApiKey;
        txtOpenAiModel.Text = s.ModelOf(AiProvider.ChatGpt);
        txtClaudeKey.Text = s.ClaudeApiKey;
        txtClaudeModel.Text = s.ModelOf(AiProvider.Claude);
        txtGeminiKey.Text = s.GeminiApiKey;
        txtGeminiModel.Text = s.ModelOf(AiProvider.Gemini);
        txtInstructions.Text = string.IsNullOrWhiteSpace(s.PatchInstruction)
            ? MatriboxPatchPrompt.DefaultSystemInstruction
            : s.PatchInstruction;
        txtPrstFolder.Text = s.PrstFolder;
        RefreshPrstStatus();
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        if (_firstRun)
        {
            Text = "Configuração inicial";
            lblHint.Text = "Primeira execução: cola as chaves da IA (podes saltar e configurar depois no início).\nFicam só neste PC. ChatGPT: platform.openai.com  ·  Claude: console.anthropic.com  ·  Gemini: aistudio.google.com";
            btnCancel.Text = "Agora não";
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        var current = AiSettingsStore.Load();
        AiSettingsStore.Save(new AiSettings
        {
            OpenAiApiKey = txtOpenAiKey.Text.Trim(),
            OpenAiModel = string.IsNullOrWhiteSpace(txtOpenAiModel.Text)
                ? "gpt-4o-mini"
                : txtOpenAiModel.Text.Trim(),
            ClaudeApiKey = txtClaudeKey.Text.Trim(),
            ClaudeModel = string.IsNullOrWhiteSpace(txtClaudeModel.Text)
                ? "claude-sonnet-4-5"
                : txtClaudeModel.Text.Trim(),
            GeminiApiKey = txtGeminiKey.Text.Trim(),
            GeminiModel = string.IsNullOrWhiteSpace(txtGeminiModel.Text)
                ? "gemini-2.5-flash"
                : txtGeminiModel.Text.Trim(),
            LastProvider = current.LastProvider,
            IncludeMatriboxManual = current.IncludeMatriboxManual,
            PatchInstruction = InstructionToStore(txtInstructions.Text),
            PrstFolder = txtPrstFolder.Text.Trim(),
            SetupCompleted = true
        });
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (_firstRun)
        {
            var current = AiSettingsStore.Load();
            current.SetupCompleted = true;
            AiSettingsStore.Save(current);
        }

        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void btnResetInstructions_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this,
                "Repor o texto original das instruções?",
                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        txtInstructions.Text = MatriboxPatchPrompt.DefaultSystemInstruction;
    }

    private void btnBrowsePrst_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "Pasta dos .prst do editor Sonicake",
            UseDescriptionForTitle = true
        };
        if (Directory.Exists(txtPrstFolder.Text))
        {
            dlg.SelectedPath = txtPrstFolder.Text;
            dlg.InitialDirectory = txtPrstFolder.Text;
        }

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            txtPrstFolder.Text = dlg.SelectedPath;
        }
    }

    private void txtPrstFolder_TextChanged(object sender, EventArgs e) => RefreshPrstStatus();

    private void RefreshPrstStatus()
    {
        string folder = txtPrstFolder.Text.Trim();
        if (folder.Length == 0)
        {
            lblPrstStatus.Text = "Sem pasta definida. O programa procura nas pastas padrão do Sonicake.";
            return;
        }

        if (!Directory.Exists(folder))
        {
            lblPrstStatus.Text = "Esta pasta não existe.";
            return;
        }

        int count;
        try
        {
            count = Directory.EnumerateFiles(folder, "*.prst").Count();
        }
        catch
        {
            lblPrstStatus.Text = "Não foi possível ler esta pasta.";
            return;
        }

        lblPrstStatus.Text = count == 0
            ? "A pasta existe, mas não tem ficheiros .prst."
            : count == 1 ? "1 ficheiro .prst encontrado." : count + " ficheiros .prst encontrados.";
    }

    private static string InstructionToStore(string text)
    {
        string edited = text.Replace("\r\n", "\n").Trim();
        string fallback = MatriboxPatchPrompt.DefaultSystemInstruction.Replace("\r\n", "\n").Trim();
        return edited.Length == 0 || edited == fallback ? "" : edited;
    }
}
