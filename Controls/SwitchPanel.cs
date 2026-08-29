using System.ComponentModel;

namespace ChocolateEditor.Controls;

[ToolboxItem(true)]
[DesignerCategory("UserControl")]
public partial class SwitchPanel : UserControl
{
    public SwitchPanel()
    {
        InitializeComponent();
        cmbBehavior.Items.AddRange(
        [
            UiLabels.Behavior(Models.SwitchBehavior.ShortTap),
            UiLabels.Behavior(Models.SwitchBehavior.ShortTapToggle),
            UiLabels.Behavior(Models.SwitchBehavior.PressRelease),
            UiLabels.Behavior(Models.SwitchBehavior.LongPress),
            UiLabels.Behavior(Models.SwitchBehavior.ShortAndLong)
        ]);
        cmbBehavior.SelectedIndex = 4;
    }

    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue("Switch A")]
    public string SwitchTitle
    {
        get => grpSwitch.Text;
        set => grpSwitch.Text = value;
    }

    public bool ShouldSerializeSwitchTitle() => grpSwitch.Text != "Switch A";
    public void ResetSwitchTitle() => grpSwitch.Text = "Switch A";

    public event EventHandler? Changed;

    public void LoadFrom(Models.FootswitchConfig config)
    {
        cmbBehavior.SelectedIndex = (int)config.Behavior;
        numCustomCc.Value = Math.Clamp(config.CustomCc, 0, 127);
        chkLatching.Checked = config.CustomLatching;
        FillList(lstShort, config.ShortPress);
        FillList(lstLong, config.LongPress);
    }

    public void SaveTo(Models.FootswitchConfig config)
    {
        config.Behavior = (Models.SwitchBehavior)Math.Clamp(cmbBehavior.SelectedIndex, 0, 4);
        config.CustomCc = (int)numCustomCc.Value;
        config.CustomLatching = chkLatching.Checked;
        config.ShortPress = Items(lstShort);
        config.LongPress = Items(lstLong);
    }

    private static void FillList(ListBox list, IEnumerable<Models.MidiCommand> commands)
    {
        list.Items.Clear();
        foreach (var cmd in commands)
        {
            list.Items.Add(cmd);
        }
    }

    private static List<Models.MidiCommand> Items(ListBox list)
    {
        return list.Items.Cast<Models.MidiCommand>().Select(c => c.Clone()).ToList();
    }

    private void btnAddShort_Click(object sender, EventArgs e) => Add(lstShort);
    private void btnEditShort_Click(object sender, EventArgs e) => Edit(lstShort);
    private void btnRemoveShort_Click(object sender, EventArgs e) => Remove(lstShort);
    private void btnAddLong_Click(object sender, EventArgs e) => Add(lstLong);
    private void btnEditLong_Click(object sender, EventArgs e) => Edit(lstLong);
    private void btnRemoveLong_Click(object sender, EventArgs e) => Remove(lstLong);
    private void lstShort_DoubleClick(object sender, EventArgs e) => Edit(lstShort);
    private void lstLong_DoubleClick(object sender, EventArgs e) => Edit(lstLong);

    private void Add(ListBox list)
    {
        using var dlg = new Forms.FormMidiCommand();
        if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
        {
            list.Items.Add(dlg.Command);
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Edit(ListBox list)
    {
        if (list.SelectedItem is not Models.MidiCommand cmd)
        {
            return;
        }

        using var dlg = new Forms.FormMidiCommand(cmd);
        if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
        {
            int i = list.SelectedIndex;
            list.Items[i] = dlg.Command;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Remove(ListBox list)
    {
        if (list.SelectedIndex < 0)
        {
            return;
        }

        list.Items.RemoveAt(list.SelectedIndex);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void cmbBehavior_SelectedIndexChanged(object sender, EventArgs e) => Changed?.Invoke(this, EventArgs.Empty);
    private void numCustomCc_ValueChanged(object sender, EventArgs e) => Changed?.Invoke(this, EventArgs.Empty);
    private void chkLatching_CheckedChanged(object sender, EventArgs e) => Changed?.Invoke(this, EventArgs.Empty);
}
