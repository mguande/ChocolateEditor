using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

namespace ChocolateEditor.Forms;

partial class FormMatribox
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMatribox));
        panelScan = new Panel();
        progressBarScan = new ProgressBar();
        lblScan = new Label();
        panelTop = new Panel();
        lblMatriboxHint = new Label();
        numChannel = new NumericUpDown();
        lblChannel = new Label();
        btnRefresh = new ForeverButton();
        btnConnectChocolate = new ForeverButton();
        cmbChocolateMidi = new ComboBox();
        lblChocolateMidi = new Label();
        btnConnectMatribox = new ForeverButton();
        cmbMatriboxMidi = new ComboBox();
        lblMatriboxMidi = new Label();
        btnInicio = new ForeverButton();
        splitMain = new SplitContainer();
        splitLeft = new SplitContainer();
        lstChoco = new ListBox();
        cmbSwitch = new ComboBox();
        lblSwitch = new Label();
        lblChoco = new Label();
        treePresets = new TreeView();
        lblTree = new Label();
        panelRight = new Panel();
        panelFxScroll = new Panel();
        tableFx = new TableLayoutPanel();
        txtPreview = new TextBox();
        panelBottom = new Panel();
        txtLog = new TextBox();
        panelBtnRow2 = new Panel();
        btnWriteChocolate = new ForeverButton();
        btnReadChocolate = new ForeverButton();
        btnTestMatribox = new ForeverButton();
        panelBtnRow1 = new Panel();
        btnMapBank = new ForeverButton();
        btnApply = new ForeverButton();
        panelFxTop = new Panel();
        btnImportPrst = new ForeverButton();
        btnScanMatribox = new ForeverButton();
        btnAllOff = new ForeverButton();
        btnAllOn = new ForeverButton();
        chkStomp = new CheckBox();
        lblFx = new Label();
        panelScan.SuspendLayout();
        panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitLeft).BeginInit();
        splitLeft.Panel1.SuspendLayout();
        splitLeft.Panel2.SuspendLayout();
        splitLeft.SuspendLayout();
        panelRight.SuspendLayout();
        panelFxScroll.SuspendLayout();
        panelBottom.SuspendLayout();
        panelBtnRow2.SuspendLayout();
        panelBtnRow1.SuspendLayout();
        panelFxTop.SuspendLayout();
        SuspendLayout();
        // 
        // panelScan
        // 
        panelScan.Controls.Add(progressBarScan);
        panelScan.Controls.Add(lblScan);
        panelScan.Dock = DockStyle.Top;
        panelScan.Location = new Point(0, 88);
        panelScan.Name = "panelScan";
        panelScan.Padding = new Padding(12, 8, 12, 8);
        panelScan.Size = new Size(1100, 40);
        panelScan.TabIndex = 3;
        panelScan.Visible = false;
        // 
        // lblScan
        // 
        lblScan.Dock = DockStyle.Left;
        lblScan.Location = new Point(12, 8);
        lblScan.Name = "lblScan";
        lblScan.Size = new Size(240, 24);
        lblScan.TabIndex = 0;
        lblScan.Text = "A ler…";
        lblScan.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // progressBarScan
        // 
        progressBarScan.Dock = DockStyle.Fill;
        progressBarScan.Location = new Point(252, 8);
        progressBarScan.Name = "progressBarScan";
        progressBarScan.Size = new Size(836, 24);
        progressBarScan.TabIndex = 1;
        // 
        // panelTop
        // 
        panelTop.Controls.Add(lblMatriboxHint);
        panelTop.Controls.Add(numChannel);
        panelTop.Controls.Add(lblChannel);
        panelTop.Controls.Add(btnRefresh);
        panelTop.Controls.Add(btnConnectChocolate);
        panelTop.Controls.Add(cmbChocolateMidi);
        panelTop.Controls.Add(lblChocolateMidi);
        panelTop.Controls.Add(btnConnectMatribox);
        panelTop.Controls.Add(cmbMatriboxMidi);
        panelTop.Controls.Add(lblMatriboxMidi);
        panelTop.Controls.Add(btnInicio);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(1100, 88);
        panelTop.TabIndex = 0;
        // 
        // lblMatriboxHint
        // 
        lblMatriboxHint.AutoSize = true;
        lblMatriboxHint.Location = new Point(270, 54);
        lblMatriboxHint.Name = "lblMatriboxHint";
        lblMatriboxHint.Size = new Size(247, 15);
        lblMatriboxHint.TabIndex = 0;
        lblMatriboxHint.Text = "Matribox IN = USB. Chocolate = FootCtrlPlus.";
        // 
        // numChannel
        // 
        numChannel.Location = new Point(200, 50);
        numChannel.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
        numChannel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numChannel.Name = "numChannel";
        numChannel.Size = new Size(50, 23);
        numChannel.TabIndex = 6;
        numChannel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numChannel.ValueChanged += numChannel_ValueChanged;
        // 
        // lblChannel
        // 
        lblChannel.AutoSize = true;
        lblChannel.Location = new Point(104, 54);
        lblChannel.Name = "lblChannel";
        lblChannel.Size = new Size(65, 15);
        lblChannel.TabIndex = 7;
        lblChannel.Text = "Canal MIDI";
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(988, 12);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Rounded = true;
        btnRefresh.Size = new Size(90, 32);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "Atualizar";
        btnRefresh.TextColor = Color.White;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnConnectChocolate
        // 
        btnConnectChocolate.Location = new Point(832, 12);
        btnConnectChocolate.Name = "btnConnectChocolate";
        btnConnectChocolate.Rounded = true;
        btnConnectChocolate.Size = new Size(150, 32);
        btnConnectChocolate.TabIndex = 4;
        btnConnectChocolate.Text = "Conectar Chocolate";
        btnConnectChocolate.TextColor = Color.White;
        btnConnectChocolate.Click += btnConnectChocolate_Click;
        // 
        // cmbChocolateMidi
        // 
        cmbChocolateMidi.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbChocolateMidi.Location = new Point(646, 14);
        cmbChocolateMidi.Name = "cmbChocolateMidi";
        cmbChocolateMidi.Size = new Size(180, 23);
        cmbChocolateMidi.TabIndex = 3;
        // 
        // lblChocolateMidi
        // 
        lblChocolateMidi.AutoSize = true;
        lblChocolateMidi.Location = new Point(540, 18);
        lblChocolateMidi.Name = "lblChocolateMidi";
        lblChocolateMidi.Size = new Size(89, 15);
        lblChocolateMidi.TabIndex = 8;
        lblChocolateMidi.Text = "MIDI Chocolate";
        // 
        // btnConnectMatribox
        // 
        btnConnectMatribox.Location = new Point(386, 12);
        btnConnectMatribox.Name = "btnConnectMatribox";
        btnConnectMatribox.Rounded = true;
        btnConnectMatribox.Size = new Size(150, 32);
        btnConnectMatribox.TabIndex = 2;
        btnConnectMatribox.Text = "Conectar Matribox";
        btnConnectMatribox.TextColor = Color.White;
        btnConnectMatribox.Click += btnConnectMatribox_Click;
        // 
        // cmbMatriboxMidi
        // 
        cmbMatriboxMidi.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMatriboxMidi.Location = new Point(200, 14);
        cmbMatriboxMidi.Name = "cmbMatriboxMidi";
        cmbMatriboxMidi.Size = new Size(180, 23);
        cmbMatriboxMidi.TabIndex = 1;
        // 
        // lblMatriboxMidi
        // 
        lblMatriboxMidi.AutoSize = true;
        lblMatriboxMidi.Location = new Point(104, 18);
        lblMatriboxMidi.Name = "lblMatriboxMidi";
        lblMatriboxMidi.Size = new Size(83, 15);
        lblMatriboxMidi.TabIndex = 9;
        lblMatriboxMidi.Text = "MIDI Matribox";
        // 
        // btnInicio
        // 
        btnInicio.Location = new Point(12, 12);
        btnInicio.Name = "btnInicio";
        btnInicio.Rounded = true;
        btnInicio.Size = new Size(88, 32);
        btnInicio.TabIndex = 0;
        btnInicio.Text = "← Início";
        btnInicio.TextColor = Color.White;
        btnInicio.Click += btnInicio_Click;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 88);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(splitLeft);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(panelRight);
        splitMain.Size = new Size(1100, 712);
        splitMain.SplitterDistance = 640;
        splitMain.TabIndex = 1;
        // 
        // splitLeft
        // 
        splitLeft.Dock = DockStyle.Fill;
        splitLeft.Location = new Point(0, 0);
        splitLeft.Name = "splitLeft";
        // 
        // splitLeft.Panel1
        // 
        splitLeft.Panel1.Controls.Add(lstChoco);
        splitLeft.Panel1.Controls.Add(cmbSwitch);
        splitLeft.Panel1.Controls.Add(lblSwitch);
        splitLeft.Panel1.Controls.Add(lblChoco);
        splitLeft.Panel1.Padding = new Padding(8);
        // 
        // splitLeft.Panel2
        // 
        splitLeft.Panel2.Controls.Add(treePresets);
        splitLeft.Panel2.Controls.Add(lblTree);
        splitLeft.Panel2.Padding = new Padding(8);
        splitLeft.Size = new Size(640, 712);
        splitLeft.SplitterDistance = 220;
        splitLeft.TabIndex = 0;
        // 
        // lstChoco
        // 
        lstChoco.Dock = DockStyle.Fill;
        lstChoco.Location = new Point(8, 87);
        lstChoco.Name = "lstChoco";
        lstChoco.Size = new Size(204, 617);
        lstChoco.TabIndex = 1;
        // 
        // cmbSwitch
        // 
        cmbSwitch.Dock = DockStyle.Top;
        cmbSwitch.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSwitch.Location = new Point(8, 64);
        cmbSwitch.Name = "cmbSwitch";
        cmbSwitch.Size = new Size(204, 23);
        cmbSwitch.TabIndex = 0;
        // 
        // lblSwitch
        // 
        lblSwitch.Dock = DockStyle.Top;
        lblSwitch.Location = new Point(8, 44);
        lblSwitch.Name = "lblSwitch";
        lblSwitch.Size = new Size(204, 20);
        lblSwitch.TabIndex = 2;
        lblSwitch.Text = "Switch";
        // 
        // lblChoco
        // 
        lblChoco.Dock = DockStyle.Top;
        lblChoco.Location = new Point(8, 8);
        lblChoco.Name = "lblChoco";
        lblChoco.Size = new Size(204, 36);
        lblChoco.TabIndex = 3;
        lblChoco.Text = "Grupos no Chocolate\n(o que vais gravar no pedal)";
        // 
        // treePresets
        // 
        treePresets.Dock = DockStyle.Fill;
        treePresets.HideSelection = false;
        treePresets.LabelEdit = true;
        treePresets.BeforeLabelEdit += treePresets_BeforeLabelEdit;
        treePresets.AfterLabelEdit += treePresets_AfterLabelEdit;
        treePresets.Location = new Point(8, 44);
        treePresets.Name = "treePresets";
        treePresets.Size = new Size(400, 660);
        treePresets.TabIndex = 0;
        treePresets.AfterSelect += treePresets_AfterSelect;
        // 
        // lblTree
        // 
        lblTree.Dock = DockStyle.Top;
        lblTree.Location = new Point(8, 8);
        lblTree.Name = "lblTree";
        lblTree.Size = new Size(400, 36);
        lblTree.TabIndex = 1;
        lblTree.Text = "Nomes: importar .prst da Sonicake, ou duplo clique.\n60 bancos × A–D = 240 patches.";
        // 
        // panelRight
        // 
        panelRight.Controls.Add(panelFxScroll);
        panelRight.Controls.Add(txtPreview);
        panelRight.Controls.Add(panelBottom);
        panelRight.Controls.Add(panelFxTop);
        panelRight.Dock = DockStyle.Fill;
        panelRight.Location = new Point(0, 0);
        panelRight.Name = "panelRight";
        panelRight.Padding = new Padding(8);
        panelRight.Size = new Size(456, 712);
        panelRight.TabIndex = 0;
        // 
        // panelFxScroll
        // 
        panelFxScroll.AutoScroll = true;
        panelFxScroll.Controls.Add(tableFx);
        panelFxScroll.Dock = DockStyle.Fill;
        panelFxScroll.Location = new Point(8, 104);
        panelFxScroll.Name = "panelFxScroll";
        panelFxScroll.Size = new Size(440, 340);
        panelFxScroll.TabIndex = 1;
        // 
        // tableFx
        // 
        tableFx.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableFx.Dock = DockStyle.Top;
        tableFx.Location = new Point(0, 0);
        tableFx.Name = "tableFx";
        tableFx.Size = new Size(440, 336);
        tableFx.TabIndex = 0;
        // 
        // txtPreview
        // 
        txtPreview.Dock = DockStyle.Bottom;
        txtPreview.Font = new Font("Consolas", 8.25F);
        txtPreview.Location = new Point(8, 444);
        txtPreview.Multiline = true;
        txtPreview.Name = "txtPreview";
        txtPreview.ReadOnly = true;
        txtPreview.ScrollBars = ScrollBars.Vertical;
        txtPreview.Size = new Size(440, 80);
        txtPreview.TabIndex = 2;
        // 
        // panelBottom
        // 
        panelBottom.Controls.Add(txtLog);
        panelBottom.Controls.Add(panelBtnRow2);
        panelBottom.Controls.Add(panelBtnRow1);
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Location = new Point(8, 524);
        panelBottom.Name = "panelBottom";
        panelBottom.Size = new Size(440, 180);
        panelBottom.TabIndex = 3;
        // 
        // txtLog
        // 
        txtLog.BackColor = Color.FromArgb(30, 30, 30);
        txtLog.Dock = DockStyle.Fill;
        txtLog.ForeColor = Color.Gainsboro;
        txtLog.Location = new Point(0, 68);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(440, 112);
        txtLog.TabIndex = 2;
        // 
        // panelBtnRow2
        // 
        panelBtnRow2.Controls.Add(btnWriteChocolate);
        panelBtnRow2.Controls.Add(btnReadChocolate);
        panelBtnRow2.Controls.Add(btnTestMatribox);
        panelBtnRow2.Dock = DockStyle.Top;
        panelBtnRow2.Location = new Point(0, 36);
        panelBtnRow2.Name = "panelBtnRow2";
        panelBtnRow2.Size = new Size(440, 32);
        panelBtnRow2.TabIndex = 1;
        // 
        // btnWriteChocolate
        // 
        btnWriteChocolate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnWriteChocolate.Location = new Point(292, 2);
        btnWriteChocolate.Name = "btnWriteChocolate";
        btnWriteChocolate.Rounded = true;
        btnWriteChocolate.Size = new Size(148, 28);
        btnWriteChocolate.TabIndex = 2;
        btnWriteChocolate.Text = "Gravar no Chocolate";
        btnWriteChocolate.TextColor = Color.White;
        btnWriteChocolate.Click += btnWriteChocolate_Click;
        // 
        // btnReadChocolate
        // 
        btnReadChocolate.Location = new Point(146, 2);
        btnReadChocolate.Name = "btnReadChocolate";
        btnReadChocolate.Rounded = true;
        btnReadChocolate.Size = new Size(140, 28);
        btnReadChocolate.TabIndex = 1;
        btnReadChocolate.Text = "Ler Chocolate";
        btnReadChocolate.TextColor = Color.White;
        btnReadChocolate.Click += btnReadChocolate_Click;
        // 
        // btnTestMatribox
        // 
        btnTestMatribox.Location = new Point(0, 2);
        btnTestMatribox.Name = "btnTestMatribox";
        btnTestMatribox.Rounded = true;
        btnTestMatribox.Size = new Size(140, 28);
        btnTestMatribox.TabIndex = 0;
        btnTestMatribox.Text = "Testar na Matribox";
        btnTestMatribox.TextColor = Color.White;
        btnTestMatribox.Click += btnTestMatribox_Click;
        // 
        // panelBtnRow1
        // 
        panelBtnRow1.Controls.Add(btnMapBank);
        panelBtnRow1.Controls.Add(btnApply);
        panelBtnRow1.Dock = DockStyle.Top;
        panelBtnRow1.Location = new Point(0, 0);
        panelBtnRow1.Name = "panelBtnRow1";
        panelBtnRow1.Size = new Size(440, 36);
        panelBtnRow1.TabIndex = 0;
        // 
        // btnMapBank
        // 
        btnMapBank.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnMapBank.Location = new Point(216, 2);
        btnMapBank.Name = "btnMapBank";
        btnMapBank.Rounded = true;
        btnMapBank.Size = new Size(224, 32);
        btnMapBank.TabIndex = 1;
        btnMapBank.Text = "A–D = patches A–D deste banco";
        btnMapBank.TextColor = Color.White;
        btnMapBank.Click += btnMapBank_Click;
        // 
        // btnApply
        // 
        btnApply.Location = new Point(0, 2);
        btnApply.Name = "btnApply";
        btnApply.Rounded = true;
        btnApply.Size = new Size(210, 32);
        btnApply.TabIndex = 0;
        btnApply.Text = "Aplicar neste switch";
        btnApply.TextColor = Color.White;
        btnApply.Click += btnApply_Click;
        // 
        // panelFxTop
        // 
        panelFxTop.Controls.Add(btnImportPrst);
        panelFxTop.Controls.Add(btnScanMatribox);
        panelFxTop.Controls.Add(btnAllOff);
        panelFxTop.Controls.Add(btnAllOn);
        panelFxTop.Controls.Add(chkStomp);
        panelFxTop.Controls.Add(lblFx);
        panelFxTop.Dock = DockStyle.Top;
        panelFxTop.Location = new Point(8, 8);
        panelFxTop.Name = "panelFxTop";
        panelFxTop.Size = new Size(440, 128);
        panelFxTop.TabIndex = 0;
        // 
        // btnImportPrst
        // 
        btnImportPrst.Location = new Point(0, 94);
        btnImportPrst.Name = "btnImportPrst";
        btnImportPrst.Rounded = true;
        btnImportPrst.Size = new Size(440, 28);
        btnImportPrst.TabIndex = 5;
        btnImportPrst.Text = "Importar .prst da Sonicake…";
        btnImportPrst.TextColor = Color.White;
        btnImportPrst.Click += btnImportPrst_Click;
        // 
        // btnScanMatribox
        // 
        btnScanMatribox.Location = new Point(212, 64);
        btnScanMatribox.Name = "btnScanMatribox";
        btnScanMatribox.Rounded = true;
        btnScanMatribox.Size = new Size(228, 26);
        btnScanMatribox.TabIndex = 4;
        btnScanMatribox.Text = "Testar MIDI (240 patches)";
        btnScanMatribox.TextColor = Color.White;
        btnScanMatribox.Click += btnScanMatribox_Click;
        // 
        // btnAllOff
        // 
        btnAllOff.Location = new Point(106, 64);
        btnAllOff.Name = "btnAllOff";
        btnAllOff.Rounded = true;
        btnAllOff.Size = new Size(100, 26);
        btnAllOff.TabIndex = 2;
        btnAllOff.Text = "Todos Off";
        btnAllOff.TextColor = Color.White;
        btnAllOff.Click += btnAllOff_Click;
        // 
        // btnAllOn
        // 
        btnAllOn.Location = new Point(0, 64);
        btnAllOn.Name = "btnAllOn";
        btnAllOn.Rounded = true;
        btnAllOn.Size = new Size(100, 26);
        btnAllOn.TabIndex = 1;
        btnAllOn.Text = "Todos On";
        btnAllOn.TextColor = Color.White;
        btnAllOn.Click += btnAllOn_Click;
        // 
        // chkStomp
        // 
        chkStomp.AutoSize = true;
        chkStomp.Dock = DockStyle.Top;
        chkStomp.Location = new Point(0, 36);
        chkStomp.Name = "chkStomp";
        chkStomp.Size = new Size(440, 19);
        chkStomp.TabIndex = 0;
        chkStomp.Text = "Mandar Stomp Mode (CC 29 = 127) no toque curto";
        chkStomp.CheckedChanged += chkStomp_CheckedChanged;
        // 
        // lblFx
        // 
        lblFx.Dock = DockStyle.Top;
        lblFx.Location = new Point(0, 0);
        lblFx.Name = "lblFx";
        lblFx.Size = new Size(440, 36);
        lblFx.TabIndex = 3;
        lblFx.Text = "Estado de cada SLOT da cadeia ao pisar (A Bank / curto)\nA ordem muda com o patch. CC 43 = 1º da cadeia, CC 54 = 12º.";
        // 
        // FormMatribox
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 800);
        Controls.Add(splitMain);
        Controls.Add(panelScan);
        Controls.Add(panelTop);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimumSize = new Size(1000, 640);
        Name = "FormMatribox";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Configuração Matribox";
        panelScan.ResumeLayout(false);
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).EndInit();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        splitLeft.Panel1.ResumeLayout(false);
        splitLeft.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitLeft).EndInit();
        splitLeft.ResumeLayout(false);
        panelRight.ResumeLayout(false);
        panelRight.PerformLayout();
        panelFxScroll.ResumeLayout(false);
        panelBottom.ResumeLayout(false);
        panelBottom.PerformLayout();
        panelBtnRow2.ResumeLayout(false);
        panelBtnRow1.ResumeLayout(false);
        panelFxTop.ResumeLayout(false);
        panelFxTop.PerformLayout();
        ResumeLayout(false);
    }

    private Panel panelScan;
    private Label lblScan;
    private ProgressBar progressBarScan;
    private Panel panelTop;
    private ForeverButton btnInicio;
    private Label lblMatriboxMidi;
    private ComboBox cmbMatriboxMidi;
    private ForeverButton btnConnectMatribox;
    private Label lblChocolateMidi;
    private ComboBox cmbChocolateMidi;
    private ForeverButton btnConnectChocolate;
    private ForeverButton btnRefresh;
    private Label lblChannel;
    private NumericUpDown numChannel;
    private Label lblMatriboxHint;
    private SplitContainer splitMain;
    private SplitContainer splitLeft;
    private Label lblChoco;
    private Label lblSwitch;
    private ComboBox cmbSwitch;
    private ListBox lstChoco;
    private Label lblTree;
    private TreeView treePresets;
    private Panel panelRight;
    private Panel panelFxTop;
    private Panel panelFxScroll;
    private Panel panelBottom;
    private Panel panelBtnRow1;
    private Panel panelBtnRow2;
    private Label lblFx;
    private CheckBox chkStomp;
    private ForeverButton btnAllOn;
    private ForeverButton btnAllOff;
    private ForeverButton btnImportPrst;
    private ForeverButton btnScanMatribox;
    private TableLayoutPanel tableFx;
    private TextBox txtPreview;
    private ForeverButton btnApply;
    private ForeverButton btnMapBank;
    private ForeverButton btnTestMatribox;
    private ForeverButton btnReadChocolate;
    private ForeverButton btnWriteChocolate;
    private TextBox txtLog;
}
