namespace ChocolateEditor.Forms;

partial class FormMidiCommand
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblType = new Label();
        cmbType = new ComboBox();
        lblChannel = new Label();
        numChannel = new NumericUpDown();
        lblData1 = new Label();
        numData1 = new NumericUpDown();
        lblData2 = new Label();
        numData2 = new NumericUpDown();
        lblSysEx = new Label();
        txtSysEx = new TextBox();
        lblPreview = new Label();
        btnOk = new Button();
        btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)numChannel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numData1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numData2).BeginInit();
        SuspendLayout();
        //
        // lblType
        //
        lblType.AutoSize = true;
        lblType.Location = new Point(16, 18);
        lblType.Name = "lblType";
        lblType.Size = new Size(31, 15);
        lblType.TabIndex = 0;
        lblType.Text = "Tipo";
        //
        // cmbType
        //
        cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbType.FormattingEnabled = true;
        cmbType.Location = new Point(110, 14);
        cmbType.Name = "cmbType";
        cmbType.Size = new Size(280, 23);
        cmbType.TabIndex = 1;
        cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;
        //
        // lblChannel
        //
        lblChannel.AutoSize = true;
        lblChannel.Location = new Point(16, 52);
        lblChannel.Name = "lblChannel";
        lblChannel.Size = new Size(46, 15);
        lblChannel.TabIndex = 2;
        lblChannel.Text = "Canal";
        //
        // numChannel
        //
        numChannel.Location = new Point(110, 48);
        numChannel.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
        numChannel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numChannel.Name = "numChannel";
        numChannel.Size = new Size(80, 23);
        numChannel.TabIndex = 3;
        numChannel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numChannel.ValueChanged += ValueChanged;
        //
        // lblData1
        //
        lblData1.AutoSize = true;
        lblData1.Location = new Point(16, 86);
        lblData1.Name = "lblData1";
        lblData1.Size = new Size(72, 15);
        lblData1.TabIndex = 4;
        lblData1.Text = "PC / Data 1";
        //
        // numData1
        //
        numData1.Location = new Point(110, 82);
        numData1.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
        numData1.Name = "numData1";
        numData1.Size = new Size(80, 23);
        numData1.TabIndex = 5;
        numData1.ValueChanged += ValueChanged;
        //
        // lblData2
        //
        lblData2.AutoSize = true;
        lblData2.Location = new Point(210, 86);
        lblData2.Name = "lblData2";
        lblData2.Size = new Size(42, 15);
        lblData2.TabIndex = 6;
        lblData2.Text = "Data 2";
        //
        // numData2
        //
        numData2.Location = new Point(270, 82);
        numData2.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
        numData2.Name = "numData2";
        numData2.Size = new Size(80, 23);
        numData2.TabIndex = 7;
        numData2.Value = new decimal(new int[] { 127, 0, 0, 0 });
        numData2.ValueChanged += ValueChanged;
        //
        // lblSysEx
        //
        lblSysEx.AutoSize = true;
        lblSysEx.Location = new Point(16, 120);
        lblSysEx.Name = "lblSysEx";
        lblSysEx.Size = new Size(88, 15);
        lblSysEx.TabIndex = 8;
        lblSysEx.Text = "SysEx (hex)";
        //
        // txtSysEx
        //
        txtSysEx.Enabled = false;
        txtSysEx.Location = new Point(110, 116);
        txtSysEx.Multiline = true;
        txtSysEx.Name = "txtSysEx";
        txtSysEx.PlaceholderText = "F0 00 32 ... F7";
        txtSysEx.Size = new Size(280, 64);
        txtSysEx.TabIndex = 9;
        txtSysEx.TextChanged += ValueChanged;
        //
        // lblPreview
        //
        lblPreview.AutoSize = true;
        lblPreview.Location = new Point(16, 192);
        lblPreview.Name = "lblPreview";
        lblPreview.Size = new Size(46, 15);
        lblPreview.TabIndex = 10;
        lblPreview.Text = "Prévia:";
        //
        // btnOk
        //
        btnOk.Location = new Point(214, 228);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(85, 28);
        btnOk.TabIndex = 11;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(305, 228);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(85, 28);
        btnCancel.TabIndex = 12;
        btnCancel.Text = "Cancelar";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // FormMidiCommand
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(408, 272);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Controls.Add(lblType);
        Controls.Add(cmbType);
        Controls.Add(lblChannel);
        Controls.Add(numChannel);
        Controls.Add(lblData1);
        Controls.Add(numData1);
        Controls.Add(lblData2);
        Controls.Add(numData2);
        Controls.Add(lblSysEx);
        Controls.Add(txtSysEx);
        Controls.Add(lblPreview);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormMidiCommand";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Mensagem MIDI";
        ((System.ComponentModel.ISupportInitialize)numChannel).EndInit();
        ((System.ComponentModel.ISupportInitialize)numData1).EndInit();
        ((System.ComponentModel.ISupportInitialize)numData2).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblType;
    private ComboBox cmbType;
    private Label lblChannel;
    private NumericUpDown numChannel;
    private Label lblData1;
    private NumericUpDown numData1;
    private Label lblData2;
    private NumericUpDown numData2;
    private Label lblSysEx;
    private TextBox txtSysEx;
    private Label lblPreview;
    private Button btnOk;
    private Button btnCancel;
}
