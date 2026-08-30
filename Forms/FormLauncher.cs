namespace ChocolateEditor.Forms;

public partial class FormLauncher : Form
{
    public FormLauncher()
    {
        InitializeComponent();
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
