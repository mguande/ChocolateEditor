namespace ChocolateEditor.Forms;

partial class FormChocolateMatribox
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop = new Panel();
        btnInicio = new Button();
        lblTitle = new Label();
        cmbChocolateMidi = new ComboBox();
        btnConnect = new Button();
        btnRead = new Button();
        btnWrite = new Button();
        lblChannel = new Label();
        numChannel = new NumericUpDown();
        panelChoco = new Panel();
        tabChoco = new Label();
        lblBanco = new Label();
        cmbChocoBank = new ComboBox();
        btnSwA = new Button();
        btnSwB = new Button();
        btnSwC = new Button();
        btnSwD = new Button();
        lblChocoHint = new Label();
        chkStomp = new CheckBox();
        panelMatribox = new Panel();
        tabMatribox = new Label();
        splitMatribox = new SplitContainer();
        panelBancos = new Panel();
        tabBancos = new Label();
        lstPatches = new ListBox();
        panelFx = new Panel();
        tabFx = new Label();
        lblFxHint = new Label();
        panelFxScroll = new Panel();
        tableFx = new TableLayoutPanel();
        panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).BeginInit();
        panelChoco.SuspendLayout();
        panelMatribox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMatribox).BeginInit();
        splitMatribox.Panel1.SuspendLayout();
        splitMatribox.Panel2.SuspendLayout();
        splitMatribox.SuspendLayout();
        panelBancos.SuspendLayout();
        panelFx.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.Controls.Add(numChannel);
        panelTop.Controls.Add(lblChannel);
        panelTop.Controls.Add(btnWrite);
        panelTop.Controls.Add(btnRead);
        panelTop.Controls.Add(btnConnect);
        panelTop.Controls.Add(cmbChocolateMidi);
        panelTop.Controls.Add(lblTitle);
        panelTop.Controls.Add(btnInicio);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(16, 12, 16, 12);
        panelTop.Size = new Size(1100, 64);
        panelTop.TabIndex = 0;
        // 
        // btnInicio
        // 
        btnInicio.FlatStyle = FlatStyle.Flat;
        btnInicio.Location = new Point(16, 16);
        btnInicio.Name = "btnInicio";
        btnInicio.Size = new Size(80, 32);
        btnInicio.TabIndex = 0;
        btnInicio.Text = "← Início";
        btnInicio.Click += btnInicio_Click;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.Location = new Point(108, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(220, 25);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Chocolate × Matribox";
        // 
        // cmbChocolateMidi
        // 
        cmbChocolateMidi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cmbChocolateMidi.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbChocolateMidi.Location = new Point(348, 20);
        cmbChocolateMidi.Name = "cmbChocolateMidi";
        cmbChocolateMidi.Size = new Size(176, 23);
        cmbChocolateMidi.TabIndex = 2;
        // 
        // btnConnect
        // 
        btnConnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.Location = new Point(532, 16);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(100, 32);
        btnConnect.TabIndex = 3;
        btnConnect.Text = "Conectar";
        btnConnect.Click += btnConnect_Click;
        // 
        // btnRead
        // 
        btnRead.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRead.FlatStyle = FlatStyle.Flat;
        btnRead.Location = new Point(638, 16);
        btnRead.Name = "btnRead";
        btnRead.Size = new Size(100, 32);
        btnRead.TabIndex = 4;
        btnRead.Text = "Ler";
        btnRead.Click += btnRead_Click;
        // 
        // btnWrite
        // 
        btnWrite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnWrite.FlatStyle = FlatStyle.Flat;
        btnWrite.Location = new Point(744, 16);
        btnWrite.Name = "btnWrite";
        btnWrite.Size = new Size(148, 32);
        btnWrite.TabIndex = 5;
        btnWrite.Text = "Gravar no Chocolate";
        btnWrite.Click += btnWrite_Click;
        // 
        // lblChannel
        // 
        lblChannel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblChannel.AutoSize = true;
        lblChannel.Location = new Point(936, 22);
        lblChannel.Name = "lblChannel";
        lblChannel.Size = new Size(26, 15);
        lblChannel.TabIndex = 6;
        lblChannel.Text = "Ch";
        // 
        // numChannel
        // 
        numChannel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        numChannel.Location = new Point(968, 20);
        numChannel.Maximum = 16;
        numChannel.Minimum = 1;
        numChannel.Name = "numChannel";
        numChannel.Size = new Size(48, 23);
        numChannel.TabIndex = 7;
        numChannel.Value = 1;
        numChannel.ValueChanged += numChannel_ValueChanged;
        // 
        // panelChoco
        // 
        panelChoco.Controls.Add(chkStomp);
        panelChoco.Controls.Add(lblChocoHint);
        panelChoco.Controls.Add(btnSwD);
        panelChoco.Controls.Add(btnSwC);
        panelChoco.Controls.Add(btnSwB);
        panelChoco.Controls.Add(btnSwA);
        panelChoco.Controls.Add(cmbChocoBank);
        panelChoco.Controls.Add(lblBanco);
        panelChoco.Controls.Add(tabChoco);
        panelChoco.Dock = DockStyle.Top;
        panelChoco.Location = new Point(0, 64);
        panelChoco.Name = "panelChoco";
        panelChoco.Padding = new Padding(20, 16, 20, 12);
        panelChoco.Size = new Size(1100, 168);
        panelChoco.TabIndex = 1;
        // 
        // tabChoco
        // 
        tabChoco.AutoSize = true;
        tabChoco.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        tabChoco.Location = new Point(24, 8);
        tabChoco.Name = "tabChoco";
        tabChoco.Padding = new Padding(10, 4, 10, 4);
        tabChoco.Size = new Size(86, 25);
        tabChoco.TabIndex = 0;
        tabChoco.Text = "Chocolate";
        // 
        // lblBanco
        // 
        lblBanco.AutoSize = true;
        lblBanco.Location = new Point(24, 48);
        lblBanco.Name = "lblBanco";
        lblBanco.Size = new Size(42, 15);
        lblBanco.TabIndex = 1;
        lblBanco.Text = "Banco";
        // 
        // cmbChocoBank
        // 
        cmbChocoBank.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbChocoBank.Location = new Point(80, 44);
        cmbChocoBank.Name = "cmbChocoBank";
        cmbChocoBank.Size = new Size(220, 23);
        cmbChocoBank.TabIndex = 2;
        cmbChocoBank.SelectedIndexChanged += cmbChocoBank_SelectedIndexChanged;
        // 
        // btnSwA
        // 
        btnSwA.FlatStyle = FlatStyle.Flat;
        btnSwA.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwA.Location = new Point(24, 84);
        btnSwA.Name = "btnSwA";
        btnSwA.Size = new Size(72, 40);
        btnSwA.TabIndex = 3;
        btnSwA.Text = "A";
        btnSwA.Click += btnSwitch_Click;
        // 
        // btnSwB
        // 
        btnSwB.FlatStyle = FlatStyle.Flat;
        btnSwB.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwB.Location = new Point(100, 84);
        btnSwB.Name = "btnSwB";
        btnSwB.Size = new Size(72, 40);
        btnSwB.TabIndex = 4;
        btnSwB.Text = "B";
        btnSwB.Click += btnSwitch_Click;
        // 
        // btnSwC
        // 
        btnSwC.FlatStyle = FlatStyle.Flat;
        btnSwC.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwC.Location = new Point(176, 84);
        btnSwC.Name = "btnSwC";
        btnSwC.Size = new Size(72, 40);
        btnSwC.TabIndex = 5;
        btnSwC.Text = "C";
        btnSwC.Click += btnSwitch_Click;
        // 
        // btnSwD
        // 
        btnSwD.FlatStyle = FlatStyle.Flat;
        btnSwD.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwD.Location = new Point(252, 84);
        btnSwD.Name = "btnSwD";
        btnSwD.Size = new Size(72, 40);
        btnSwD.TabIndex = 6;
        btnSwD.Text = "D";
        btnSwD.Click += btnSwitch_Click;
        // 
        // lblChocoHint
        // 
        lblChocoHint.AutoSize = true;
        lblChocoHint.Location = new Point(340, 96);
        lblChocoHint.Name = "lblChocoHint";
        lblChocoHint.Size = new Size(400, 15);
        lblChocoHint.TabIndex = 7;
        lblChocoHint.Text = "A · curto manda efeitos; A · longo escolhe o patch. B/C/D · curto liga/desliga efeitos.";
        // 
        // chkStomp
        // 
        chkStomp.AutoSize = true;
        chkStomp.Location = new Point(340, 48);
        chkStomp.Name = "chkStomp";
        chkStomp.Size = new Size(280, 19);
        chkStomp.TabIndex = 8;
        chkStomp.Text = "Stomp Mode (CC 29) no toque curto do A";
        chkStomp.CheckedChanged += chkStomp_CheckedChanged;
        // 
        // panelMatribox
        // 
        panelMatribox.Controls.Add(splitMatribox);
        panelMatribox.Controls.Add(tabMatribox);
        panelMatribox.Dock = DockStyle.Fill;
        panelMatribox.Location = new Point(0, 232);
        panelMatribox.Name = "panelMatribox";
        panelMatribox.Padding = new Padding(16, 28, 16, 16);
        panelMatribox.Size = new Size(1100, 568);
        panelMatribox.TabIndex = 2;
        // 
        // tabMatribox
        // 
        tabMatribox.AutoSize = true;
        tabMatribox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        tabMatribox.Location = new Point(24, 8);
        tabMatribox.Name = "tabMatribox";
        tabMatribox.Padding = new Padding(10, 4, 10, 4);
        tabMatribox.Size = new Size(122, 25);
        tabMatribox.TabIndex = 0;
        tabMatribox.Text = "Matribox II Pro";
        // 
        // splitMatribox
        // 
        splitMatribox.Dock = DockStyle.Fill;
        splitMatribox.Location = new Point(16, 28);
        splitMatribox.Name = "splitMatribox";
        // 
        // splitMatribox.Panel1
        // 
        splitMatribox.Panel1.Controls.Add(panelBancos);
        splitMatribox.Panel1MinSize = 280;
        // 
        // splitMatribox.Panel2
        // 
        splitMatribox.Panel2.Controls.Add(panelFx);
        splitMatribox.Panel2MinSize = 280;
        splitMatribox.Size = new Size(1068, 524);
        splitMatribox.SplitterDistance = 420;
        splitMatribox.TabIndex = 1;
        // 
        // panelBancos
        // 
        panelBancos.Controls.Add(lstPatches);
        panelBancos.Controls.Add(tabBancos);
        panelBancos.Dock = DockStyle.Fill;
        panelBancos.Location = new Point(0, 0);
        panelBancos.Name = "panelBancos";
        panelBancos.Padding = new Padding(8, 28, 8, 8);
        panelBancos.Size = new Size(420, 524);
        panelBancos.TabIndex = 0;
        // 
        // tabBancos
        // 
        tabBancos.AutoSize = true;
        tabBancos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        tabBancos.Location = new Point(12, 6);
        tabBancos.Name = "tabBancos";
        tabBancos.Padding = new Padding(10, 4, 10, 4);
        tabBancos.Size = new Size(64, 25);
        tabBancos.TabIndex = 0;
        tabBancos.Text = "Bancos";
        // 
        // lstPatches
        // 
        lstPatches.BorderStyle = BorderStyle.None;
        lstPatches.Dock = DockStyle.Fill;
        lstPatches.Font = new Font("Segoe UI", 10F);
        lstPatches.IntegralHeight = false;
        lstPatches.ItemHeight = 22;
        lstPatches.Location = new Point(8, 28);
        lstPatches.Name = "lstPatches";
        lstPatches.Size = new Size(404, 488);
        lstPatches.TabIndex = 1;
        lstPatches.SelectedIndexChanged += lstPatches_SelectedIndexChanged;
        // 
        // panelFx
        // 
        panelFx.Controls.Add(panelFxScroll);
        panelFx.Controls.Add(lblFxHint);
        panelFx.Controls.Add(tabFx);
        panelFx.Dock = DockStyle.Fill;
        panelFx.Location = new Point(0, 0);
        panelFx.Name = "panelFx";
        panelFx.Padding = new Padding(8, 28, 8, 8);
        panelFx.Size = new Size(644, 524);
        panelFx.TabIndex = 0;
        // 
        // tabFx
        // 
        tabFx.AutoSize = true;
        tabFx.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        tabFx.Location = new Point(12, 6);
        tabFx.Name = "tabFx";
        tabFx.Padding = new Padding(10, 4, 10, 4);
        tabFx.Size = new Size(64, 25);
        tabFx.TabIndex = 0;
        tabFx.Text = "Efeitos";
        // 
        // lblFxHint
        // 
        lblFxHint.Dock = DockStyle.Top;
        lblFxHint.Location = new Point(8, 28);
        lblFxHint.Name = "lblFxHint";
        lblFxHint.Size = new Size(628, 36);
        lblFxHint.TabIndex = 1;
        lblFxHint.Text = "Estados que este switch manda no toque curto (CC 43–54).";
        // 
        // panelFxScroll
        // 
        panelFxScroll.AutoScroll = true;
        panelFxScroll.Controls.Add(tableFx);
        panelFxScroll.Dock = DockStyle.Fill;
        panelFxScroll.Location = new Point(8, 64);
        panelFxScroll.Name = "panelFxScroll";
        panelFxScroll.Size = new Size(628, 452);
        panelFxScroll.TabIndex = 2;
        // 
        // tableFx
        // 
        tableFx.ColumnCount = 2;
        tableFx.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        tableFx.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        tableFx.Location = new Point(0, 0);
        tableFx.Name = "tableFx";
        tableFx.Size = new Size(600, 336);
        tableFx.TabIndex = 0;
        // 
        // FormChocolateMatribox
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 800);
        Controls.Add(panelMatribox);
        Controls.Add(panelChoco);
        Controls.Add(panelTop);
        MinimumSize = new Size(1000, 700);
        Name = "FormChocolateMatribox";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chocolate × Matribox";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).EndInit();
        panelChoco.ResumeLayout(false);
        panelChoco.PerformLayout();
        panelMatribox.ResumeLayout(false);
        panelMatribox.PerformLayout();
        splitMatribox.Panel1.ResumeLayout(false);
        splitMatribox.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMatribox).EndInit();
        splitMatribox.ResumeLayout(false);
        panelBancos.ResumeLayout(false);
        panelBancos.PerformLayout();
        panelFx.ResumeLayout(false);
        panelFx.PerformLayout();
        ResumeLayout(false);
    }

    private Panel panelTop;
    private Button btnInicio;
    private Label lblTitle;
    private ComboBox cmbChocolateMidi;
    private Button btnConnect;
    private Button btnRead;
    private Button btnWrite;
    private Label lblChannel;
    private NumericUpDown numChannel;
    private Panel panelChoco;
    private Label tabChoco;
    private Label lblBanco;
    private ComboBox cmbChocoBank;
    private Button btnSwA;
    private Button btnSwB;
    private Button btnSwC;
    private Button btnSwD;
    private Label lblChocoHint;
    private CheckBox chkStomp;
    private Panel panelMatribox;
    private Label tabMatribox;
    private SplitContainer splitMatribox;
    private Panel panelBancos;
    private Label tabBancos;
    private ListBox lstPatches;
    private Panel panelFx;
    private Label tabFx;
    private Label lblFxHint;
    private Panel panelFxScroll;
    private TableLayoutPanel tableFx;
}
