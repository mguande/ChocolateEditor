namespace ChocolateEditor.Forms;

partial class FormCopyBank
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
        lblFrom = new Label();
        cmbFrom = new ComboBox();
        lblTo = new Label();
        cmbTo = new ComboBox();
        chkRange = new CheckBox();
        lblToEnd = new Label();
        numToEnd = new NumericUpDown();
        grpSwitches = new GroupBox();
        chkA = new CheckBox();
        chkB = new CheckBox();
        chkC = new CheckBox();
        chkD = new CheckBox();
        grpMessages = new GroupBox();
        chkShort = new CheckBox();
        chkLong = new CheckBox();
        btnOk = new Button();
        btnCancel = new Button();
        lblHint = new Label();
        grpSwitches.SuspendLayout();
        grpMessages.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numToEnd).BeginInit();
        SuspendLayout();
        //
        // lblFrom
        //
        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(16, 18);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(90, 15);
        lblFrom.TabIndex = 0;
        lblFrom.Text = "Banco origem";
        //
        // cmbFrom
        //
        cmbFrom.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFrom.FormattingEnabled = true;
        cmbFrom.Location = new Point(130, 14);
        cmbFrom.Name = "cmbFrom";
        cmbFrom.Size = new Size(250, 23);
        cmbFrom.TabIndex = 1;
        //
        // lblTo
        //
        lblTo.AutoSize = true;
        lblTo.Location = new Point(16, 52);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(92, 15);
        lblTo.TabIndex = 2;
        lblTo.Text = "Banco destino";
        //
        // cmbTo
        //
        cmbTo.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTo.FormattingEnabled = true;
        cmbTo.Location = new Point(130, 48);
        cmbTo.Name = "cmbTo";
        cmbTo.Size = new Size(250, 23);
        cmbTo.TabIndex = 3;
        //
        // chkRange
        //
        chkRange.AutoSize = true;
        chkRange.Location = new Point(130, 82);
        chkRange.Name = "chkRange";
        chkRange.Size = new Size(220, 19);
        chkRange.TabIndex = 4;
        chkRange.Text = "Copiar para um intervalo de bancos";
        chkRange.UseVisualStyleBackColor = true;
        chkRange.CheckedChanged += chkRange_CheckedChanged;
        //
        // lblToEnd
        //
        lblToEnd.AutoSize = true;
        lblToEnd.Enabled = false;
        lblToEnd.Location = new Point(16, 114);
        lblToEnd.Name = "lblToEnd";
        lblToEnd.Size = new Size(108, 15);
        lblToEnd.TabIndex = 5;
        lblToEnd.Text = "Até o banco nº";
        //
        // numToEnd
        //
        numToEnd.Enabled = false;
        numToEnd.Location = new Point(130, 110);
        numToEnd.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
        numToEnd.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numToEnd.Name = "numToEnd";
        numToEnd.Size = new Size(80, 23);
        numToEnd.TabIndex = 6;
        numToEnd.Value = new decimal(new int[] { 16, 0, 0, 0 });
        //
        // grpSwitches
        //
        grpSwitches.Controls.Add(chkA);
        grpSwitches.Controls.Add(chkB);
        grpSwitches.Controls.Add(chkC);
        grpSwitches.Controls.Add(chkD);
        grpSwitches.Location = new Point(16, 148);
        grpSwitches.Name = "grpSwitches";
        grpSwitches.Size = new Size(364, 56);
        grpSwitches.TabIndex = 7;
        grpSwitches.TabStop = false;
        grpSwitches.Text = "Switches";
        //
        // chkA
        //
        chkA.AutoSize = true;
        chkA.Location = new Point(16, 24);
        chkA.Name = "chkA";
        chkA.Size = new Size(33, 19);
        chkA.TabIndex = 0;
        chkA.Text = "A";
        chkA.UseVisualStyleBackColor = true;
        //
        // chkB
        //
        chkB.AutoSize = true;
        chkB.Location = new Point(70, 24);
        chkB.Name = "chkB";
        chkB.Size = new Size(33, 19);
        chkB.TabIndex = 1;
        chkB.Text = "B";
        chkB.UseVisualStyleBackColor = true;
        //
        // chkC
        //
        chkC.AutoSize = true;
        chkC.Location = new Point(124, 24);
        chkC.Name = "chkC";
        chkC.Size = new Size(33, 19);
        chkC.TabIndex = 2;
        chkC.Text = "C";
        chkC.UseVisualStyleBackColor = true;
        //
        // chkD
        //
        chkD.AutoSize = true;
        chkD.Location = new Point(178, 24);
        chkD.Name = "chkD";
        chkD.Size = new Size(33, 19);
        chkD.TabIndex = 3;
        chkD.Text = "D";
        chkD.UseVisualStyleBackColor = true;
        //
        // grpMessages
        //
        grpMessages.Controls.Add(chkShort);
        grpMessages.Controls.Add(chkLong);
        grpMessages.Location = new Point(16, 214);
        grpMessages.Name = "grpMessages";
        grpMessages.Size = new Size(364, 56);
        grpMessages.TabIndex = 8;
        grpMessages.TabStop = false;
        grpMessages.Text = "Mensagens";
        //
        // chkShort
        //
        chkShort.AutoSize = true;
        chkShort.Location = new Point(16, 24);
        chkShort.Name = "chkShort";
        chkShort.Size = new Size(96, 19);
        chkShort.TabIndex = 0;
        chkShort.Text = "Toque curto";
        chkShort.UseVisualStyleBackColor = true;
        //
        // chkLong
        //
        chkLong.AutoSize = true;
        chkLong.Location = new Point(140, 24);
        chkLong.Name = "chkLong";
        chkLong.Size = new Size(98, 19);
        chkLong.TabIndex = 1;
        chkLong.Text = "Toque longo";
        chkLong.UseVisualStyleBackColor = true;
        //
        // lblHint
        //
        lblHint.Location = new Point(16, 280);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(364, 36);
        lblHint.TabIndex = 9;
        lblHint.Text = "O banco destino é sobrescrito com uma cópia do origem. Depois você pode alterar só o que for diferente.";
        //
        // btnOk
        //
        btnOk.Location = new Point(204, 328);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(85, 28);
        btnOk.TabIndex = 10;
        btnOk.Text = "Copiar";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(295, 328);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(85, 28);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Cancelar";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // FormCopyBank
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(398, 372);
        Controls.Add(lblFrom);
        Controls.Add(cmbFrom);
        Controls.Add(lblTo);
        Controls.Add(cmbTo);
        Controls.Add(chkRange);
        Controls.Add(lblToEnd);
        Controls.Add(numToEnd);
        Controls.Add(grpSwitches);
        Controls.Add(grpMessages);
        Controls.Add(lblHint);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormCopyBank";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Copiar banco";
        grpSwitches.ResumeLayout(false);
        grpSwitches.PerformLayout();
        grpMessages.ResumeLayout(false);
        grpMessages.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numToEnd).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFrom;
    private ComboBox cmbFrom;
    private Label lblTo;
    private ComboBox cmbTo;
    private CheckBox chkRange;
    private Label lblToEnd;
    private NumericUpDown numToEnd;
    private GroupBox grpSwitches;
    private CheckBox chkA;
    private CheckBox chkB;
    private CheckBox chkC;
    private CheckBox chkD;
    private GroupBox grpMessages;
    private CheckBox chkShort;
    private CheckBox chkLong;
    private Button btnOk;
    private Button btnCancel;
    private Label lblHint;
}
