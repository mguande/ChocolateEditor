using ChocolateEditor.Ai;
using ChocolateEditor.Ui;

namespace ChocolateEditor.Forms;

public partial class FormLauncher : DarkForm
{
    protected override bool AutoThemeControls => false;

    public FormLauncher()
    {
        InitializeComponent();
        BackColor = UiTheme.Bg;
        lblTitle.ForeColor = UiTheme.Text;
        lblSub.ForeColor = UiTheme.Muted;
        lblHint.ForeColor = UiTheme.Muted;
        UiTheme.StyleTile(btnChocolate, UiTheme.Chocolate);
        UiTheme.StyleTile(btnScene, UiTheme.Blue);
        UiTheme.StyleTile(btnMatribox, UiTheme.Teal);
        UiTheme.StyleTile(btnAi, Color.FromArgb(58, 58, 78));
        UiTheme.StyleTile(btnConfig, Color.FromArgb(78, 68, 58));
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
