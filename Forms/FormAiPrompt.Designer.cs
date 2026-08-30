using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

namespace ChocolateEditor.Forms;

partial class FormAiPrompt
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
        btnInicio = new ForeverButton();
        lblTitle = new Label();
        cmbProvider = new ComboBox();
        btnSettings = new ForeverButton();
        btnSend = new ForeverButton();
        panelBar = new Panel();
        chkPatchMatribox = new CheckBox();
        chkSendManual = new CheckBox();
        txtPrompt = new TextBox();
        grpChain = new GroupBox();
        panelChainHead = new Panel();
        lblPatchTitle = new Label();
        lblPatchMeta = new Label();
        btnExportPrst = new ForeverButton();
        flowChain = new FlowLayoutPanel();
        lblPatchNote = new Label();
        webReply = new WebBrowser();
        panelTop.SuspendLayout();
        panelBar.SuspendLayout();
        grpChain.SuspendLayout();
        panelChainHead.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.Controls.Add(btnSend);
        panelTop.Controls.Add(btnSettings);
        panelTop.Controls.Add(cmbProvider);
        panelTop.Controls.Add(lblTitle);
        panelTop.Controls.Add(btnInicio);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(12, 10, 12, 10);
        panelTop.Size = new Size(960, 64);
        panelTop.TabIndex = 0;
        // 
        // btnInicio
        // 
        btnInicio.Location = new Point(12, 14);
        btnInicio.Name = "btnInicio";
        btnInicio.Rounded = true;
        btnInicio.Size = new Size(88, 36);
        btnInicio.TabIndex = 0;
        btnInicio.Text = "← Início";
        btnInicio.TextColor = Color.White;
        btnInicio.Click += btnInicio_Click;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(104, 16);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(80, 21);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Prompt IA";
        // 
        // cmbProvider
        // 
        cmbProvider.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cmbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProvider.Location = new Point(548, 20);
        cmbProvider.Name = "cmbProvider";
        cmbProvider.Size = new Size(140, 23);
        cmbProvider.TabIndex = 2;
        // 
        // btnSettings
        // 
        btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSettings.Location = new Point(696, 14);
        btnSettings.Name = "btnSettings";
        btnSettings.Rounded = true;
        btnSettings.Size = new Size(100, 36);
        btnSettings.TabIndex = 3;
        btnSettings.Text = "Configurar";
        btnSettings.TextColor = Color.White;
        btnSettings.Click += btnSettings_Click;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSend.Location = new Point(804, 14);
        btnSend.Name = "btnSend";
        btnSend.Rounded = true;
        btnSend.Size = new Size(144, 36);
        btnSend.TabIndex = 4;
        btnSend.Text = "Enviar";
        btnSend.TextColor = Color.White;
        btnSend.Click += btnSend_Click;
        // 
        // panelBar
        // 
        panelBar.Controls.Add(chkSendManual);
        panelBar.Controls.Add(chkPatchMatribox);
        panelBar.Dock = DockStyle.Top;
        panelBar.Location = new Point(0, 64);
        panelBar.Name = "panelBar";
        panelBar.Size = new Size(960, 58);
        panelBar.TabIndex = 1;
        // 
        // chkPatchMatribox
        // 
        chkPatchMatribox.AutoSize = true;
        chkPatchMatribox.Checked = true;
        chkPatchMatribox.CheckState = CheckState.Checked;
        chkPatchMatribox.Location = new Point(16, 6);
        chkPatchMatribox.Name = "chkPatchMatribox";
        chkPatchMatribox.Size = new Size(520, 19);
        chkPatchMatribox.TabIndex = 0;
        chkPatchMatribox.Text = "Gerar patch Matribox II Pro (envia a tabela e mostra a cadeia)";
        chkPatchMatribox.CheckedChanged += chkPatchMatribox_CheckedChanged;
        // 
        // chkSendManual
        // 
        chkSendManual.AutoSize = true;
        chkSendManual.Enabled = true;
        chkSendManual.Location = new Point(16, 32);
        chkSendManual.Name = "chkSendManual";
        chkSendManual.Size = new Size(420, 19);
        chkSendManual.TabIndex = 1;
        chkSendManual.Text = "Enviar manual e knobs (mais pesado, melhor contexto)";
        chkSendManual.CheckedChanged += chkSendManual_CheckedChanged;
        // 
        // txtPrompt
        // 
        txtPrompt.Dock = DockStyle.Top;
        txtPrompt.Font = new Font("Segoe UI", 11F);
        txtPrompt.Location = new Point(0, 96);
        txtPrompt.Multiline = true;
        txtPrompt.Name = "txtPrompt";
        txtPrompt.PlaceholderText = "Ex.: gerar um patch para Matribox 2 Pro para tocar Nirvana";
        txtPrompt.ScrollBars = ScrollBars.Vertical;
        txtPrompt.Size = new Size(960, 140);
        txtPrompt.TabIndex = 2;
        // 
        // grpChain
        // 
        grpChain.Controls.Add(flowChain);
        grpChain.Controls.Add(lblPatchNote);
        grpChain.Controls.Add(panelChainHead);
        grpChain.Dock = DockStyle.Fill;
        grpChain.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpChain.Location = new Point(0, 236);
        grpChain.Name = "grpChain";
        grpChain.Padding = new Padding(10, 8, 10, 8);
        grpChain.Size = new Size(960, 404);
        grpChain.TabIndex = 3;
        grpChain.Text = "Cadeia";
        grpChain.Visible = false;
        // 
        // panelChainHead
        // 
        panelChainHead.Controls.Add(btnExportPrst);
        panelChainHead.Controls.Add(lblPatchMeta);
        panelChainHead.Controls.Add(lblPatchTitle);
        panelChainHead.Dock = DockStyle.Top;
        panelChainHead.Location = new Point(10, 24);
        panelChainHead.Name = "panelChainHead";
        panelChainHead.Size = new Size(940, 48);
        panelChainHead.TabIndex = 0;
        // 
        // lblPatchTitle
        // 
        lblPatchTitle.AutoSize = true;
        lblPatchTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblPatchTitle.Location = new Point(4, 4);
        lblPatchTitle.Name = "lblPatchTitle";
        lblPatchTitle.Size = new Size(80, 21);
        lblPatchTitle.TabIndex = 0;
        lblPatchTitle.Text = "Patch";
        // 
        // lblPatchMeta
        // 
        lblPatchMeta.AutoSize = true;
        lblPatchMeta.Font = new Font("Segoe UI", 9F);
        lblPatchMeta.ForeColor = Color.FromArgb(80, 80, 80);
        lblPatchMeta.Location = new Point(6, 26);
        lblPatchMeta.Name = "lblPatchMeta";
        lblPatchMeta.Size = new Size(40, 15);
        lblPatchMeta.TabIndex = 1;
        lblPatchMeta.Text = "";
        // 
        // btnExportPrst
        // 
        btnExportPrst.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExportPrst.Enabled = false;
        btnExportPrst.Font = new Font("Segoe UI", 9F);
        btnExportPrst.Location = new Point(784, 8);
        btnExportPrst.Name = "btnExportPrst";
        btnExportPrst.Rounded = true;
        btnExportPrst.Size = new Size(144, 32);
        btnExportPrst.TabIndex = 2;
        btnExportPrst.Text = "Guardar .prst";
        btnExportPrst.TextColor = Color.White;
        btnExportPrst.Click += btnExportPrst_Click;
        // 
        // flowChain
        // 
        flowChain.AutoScroll = true;
        flowChain.Dock = DockStyle.Fill;
        flowChain.Location = new Point(10, 72);
        flowChain.Name = "flowChain";
        flowChain.Padding = new Padding(4);
        flowChain.Size = new Size(940, 288);
        flowChain.TabIndex = 1;
        flowChain.WrapContents = true;
        // 
        // lblPatchNote
        // 
        lblPatchNote.Dock = DockStyle.Bottom;
        lblPatchNote.Font = new Font("Segoe UI", 9F);
        lblPatchNote.ForeColor = Color.FromArgb(60, 60, 60);
        lblPatchNote.Location = new Point(10, 360);
        lblPatchNote.Name = "lblPatchNote";
        lblPatchNote.Size = new Size(940, 36);
        lblPatchNote.TabIndex = 2;
        lblPatchNote.Text = "";
        // 
        // webReply
        // 
        webReply.AllowWebBrowserDrop = false;
        webReply.Dock = DockStyle.Fill;
        webReply.IsWebBrowserContextMenuEnabled = true;
        webReply.Location = new Point(0, 236);
        webReply.MinimumSize = new Size(20, 20);
        webReply.Name = "webReply";
        webReply.ScriptErrorsSuppressed = true;
        webReply.Size = new Size(960, 404);
        webReply.TabIndex = 3;
        webReply.WebBrowserShortcutsEnabled = true;
        // 
        // FormAiPrompt
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Controls.Add(webReply);
        Controls.Add(grpChain);
        Controls.Add(txtPrompt);
        Controls.Add(panelBar);
        Controls.Add(panelTop);
        MinimumSize = new Size(800, 520);
        Name = "FormAiPrompt";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Prompt IA";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelBar.ResumeLayout(false);
        panelBar.PerformLayout();
        grpChain.ResumeLayout(false);
        panelChainHead.ResumeLayout(false);
        panelChainHead.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel panelTop;
    private ForeverButton btnInicio;
    private Label lblTitle;
    private ComboBox cmbProvider;
    private ForeverButton btnSettings;
    private ForeverButton btnSend;
    private Panel panelBar;
    private CheckBox chkPatchMatribox;
    private CheckBox chkSendManual;
    private TextBox txtPrompt;
    private GroupBox grpChain;
    private Panel panelChainHead;
    private Label lblPatchTitle;
    private Label lblPatchMeta;
    private ForeverButton btnExportPrst;
    private FlowLayoutPanel flowChain;
    private Label lblPatchNote;
    private WebBrowser webReply;
}
