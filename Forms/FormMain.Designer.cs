using ChocolateEditor.Controls;

namespace ChocolateEditor.Forms;

partial class FormMain
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
        menuStrip = new MenuStrip();
        mnuArquivo = new ToolStripMenuItem();
        mnuNovo = new ToolStripMenuItem();
        mnuAbrir = new ToolStripMenuItem();
        mnuSalvar = new ToolStripMenuItem();
        mnuSalvarComo = new ToolStripMenuItem();
        mnuSepArquivo = new ToolStripSeparator();
        mnuSair = new ToolStripMenuItem();
        mnuDispositivo = new ToolStripMenuItem();
        mnuAtualizar = new ToolStripMenuItem();
        mnuConectar = new ToolStripMenuItem();
        mnuLer = new ToolStripMenuItem();
        mnuGravar = new ToolStripMenuItem();
        mnuBancos = new ToolStripMenuItem();
        mnuCopiarBanco = new ToolStripMenuItem();
        panelDevice = new Panel();
        lblMidiHint = new Label();
        numBankCount = new NumericUpDown();
        lblBankCount = new Label();
        cmbTrs = new ComboBox();
        lblTrs = new Label();
        cmbMode = new ComboBox();
        lblMode = new Label();
        btnWrite = new Button();
        btnRead = new Button();
        btnConnect = new Button();
        btnRefreshMidi = new Button();
        cmbMidiDevice = new ComboBox();
        lblDevice = new Label();
        splitContainer = new SplitContainer();
        lstBanks = new ListBox();
        lblBanks = new Label();
        tableSwitches = new TableLayoutPanel();
        switchA = new SwitchPanel();
        switchB = new SwitchPanel();
        switchC = new SwitchPanel();
        switchD = new SwitchPanel();
        panelBank = new Panel();
        btnCopyBank = new Button();
        txtBankName = new TextBox();
        lblBankName = new Label();
        lblBankTitle = new Label();
        txtLog = new TextBox();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        menuStrip.SuspendLayout();
        panelDevice.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numBankCount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        tableSwitches.SuspendLayout();
        panelBank.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { mnuArquivo, mnuDispositivo, mnuBancos });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1264, 24);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip";
        // 
        // mnuArquivo
        // 
        mnuArquivo.DropDownItems.AddRange(new ToolStripItem[] { mnuNovo, mnuAbrir, mnuSalvar, mnuSalvarComo, mnuSepArquivo, mnuSair });
        mnuArquivo.Name = "mnuArquivo";
        mnuArquivo.Size = new Size(61, 20);
        mnuArquivo.Text = "&Arquivo";
        // 
        // mnuNovo
        // 
        mnuNovo.Name = "mnuNovo";
        mnuNovo.ShortcutKeys = Keys.Control | Keys.N;
        mnuNovo.Size = new Size(207, 22);
        mnuNovo.Text = "&Novo do modelo";
        mnuNovo.Click += mnuNovo_Click;
        // 
        // mnuAbrir
        // 
        mnuAbrir.Name = "mnuAbrir";
        mnuAbrir.ShortcutKeys = Keys.Control | Keys.O;
        mnuAbrir.Size = new Size(207, 22);
        mnuAbrir.Text = "&Abrir…";
        mnuAbrir.Click += mnuAbrir_Click;
        // 
        // mnuSalvar
        // 
        mnuSalvar.Name = "mnuSalvar";
        mnuSalvar.ShortcutKeys = Keys.Control | Keys.S;
        mnuSalvar.Size = new Size(207, 22);
        mnuSalvar.Text = "&Salvar";
        mnuSalvar.Click += mnuSalvar_Click;
        // 
        // mnuSalvarComo
        // 
        mnuSalvarComo.Name = "mnuSalvarComo";
        mnuSalvarComo.Size = new Size(207, 22);
        mnuSalvarComo.Text = "Salvar &como…";
        mnuSalvarComo.Click += mnuSalvarComo_Click;
        // 
        // mnuSepArquivo
        // 
        mnuSepArquivo.Name = "mnuSepArquivo";
        mnuSepArquivo.Size = new Size(204, 6);
        // 
        // mnuSair
        // 
        mnuSair.Name = "mnuSair";
        mnuSair.Size = new Size(207, 22);
        mnuSair.Text = "Sair";
        mnuSair.Click += mnuSair_Click;
        // 
        // mnuDispositivo
        // 
        mnuDispositivo.DropDownItems.AddRange(new ToolStripItem[] { mnuAtualizar, mnuConectar, mnuLer, mnuGravar });
        mnuDispositivo.Name = "mnuDispositivo";
        mnuDispositivo.Size = new Size(77, 20);
        mnuDispositivo.Text = "&Dispositivo";
        // 
        // mnuAtualizar
        // 
        mnuAtualizar.Name = "mnuAtualizar";
        mnuAtualizar.Size = new Size(182, 22);
        mnuAtualizar.Text = "Atualizar lista MIDI";
        mnuAtualizar.Click += btnRefreshMidi_Click;
        // 
        // mnuConectar
        // 
        mnuConectar.Name = "mnuConectar";
        mnuConectar.Size = new Size(182, 22);
        mnuConectar.Text = "Conectar USB";
        mnuConectar.Click += btnConnect_Click;
        // 
        // mnuLer
        // 
        mnuLer.Name = "mnuLer";
        mnuLer.Size = new Size(182, 22);
        mnuLer.Text = "Ler do Chocolate";
        mnuLer.Click += btnRead_Click;
        // 
        // mnuGravar
        // 
        mnuGravar.Name = "mnuGravar";
        mnuGravar.Size = new Size(182, 22);
        mnuGravar.Text = "Gravar no Chocolate";
        mnuGravar.Click += btnWrite_Click;
        // 
        // mnuBancos
        // 
        mnuBancos.DropDownItems.AddRange(new ToolStripItem[] { mnuCopiarBanco });
        mnuBancos.Name = "mnuBancos";
        mnuBancos.Size = new Size(57, 20);
        mnuBancos.Text = "&Bancos";
        // 
        // mnuCopiarBanco
        // 
        mnuCopiarBanco.Name = "mnuCopiarBanco";
        mnuCopiarBanco.ShortcutKeys = Keys.Control | Keys.D;
        mnuCopiarBanco.Size = new Size(196, 22);
        mnuCopiarBanco.Text = "&Copiar banco…";
        mnuCopiarBanco.Click += mnuCopiarBanco_Click;
        // 
        // panelDevice
        // 
        panelDevice.Controls.Add(lblMidiHint);
        panelDevice.Controls.Add(numBankCount);
        panelDevice.Controls.Add(lblBankCount);
        panelDevice.Controls.Add(cmbTrs);
        panelDevice.Controls.Add(lblTrs);
        panelDevice.Controls.Add(cmbMode);
        panelDevice.Controls.Add(lblMode);
        panelDevice.Controls.Add(btnWrite);
        panelDevice.Controls.Add(btnRead);
        panelDevice.Controls.Add(btnConnect);
        panelDevice.Controls.Add(btnRefreshMidi);
        panelDevice.Controls.Add(cmbMidiDevice);
        panelDevice.Controls.Add(lblDevice);
        panelDevice.Dock = DockStyle.Top;
        panelDevice.Location = new Point(0, 24);
        panelDevice.Name = "panelDevice";
        panelDevice.Size = new Size(1264, 72);
        panelDevice.TabIndex = 1;
        // 
        // lblMidiHint
        // 
        lblMidiHint.AutoSize = true;
        lblMidiHint.ForeColor = SystemColors.GrayText;
        lblMidiHint.Location = new Point(780, 12);
        lblMidiHint.Name = "lblMidiHint";
        lblMidiHint.Size = new Size(144, 15);
        lblMidiHint.TabIndex = 12;
        lblMidiHint.Text = "Switch U + USB-C (dados)";
        // 
        // numBankCount
        // 
        numBankCount.Location = new Point(648, 40);
        numBankCount.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
        numBankCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numBankCount.Name = "numBankCount";
        numBankCount.Size = new Size(60, 23);
        numBankCount.TabIndex = 11;
        numBankCount.Value = new decimal(new int[] { 16, 0, 0, 0 });
        numBankCount.ValueChanged += ControlChanged;
        // 
        // lblBankCount
        // 
        lblBankCount.AutoSize = true;
        lblBankCount.Location = new Point(552, 44);
        lblBankCount.Name = "lblBankCount";
        lblBankCount.Size = new Size(87, 15);
        lblBankCount.TabIndex = 10;
        lblBankCount.Text = "Qtd. de bancos";
        // 
        // cmbTrs
        // 
        cmbTrs.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTrs.FormattingEnabled = true;
        cmbTrs.Items.AddRange(new object[] { "Expression", "MIDI" });
        cmbTrs.Location = new Point(430, 40);
        cmbTrs.Name = "cmbTrs";
        cmbTrs.Size = new Size(110, 23);
        cmbTrs.TabIndex = 9;
        cmbTrs.SelectedIndexChanged += ControlChanged;
        // 
        // lblTrs
        // 
        lblTrs.AutoSize = true;
        lblTrs.Location = new Point(398, 44);
        lblTrs.Name = "lblTrs";
        lblTrs.Size = new Size(26, 15);
        lblTrs.TabIndex = 8;
        lblTrs.Text = "TRS";
        // 
        // cmbMode
        // 
        cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMode.FormattingEnabled = true;
        cmbMode.Location = new Point(80, 40);
        cmbMode.Name = "cmbMode";
        cmbMode.Size = new Size(306, 23);
        cmbMode.TabIndex = 7;
        cmbMode.SelectedIndexChanged += ControlChanged;
        // 
        // lblMode
        // 
        lblMode.AutoSize = true;
        lblMode.Location = new Point(12, 44);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(39, 15);
        lblMode.TabIndex = 6;
        lblMode.Text = "Modo";
        // 
        // btnWrite
        // 
        btnWrite.Location = new Point(634, 7);
        btnWrite.Name = "btnWrite";
        btnWrite.Size = new Size(140, 25);
        btnWrite.TabIndex = 5;
        btnWrite.Text = "Gravar no Chocolate";
        btnWrite.UseVisualStyleBackColor = true;
        btnWrite.Click += btnWrite_Click;
        // 
        // btnRead
        // 
        btnRead.Location = new Point(498, 7);
        btnRead.Name = "btnRead";
        btnRead.Size = new Size(130, 25);
        btnRead.TabIndex = 4;
        btnRead.Text = "Ler do Chocolate";
        btnRead.UseVisualStyleBackColor = true;
        btnRead.Click += btnRead_Click;
        // 
        // btnConnect
        // 
        btnConnect.Location = new Point(392, 7);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(100, 25);
        btnConnect.TabIndex = 3;
        btnConnect.Text = "Conectar";
        btnConnect.UseVisualStyleBackColor = true;
        btnConnect.Click += btnConnect_Click;
        // 
        // btnRefreshMidi
        // 
        btnRefreshMidi.Location = new Point(306, 7);
        btnRefreshMidi.Name = "btnRefreshMidi";
        btnRefreshMidi.Size = new Size(80, 25);
        btnRefreshMidi.TabIndex = 2;
        btnRefreshMidi.Text = "Atualizar";
        btnRefreshMidi.UseVisualStyleBackColor = true;
        btnRefreshMidi.Click += btnRefreshMidi_Click;
        // 
        // cmbMidiDevice
        // 
        cmbMidiDevice.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMidiDevice.FormattingEnabled = true;
        cmbMidiDevice.Location = new Point(80, 8);
        cmbMidiDevice.Name = "cmbMidiDevice";
        cmbMidiDevice.Size = new Size(220, 23);
        cmbMidiDevice.TabIndex = 1;
        // 
        // lblDevice
        // 
        lblDevice.AutoSize = true;
        lblDevice.Location = new Point(12, 12);
        lblDevice.Name = "lblDevice";
        lblDevice.Size = new Size(56, 15);
        lblDevice.TabIndex = 0;
        lblDevice.Text = "MIDI USB";
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 96);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(lstBanks);
        splitContainer.Panel1.Controls.Add(lblBanks);
        splitContainer.Panel1MinSize = 180;
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(tableSwitches);
        splitContainer.Panel2.Controls.Add(panelBank);
        splitContainer.Size = new Size(1264, 562);
        splitContainer.SplitterDistance = 230;
        splitContainer.TabIndex = 2;
        // 
        // lstBanks
        // 
        lstBanks.Dock = DockStyle.Fill;
        lstBanks.FormattingEnabled = true;
        lstBanks.Location = new Point(0, 28);
        lstBanks.Name = "lstBanks";
        lstBanks.Size = new Size(230, 534);
        lstBanks.TabIndex = 1;
        lstBanks.SelectedIndexChanged += lstBanks_SelectedIndexChanged;
        // 
        // lblBanks
        // 
        lblBanks.Dock = DockStyle.Top;
        lblBanks.Location = new Point(0, 0);
        lblBanks.Name = "lblBanks";
        lblBanks.Padding = new Padding(8, 8, 8, 0);
        lblBanks.Size = new Size(230, 28);
        lblBanks.TabIndex = 0;
        lblBanks.Text = "Grupos (E/F no pedal)";
        // 
        // tableSwitches
        // 
        tableSwitches.ColumnCount = 2;
        tableSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableSwitches.Controls.Add(switchA, 0, 0);
        tableSwitches.Controls.Add(switchB, 1, 0);
        tableSwitches.Controls.Add(switchC, 0, 1);
        tableSwitches.Controls.Add(switchD, 1, 1);
        tableSwitches.Dock = DockStyle.Fill;
        tableSwitches.Location = new Point(0, 48);
        tableSwitches.Name = "tableSwitches";
        tableSwitches.Padding = new Padding(4);
        tableSwitches.RowCount = 2;
        tableSwitches.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableSwitches.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableSwitches.Size = new Size(1030, 514);
        tableSwitches.TabIndex = 1;
        // 
        // switchA
        // 
        switchA.Dock = DockStyle.Fill;
        switchA.Location = new Point(7, 7);
        switchA.Name = "switchA";
        switchA.Size = new Size(505, 247);
        switchA.TabIndex = 0;
        switchA.Changed += ControlChanged;
        // 
        // switchB
        // 
        switchB.Dock = DockStyle.Fill;
        switchB.Location = new Point(518, 7);
        switchB.Name = "switchB";
        switchB.Size = new Size(505, 247);
        switchB.SwitchTitle = "Switch B";
        switchB.TabIndex = 1;
        switchB.Changed += ControlChanged;
        // 
        // switchC
        // 
        switchC.Dock = DockStyle.Fill;
        switchC.Location = new Point(7, 260);
        switchC.Name = "switchC";
        switchC.Size = new Size(505, 247);
        switchC.SwitchTitle = "Switch C";
        switchC.TabIndex = 2;
        switchC.Changed += ControlChanged;
        // 
        // switchD
        // 
        switchD.Dock = DockStyle.Fill;
        switchD.Location = new Point(518, 260);
        switchD.Name = "switchD";
        switchD.Size = new Size(505, 247);
        switchD.SwitchTitle = "Switch D";
        switchD.TabIndex = 3;
        switchD.Changed += ControlChanged;
        // 
        // panelBank
        // 
        panelBank.Controls.Add(btnCopyBank);
        panelBank.Controls.Add(txtBankName);
        panelBank.Controls.Add(lblBankName);
        panelBank.Controls.Add(lblBankTitle);
        panelBank.Dock = DockStyle.Top;
        panelBank.Location = new Point(0, 0);
        panelBank.Name = "panelBank";
        panelBank.Size = new Size(1030, 48);
        panelBank.TabIndex = 0;
        // 
        // btnCopyBank
        // 
        btnCopyBank.Location = new Point(540, 11);
        btnCopyBank.Name = "btnCopyBank";
        btnCopyBank.Size = new Size(160, 25);
        btnCopyBank.TabIndex = 3;
        btnCopyBank.Text = "Copiar este banco…";
        btnCopyBank.UseVisualStyleBackColor = true;
        btnCopyBank.Click += mnuCopiarBanco_Click;
        // 
        // txtBankName
        // 
        txtBankName.Location = new Point(326, 12);
        txtBankName.Name = "txtBankName";
        txtBankName.Size = new Size(200, 23);
        txtBankName.TabIndex = 2;
        txtBankName.TextChanged += txtBankName_TextChanged;
        // 
        // lblBankName
        // 
        lblBankName.AutoSize = true;
        lblBankName.Location = new Point(280, 16);
        lblBankName.Name = "lblBankName";
        lblBankName.Size = new Size(40, 15);
        lblBankName.TabIndex = 1;
        lblBankName.Text = "Nome";
        // 
        // lblBankTitle
        // 
        lblBankTitle.AutoSize = true;
        lblBankTitle.Location = new Point(8, 16);
        lblBankTitle.Name = "lblBankTitle";
        lblBankTitle.Size = new Size(80, 15);
        lblBankTitle.TabIndex = 0;
        lblBankTitle.Text = "Banco 1 de 16";
        // 
        // txtLog
        // 
        txtLog.BackColor = Color.FromArgb(40, 32, 28);
        txtLog.Dock = DockStyle.Bottom;
        txtLog.Font = new Font("Consolas", 8.25F);
        txtLog.ForeColor = Color.FromArgb(220, 200, 170);
        txtLog.Location = new Point(0, 658);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(1264, 100);
        txtLog.TabIndex = 3;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Location = new Point(0, 758);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1264, 22);
        statusStrip.TabIndex = 4;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(108, 17);
        lblStatus.Text = "USB: desconectado";
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1264, 780);
        Controls.Add(splitContainer);
        Controls.Add(txtLog);
        Controls.Add(panelDevice);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(1100, 700);
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chocolate Plus Editor";
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        panelDevice.ResumeLayout(false);
        panelDevice.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numBankCount).EndInit();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        tableSwitches.ResumeLayout(false);
        panelBank.ResumeLayout(false);
        panelBank.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip;
    private ToolStripMenuItem mnuArquivo;
    private ToolStripMenuItem mnuNovo;
    private ToolStripMenuItem mnuAbrir;
    private ToolStripMenuItem mnuSalvar;
    private ToolStripMenuItem mnuSalvarComo;
    private ToolStripSeparator mnuSepArquivo;
    private ToolStripMenuItem mnuSair;
    private ToolStripMenuItem mnuDispositivo;
    private ToolStripMenuItem mnuAtualizar;
    private ToolStripMenuItem mnuConectar;
    private ToolStripMenuItem mnuLer;
    private ToolStripMenuItem mnuGravar;
    private ToolStripMenuItem mnuBancos;
    private ToolStripMenuItem mnuCopiarBanco;
    private Panel panelDevice;
    private Label lblDevice;
    private ComboBox cmbMidiDevice;
    private Button btnRefreshMidi;
    private Button btnConnect;
    private Button btnRead;
    private Button btnWrite;
    private Label lblMode;
    private ComboBox cmbMode;
    private Label lblTrs;
    private ComboBox cmbTrs;
    private Label lblBankCount;
    private NumericUpDown numBankCount;
    private Label lblMidiHint;
    private SplitContainer splitContainer;
    private ListBox lstBanks;
    private Label lblBanks;
    private Panel panelBank;
    private Label lblBankTitle;
    private Label lblBankName;
    private TextBox txtBankName;
    private Button btnCopyBank;
    private TableLayoutPanel tableSwitches;
    private SwitchPanel switchA;
    private SwitchPanel switchB;
    private SwitchPanel switchC;
    private SwitchPanel switchD;
    private TextBox txtLog;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
}
