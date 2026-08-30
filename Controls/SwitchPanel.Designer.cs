using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

namespace ChocolateEditor.Controls;

partial class SwitchPanel
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        grpSwitch = new GroupBox();
        cmbBehavior = new ComboBox();
        lblBehavior = new Label();
        lblShort = new Label();
        lstShort = new ListBox();
        btnAddShort = new ForeverButton();
        btnEditShort = new ForeverButton();
        btnRemoveShort = new ForeverButton();
        lblLong = new Label();
        lstLong = new ListBox();
        btnAddLong = new ForeverButton();
        btnEditLong = new ForeverButton();
        btnRemoveLong = new ForeverButton();
        lblCustomCc = new Label();
        numCustomCc = new NumericUpDown();
        chkLatching = new CheckBox();
        grpSwitch.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCustomCc).BeginInit();
        SuspendLayout();
        //
        // grpSwitch
        //
        grpSwitch.Controls.Add(chkLatching);
        grpSwitch.Controls.Add(numCustomCc);
        grpSwitch.Controls.Add(lblCustomCc);
        grpSwitch.Controls.Add(btnRemoveLong);
        grpSwitch.Controls.Add(btnEditLong);
        grpSwitch.Controls.Add(btnAddLong);
        grpSwitch.Controls.Add(lstLong);
        grpSwitch.Controls.Add(lblLong);
        grpSwitch.Controls.Add(btnRemoveShort);
        grpSwitch.Controls.Add(btnEditShort);
        grpSwitch.Controls.Add(btnAddShort);
        grpSwitch.Controls.Add(lstShort);
        grpSwitch.Controls.Add(lblShort);
        grpSwitch.Controls.Add(cmbBehavior);
        grpSwitch.Controls.Add(lblBehavior);
        grpSwitch.Dock = DockStyle.Fill;
        grpSwitch.Location = new Point(0, 0);
        grpSwitch.Name = "grpSwitch";
        grpSwitch.Size = new Size(360, 330);
        grpSwitch.TabIndex = 0;
        grpSwitch.TabStop = false;
        grpSwitch.Text = "Switch A";
        //
        // lblBehavior
        //
        lblBehavior.AutoSize = true;
        lblBehavior.Location = new Point(12, 24);
        lblBehavior.Name = "lblBehavior";
        lblBehavior.Size = new Size(48, 15);
        lblBehavior.TabIndex = 0;
        lblBehavior.Text = "Modo:";
        //
        // cmbBehavior
        //
        cmbBehavior.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBehavior.FormattingEnabled = true;
        cmbBehavior.Location = new Point(66, 20);
        cmbBehavior.Name = "cmbBehavior";
        cmbBehavior.Size = new Size(280, 23);
        cmbBehavior.TabIndex = 1;
        cmbBehavior.SelectedIndexChanged += cmbBehavior_SelectedIndexChanged;
        //
        // lblShort
        //
        lblShort.AutoSize = true;
        lblShort.Location = new Point(12, 52);
        lblShort.Name = "lblShort";
        lblShort.Size = new Size(120, 15);
        lblShort.TabIndex = 2;
        lblShort.Text = "A Bank (pressionar / curto)";
        //
        // lstShort
        //
        lstShort.FormattingEnabled = true;
        lstShort.ItemHeight = 15;
        lstShort.Location = new Point(12, 72);
        lstShort.Name = "lstShort";
        lstShort.ScrollAlwaysVisible = true;
        lstShort.Size = new Size(250, 79);
        lstShort.TabIndex = 3;
        lstShort.DoubleClick += lstShort_DoubleClick;
        //
        // btnAddShort
        //
        btnAddShort.Location = new Point(268, 72);
        btnAddShort.Name = "btnAddShort";
        btnAddShort.Rounded = true;
        btnAddShort.Size = new Size(80, 26);
        btnAddShort.TabIndex = 4;
        btnAddShort.Text = "Adicionar";
        btnAddShort.TextColor = Color.White;
        btnAddShort.Click += btnAddShort_Click;
        //
        // btnEditShort
        //
        btnEditShort.Location = new Point(268, 100);
        btnEditShort.Name = "btnEditShort";
        btnEditShort.Rounded = true;
        btnEditShort.Size = new Size(80, 26);
        btnEditShort.TabIndex = 5;
        btnEditShort.Text = "Editar";
        btnEditShort.TextColor = Color.White;
        btnEditShort.Click += btnEditShort_Click;
        //
        // btnRemoveShort
        //
        btnRemoveShort.Location = new Point(268, 128);
        btnRemoveShort.Name = "btnRemoveShort";
        btnRemoveShort.Rounded = true;
        btnRemoveShort.Size = new Size(80, 26);
        btnRemoveShort.TabIndex = 6;
        btnRemoveShort.Text = "Remover";
        btnRemoveShort.TextColor = Color.White;
        btnRemoveShort.Click += btnRemoveShort_Click;
        //
        // lblLong
        //
        lblLong.AutoSize = true;
        lblLong.Location = new Point(12, 160);
        lblLong.Name = "lblLong";
        lblLong.Size = new Size(120, 15);
        lblLong.TabIndex = 7;
        lblLong.Text = "B Bank (soltar / longo)";
        //
        // lstLong
        //
        lstLong.FormattingEnabled = true;
        lstLong.ItemHeight = 15;
        lstLong.Location = new Point(12, 180);
        lstLong.Name = "lstLong";
        lstLong.ScrollAlwaysVisible = true;
        lstLong.Size = new Size(250, 79);
        lstLong.TabIndex = 8;
        lstLong.DoubleClick += lstLong_DoubleClick;
        //
        // btnAddLong
        //
        btnAddLong.Location = new Point(268, 180);
        btnAddLong.Name = "btnAddLong";
        btnAddLong.Rounded = true;
        btnAddLong.Size = new Size(80, 26);
        btnAddLong.TabIndex = 9;
        btnAddLong.Text = "Adicionar";
        btnAddLong.TextColor = Color.White;
        btnAddLong.Click += btnAddLong_Click;
        //
        // btnEditLong
        //
        btnEditLong.Location = new Point(268, 208);
        btnEditLong.Name = "btnEditLong";
        btnEditLong.Rounded = true;
        btnEditLong.Size = new Size(80, 26);
        btnEditLong.TabIndex = 10;
        btnEditLong.Text = "Editar";
        btnEditLong.TextColor = Color.White;
        btnEditLong.Click += btnEditLong_Click;
        //
        // btnRemoveLong
        //
        btnRemoveLong.Location = new Point(268, 236);
        btnRemoveLong.Name = "btnRemoveLong";
        btnRemoveLong.Rounded = true;
        btnRemoveLong.Size = new Size(80, 26);
        btnRemoveLong.TabIndex = 11;
        btnRemoveLong.Text = "Remover";
        btnRemoveLong.TextColor = Color.White;
        btnRemoveLong.Click += btnRemoveLong_Click;
        //
        // lblCustomCc
        //
        lblCustomCc.AutoSize = true;
        lblCustomCc.Location = new Point(12, 272);
        lblCustomCc.Name = "lblCustomCc";
        lblCustomCc.Size = new Size(88, 15);
        lblCustomCc.TabIndex = 12;
        lblCustomCc.Text = "CC (modo 3):";
        //
        // numCustomCc
        //
        numCustomCc.Location = new Point(106, 268);
        numCustomCc.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
        numCustomCc.Name = "numCustomCc";
        numCustomCc.Size = new Size(70, 23);
        numCustomCc.TabIndex = 13;
        numCustomCc.ValueChanged += numCustomCc_ValueChanged;
        //
        // chkLatching
        //
        chkLatching.AutoSize = true;
        chkLatching.Location = new Point(190, 270);
        chkLatching.Name = "chkLatching";
        chkLatching.Size = new Size(140, 19);
        chkLatching.TabIndex = 14;
        chkLatching.Text = "Latching (toggle)";
        chkLatching.UseVisualStyleBackColor = true;
        chkLatching.CheckedChanged += chkLatching_CheckedChanged;
        //
        // SwitchPanel
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(grpSwitch);
        Name = "SwitchPanel";
        Size = new Size(360, 330);
        grpSwitch.ResumeLayout(false);
        grpSwitch.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCustomCc).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpSwitch;
    private ComboBox cmbBehavior;
    private Label lblBehavior;
    private Label lblShort;
    private ListBox lstShort;
    private ForeverButton btnAddShort;
    private ForeverButton btnEditShort;
    private ForeverButton btnRemoveShort;
    private Label lblLong;
    private ListBox lstLong;
    private ForeverButton btnAddLong;
    private ForeverButton btnEditLong;
    private ForeverButton btnRemoveLong;
    private Label lblCustomCc;
    private NumericUpDown numCustomCc;
    private CheckBox chkLatching;
}
