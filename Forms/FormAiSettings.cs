using ChocolateEditor.Ai;

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
}
