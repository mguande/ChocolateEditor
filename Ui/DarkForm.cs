namespace ChocolateEditor.Ui;

public class DarkForm : Form
{
    protected virtual bool AutoThemeControls => true;

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UiTheme.UseDarkTitleBar(this);
        if (!DesignMode && AutoThemeControls)
        {
            UiTheme.ApplyTree(this);
        }
    }
}
