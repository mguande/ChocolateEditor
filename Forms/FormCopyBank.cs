using ChocolateEditor.Models;

namespace ChocolateEditor.Forms;

public partial class FormCopyBank : Form
{
    public FormCopyBank(DeviceConfiguration config, int currentBank)
    {
        InitializeComponent();
        config.EnsureBankCount();
        for (int i = 0; i < DeviceConfiguration.MaxBanks; i++)
        {
            cmbFrom.Items.Add(config.GetBank(i).ListLabel);
            cmbTo.Items.Add(config.GetBank(i).ListLabel);
        }

        cmbFrom.SelectedIndex = Math.Clamp(currentBank, 0, DeviceConfiguration.MaxBanks - 1);
        cmbTo.SelectedIndex = Math.Min(currentBank + 1, DeviceConfiguration.MaxBanks - 1);
        numToEnd.Value = DeviceConfiguration.MaxBanks;
        chkA.Checked = true;
        chkB.Checked = true;
        chkC.Checked = true;
        chkD.Checked = true;
        chkShort.Checked = true;
        chkLong.Checked = true;
    }

    public int FromIndex => cmbFrom.SelectedIndex;
    public int ToIndex => cmbTo.SelectedIndex;
    public int ToEndIndex => (int)numToEnd.Value - 1;
    public bool CopyRange => chkRange.Checked;
    public bool CopyA => chkA.Checked;
    public bool CopyB => chkB.Checked;
    public bool CopyC => chkC.Checked;
    public bool CopyD => chkD.Checked;
    public bool CopyShort => chkShort.Checked;
    public bool CopyLong => chkLong.Checked;

    private void chkRange_CheckedChanged(object sender, EventArgs e)
    {
        numToEnd.Enabled = chkRange.Checked;
        lblToEnd.Enabled = chkRange.Checked;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (FromIndex < 0 || ToIndex < 0)
        {
            MessageBox.Show(this, "Escolha os bancos de origem e destino.", "Copiar banco",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!CopyA && !CopyB && !CopyC && !CopyD)
        {
            MessageBox.Show(this, "Marque pelo menos um switch (A–D).", "Copiar banco",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!CopyShort && !CopyLong)
        {
            MessageBox.Show(this, "Marque toque curto e/ou longo.", "Copiar banco",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!CopyRange && FromIndex == ToIndex)
        {
            MessageBox.Show(this, "Origem e destino são o mesmo banco.", "Copiar banco",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
