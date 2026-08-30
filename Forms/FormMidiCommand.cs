using ChocolateEditor.Models;
using ChocolateEditor.Ui;

namespace ChocolateEditor.Forms;

public partial class FormMidiCommand : DarkForm
{
    public MidiCommand Command { get; private set; }

    public FormMidiCommand()
        : this(MidiCommand.ProgramChange(0))
    {
    }

    public FormMidiCommand(MidiCommand command)
    {
        Command = command.Clone();
        InitializeComponent();
        cmbType.Items.AddRange(
        [
            UiLabels.CommandType(MidiCommandType.ProgramChange),
            UiLabels.CommandType(MidiCommandType.ControlChange),
            UiLabels.CommandType(MidiCommandType.NoteOn),
            UiLabels.CommandType(MidiCommandType.NoteOff),
            UiLabels.CommandType(MidiCommandType.SysEx)
        ]);
        cmbType.SelectedIndex = (int)Command.Type;
        numChannel.Value = Math.Clamp(Command.Channel, 1, 16);
        numData1.Value = Math.Clamp(Command.Data1, 0, 127);
        numData2.Value = Math.Clamp(Command.Data2, 0, 127);
        txtSysEx.Text = Command.SysExHex;
        UpdatePreview();
        KeyPreview = true;
        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                btnCancel_Click(btnCancel, EventArgs.Empty);
            }
        };
    }

    private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool sysex = cmbType.SelectedIndex == (int)MidiCommandType.SysEx;
        txtSysEx.Enabled = sysex;
        numChannel.Enabled = !sysex;
        numData1.Enabled = !sysex;
        numData2.Enabled = !sysex;
        lblData1.Text = cmbType.SelectedIndex switch
        {
            (int)MidiCommandType.ControlChange => "CC nº",
            (int)MidiCommandType.NoteOn or (int)MidiCommandType.NoteOff => "Nota",
            _ => "PC / Data 1"
        };
        lblData2.Text = cmbType.SelectedIndex switch
        {
            (int)MidiCommandType.ControlChange => "Valor",
            (int)MidiCommandType.NoteOn or (int)MidiCommandType.NoteOff => "Velocity",
            _ => "Data 2"
        };
        UpdatePreview();
    }

    private void ValueChanged(object sender, EventArgs e) => UpdatePreview();

    private void UpdatePreview()
    {
        var cmd = BuildCommand();
        lblPreview.Text = "Prévia: " + cmd.Summary;
    }

    private MidiCommand BuildCommand()
    {
        return new MidiCommand
        {
            Type = (MidiCommandType)Math.Max(0, cmbType.SelectedIndex),
            Channel = (int)numChannel.Value,
            Data1 = (int)numData1.Value,
            Data2 = (int)numData2.Value,
            SysExHex = txtSysEx.Text.Trim()
        };
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        Command = BuildCommand();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
