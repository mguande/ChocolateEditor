using ChocolateEditor.Ai;

namespace ChocolateEditor.Forms;

public partial class FormLauncher : Form
{
    public FormLauncher()
    {
        InitializeComponent();
    }

    private void FormLauncher_Shown(object sender, EventArgs e)
    {
        if (!AiSettingsStore.NeedsSetup())
        {
            return;
        }

        using var form = new FormAiSettings(firstRun: true);
        form.ShowDialog(this);
    }

    private void btnConfig_Click(object sender, EventArgs e)
    {
        using var form = new FormAiSettings();
        form.ShowDialog(this);
    }

    private void btnChocolate_Click(object sender, EventArgs e)
        => OpenEditor(new FormMain());

    private void btnScene_Click(object sender, EventArgs e)
        => OpenEditor(new FormChocolateMatribox());

    private void btnMatribox_Click(object sender, EventArgs e)
        => OpenEditor(new FormMatribox());

    private void btnAi_Click(object sender, EventArgs e)
        => OpenEditor(new FormAiPrompt());

    private void OpenEditor(Form editor)
    {
        Hide();
        editor.ShowDialog(this);
        editor.Dispose();
        Show();
        Activate();
    }
}
