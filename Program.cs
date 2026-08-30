using ChocolateEditor.Ui;

namespace ChocolateEditor;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetColorMode(SystemColorMode.Dark);
        Application.Run(new Forms.FormLauncher());
    }
}
