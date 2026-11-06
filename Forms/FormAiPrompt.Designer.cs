using ChocolateEditor.Controls;

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
        btnInicio = new Button();
        lblTitle = new Label();
        cmbProvider = new ComboBox();
        btnSettings = new Button();
        btnSend = new Button();
        panelBar = new Panel();
        chkPatchMatribox = new CheckBox();
        chkSendManual = new CheckBox();
        panelMusic = new Panel();
        tblMusic = new TableLayoutPanel();
        lblArtist = new Label();
        lblAlbum = new Label();
        lblSong = new Label();
        sugArtist = new SuggestBox();
        sugAlbum = new SuggestBox();
        sugSong = new SuggestBox();
        lblMusicStatus = new Label();
        panelNotes = new Panel();
        lblExtra = new Label();
        txtPrompt = new TextBox();
        grpChain = new GroupBox();
        panelChainHead = new Panel();
        lblPatchTitle = new Label();
        lblPatchMeta = new Label();
        btnExportPrst = new Button();
        flowChain = new FlowLayoutPanel();
        lblPatchNote = new Label();
        webReply = new WebBrowser();
        btnBuildPrompt = new Button();
        btnEditReply = new Button();
        panelExternal = new Panel();
        tabsExternal = new TabControl();
        tabPrompt = new TabPage();
        tabReply = new TabPage();
        panelPromptBar = new Panel();
        panelReplyBar = new Panel();
        lblPromptHint = new Label();
        lblReplyHint = new Label();
        btnCopyPrompt = new Button();
        btnProcessReply = new Button();
        txtGeneratedPrompt = new TextBox();
        txtPastedReply = new TextBox();
        panelTop.SuspendLayout();
        panelBar.SuspendLayout();
        panelMusic.SuspendLayout();
        tblMusic.SuspendLayout();
        panelNotes.SuspendLayout();
        grpChain.SuspendLayout();
        panelChainHead.SuspendLayout();
        panelExternal.SuspendLayout();
        tabsExternal.SuspendLayout();
        tabPrompt.SuspendLayout();
        tabReply.SuspendLayout();
        panelPromptBar.SuspendLayout();
        panelReplyBar.SuspendLayout();
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
        panelTop.Size = new Size(960, 56);
        panelTop.TabIndex = 0;
        // 
        // btnInicio
        // 
        btnInicio.Location = new Point(12, 12);
        btnInicio.Name = "btnInicio";
        btnInicio.Size = new Size(80, 32);
        btnInicio.TabIndex = 0;
        btnInicio.Text = "← Início";
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
        cmbProvider.Location = new Point(548, 16);
        cmbProvider.Name = "cmbProvider";
        cmbProvider.Size = new Size(140, 23);
        cmbProvider.TabIndex = 2;
        // 
        // btnSettings
        // 
        btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSettings.Location = new Point(696, 12);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(100, 32);
        btnSettings.TabIndex = 3;
        btnSettings.Text = "Configurar";
        btnSettings.Click += btnSettings_Click;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSend.Location = new Point(804, 12);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(144, 32);
        btnSend.TabIndex = 4;
        btnSend.Text = "Enviar";
        btnSend.Click += btnSend_Click;
        // 
        // panelBar
        // 
        panelBar.Controls.Add(btnBuildPrompt);
        panelBar.Controls.Add(chkSendManual);
        panelBar.Controls.Add(chkPatchMatribox);
        panelBar.Dock = DockStyle.Top;
        panelBar.Location = new Point(0, 56);
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
        // btnBuildPrompt
        // 
        btnBuildPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuildPrompt.Location = new Point(800, 13);
        btnBuildPrompt.Name = "btnBuildPrompt";
        btnBuildPrompt.Size = new Size(148, 32);
        btnBuildPrompt.TabIndex = 2;
        btnBuildPrompt.Text = "Gerar prompt";
        btnBuildPrompt.Click += btnBuildPrompt_Click;
        // 
        // panelMusic
        // 
        panelMusic.Controls.Add(tblMusic);
        panelMusic.Dock = DockStyle.Top;
        panelMusic.Location = new Point(0, 114);
        panelMusic.Name = "panelMusic";
        panelMusic.Padding = new Padding(12, 6, 12, 4);
        panelMusic.Size = new Size(960, 92);
        panelMusic.TabIndex = 2;
        // 
        // tblMusic
        // 
        tblMusic.ColumnCount = 3;
        tblMusic.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tblMusic.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tblMusic.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        tblMusic.Controls.Add(lblArtist, 0, 0);
        tblMusic.Controls.Add(lblAlbum, 1, 0);
        tblMusic.Controls.Add(lblSong, 2, 0);
        tblMusic.Controls.Add(sugArtist, 0, 1);
        tblMusic.Controls.Add(sugAlbum, 1, 1);
        tblMusic.Controls.Add(sugSong, 2, 1);
        tblMusic.Controls.Add(lblMusicStatus, 0, 2);
        tblMusic.SetColumnSpan(lblMusicStatus, 3);
        tblMusic.Dock = DockStyle.Fill;
        tblMusic.Location = new Point(12, 6);
        tblMusic.Name = "tblMusic";
        tblMusic.RowCount = 3;
        tblMusic.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tblMusic.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tblMusic.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tblMusic.Size = new Size(936, 82);
        tblMusic.TabIndex = 0;
        // 
        // lblArtist
        // 
        lblArtist.AutoSize = true;
        lblArtist.Dock = DockStyle.Fill;
        lblArtist.Location = new Point(0, 0);
        lblArtist.Margin = new Padding(0, 0, 8, 0);
        lblArtist.Name = "lblArtist";
        lblArtist.Size = new Size(304, 20);
        lblArtist.TabIndex = 0;
        lblArtist.Text = "Banda / artista";
        lblArtist.TextAlign = ContentAlignment.BottomLeft;
        // 
        // lblAlbum
        // 
        lblAlbum.AutoSize = true;
        lblAlbum.Dock = DockStyle.Fill;
        lblAlbum.Location = new Point(312, 0);
        lblAlbum.Margin = new Padding(0, 0, 8, 0);
        lblAlbum.Name = "lblAlbum";
        lblAlbum.Size = new Size(304, 20);
        lblAlbum.TabIndex = 1;
        lblAlbum.Text = "Álbum (opcional)";
        lblAlbum.TextAlign = ContentAlignment.BottomLeft;
        // 
        // lblSong
        // 
        lblSong.AutoSize = true;
        lblSong.Dock = DockStyle.Fill;
        lblSong.Location = new Point(624, 0);
        lblSong.Margin = new Padding(0);
        lblSong.Name = "lblSong";
        lblSong.Size = new Size(312, 20);
        lblSong.TabIndex = 2;
        lblSong.Text = "Música";
        lblSong.TextAlign = ContentAlignment.BottomLeft;
        // 
        // sugArtist
        // 
        sugArtist.Dock = DockStyle.Fill;
        sugArtist.Location = new Point(0, 20);
        sugArtist.Margin = new Padding(0, 2, 8, 0);
        sugArtist.Name = "sugArtist";
        sugArtist.Placeholder = "Escreve para procurar…";
        sugArtist.Size = new Size(304, 30);
        sugArtist.TabIndex = 3;
        // 
        // sugAlbum
        // 
        sugAlbum.Dock = DockStyle.Fill;
        sugAlbum.Enabled = false;
        sugAlbum.Location = new Point(312, 20);
        sugAlbum.Margin = new Padding(0, 2, 8, 0);
        sugAlbum.Name = "sugAlbum";
        sugAlbum.Placeholder = "Opcional — filtra as músicas";
        sugAlbum.Size = new Size(304, 30);
        sugAlbum.TabIndex = 4;
        // 
        // sugSong
        // 
        sugSong.Dock = DockStyle.Fill;
        sugSong.Enabled = false;
        sugSong.Location = new Point(624, 20);
        sugSong.Margin = new Padding(0, 2, 0, 0);
        sugSong.Name = "sugSong";
        sugSong.Placeholder = "Escreve para filtrar…";
        sugSong.Size = new Size(312, 30);
        sugSong.TabIndex = 5;
        // 
        // lblMusicStatus
        // 
        lblMusicStatus.AutoEllipsis = true;
        lblMusicStatus.Dock = DockStyle.Fill;
        lblMusicStatus.ForeColor = Color.FromArgb(90, 90, 90);
        lblMusicStatus.Location = new Point(0, 54);
        lblMusicStatus.Margin = new Padding(0, 4, 0, 0);
        lblMusicStatus.Name = "lblMusicStatus";
        lblMusicStatus.Size = new Size(936, 18);
        lblMusicStatus.TabIndex = 6;
        lblMusicStatus.Text = "Base iTunes. Escreve a banda e a lista vai filtrando.";
        lblMusicStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // panelNotes
        // 
        panelNotes.Controls.Add(txtPrompt);
        panelNotes.Controls.Add(lblExtra);
        panelNotes.Dock = DockStyle.Top;
        panelNotes.Location = new Point(0, 206);
        panelNotes.Name = "panelNotes";
        panelNotes.Padding = new Padding(12, 0, 12, 8);
        panelNotes.Size = new Size(960, 118);
        panelNotes.TabIndex = 3;
        // 
        // lblExtra
        // 
        lblExtra.Dock = DockStyle.Top;
        lblExtra.Location = new Point(12, 0);
        lblExtra.Name = "lblExtra";
        lblExtra.Size = new Size(936, 22);
        lblExtra.TabIndex = 0;
        lblExtra.Text = "Informação extra (opcional)";
        lblExtra.TextAlign = ContentAlignment.BottomLeft;
        // 
        // txtPrompt
        // 
        txtPrompt.Dock = DockStyle.Fill;
        txtPrompt.Font = new Font("Segoe UI", 11F);
        txtPrompt.Location = new Point(12, 22);
        txtPrompt.Multiline = true;
        txtPrompt.Name = "txtPrompt";
        txtPrompt.PlaceholderText = "Ex.: só o refrão, mais ganho, limpo na rítmica…";
        txtPrompt.ScrollBars = ScrollBars.Vertical;
        txtPrompt.Size = new Size(936, 88);
        txtPrompt.TabIndex = 1;
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
        panelChainHead.Controls.Add(btnEditReply);
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
        btnExportPrst.Size = new Size(144, 32);
        btnExportPrst.TabIndex = 2;
        btnExportPrst.Text = "Guardar .prst";
        btnExportPrst.Click += btnExportPrst_Click;
        // 
        // btnEditReply
        // 
        btnEditReply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEditReply.Font = new Font("Segoe UI", 9F);
        btnEditReply.Location = new Point(620, 8);
        btnEditReply.Name = "btnEditReply";
        btnEditReply.Size = new Size(156, 32);
        btnEditReply.TabIndex = 3;
        btnEditReply.Text = "Corrigir resposta";
        btnEditReply.Visible = false;
        btnEditReply.Click += btnEditReply_Click;
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
        // panelExternal
        // 
        panelExternal.Controls.Add(tabsExternal);
        panelExternal.Dock = DockStyle.Fill;
        panelExternal.Location = new Point(0, 236);
        panelExternal.Name = "panelExternal";
        panelExternal.Size = new Size(960, 404);
        panelExternal.TabIndex = 4;
        panelExternal.Visible = false;
        // 
        // tabsExternal
        // 
        tabsExternal.Controls.Add(tabPrompt);
        tabsExternal.Controls.Add(tabReply);
        tabsExternal.Dock = DockStyle.Fill;
        tabsExternal.Location = new Point(0, 0);
        tabsExternal.Name = "tabsExternal";
        tabsExternal.SelectedIndex = 0;
        tabsExternal.Size = new Size(960, 404);
        tabsExternal.TabIndex = 0;
        // 
        // tabPrompt
        // 
        tabPrompt.Controls.Add(txtGeneratedPrompt);
        tabPrompt.Controls.Add(panelPromptBar);
        tabPrompt.Location = new Point(4, 24);
        tabPrompt.Name = "tabPrompt";
        tabPrompt.Padding = new Padding(3);
        tabPrompt.Size = new Size(952, 376);
        tabPrompt.TabIndex = 0;
        tabPrompt.Text = "Prompt";
        tabPrompt.UseVisualStyleBackColor = true;
        // 
        // panelPromptBar
        // 
        panelPromptBar.Controls.Add(btnCopyPrompt);
        panelPromptBar.Controls.Add(lblPromptHint);
        panelPromptBar.Dock = DockStyle.Top;
        panelPromptBar.Location = new Point(3, 3);
        panelPromptBar.Name = "panelPromptBar";
        panelPromptBar.Size = new Size(946, 40);
        panelPromptBar.TabIndex = 0;
        // 
        // lblPromptHint
        // 
        lblPromptHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblPromptHint.Font = new Font("Segoe UI", 9F);
        lblPromptHint.Location = new Point(8, 10);
        lblPromptHint.Name = "lblPromptHint";
        lblPromptHint.Size = new Size(760, 20);
        lblPromptHint.TabIndex = 0;
        lblPromptHint.Text = "Copia este texto e cola numa IA (ChatGPT, Claude ou Gemini).";
        // 
        // btnCopyPrompt
        // 
        btnCopyPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCopyPrompt.Location = new Point(818, 5);
        btnCopyPrompt.Name = "btnCopyPrompt";
        btnCopyPrompt.Size = new Size(120, 30);
        btnCopyPrompt.TabIndex = 1;
        btnCopyPrompt.Text = "Copiar";
        btnCopyPrompt.Click += btnCopyPrompt_Click;
        // 
        // txtGeneratedPrompt
        // 
        txtGeneratedPrompt.Dock = DockStyle.Fill;
        txtGeneratedPrompt.Font = new Font("Consolas", 9.5F);
        txtGeneratedPrompt.Location = new Point(3, 43);
        txtGeneratedPrompt.MaxLength = 0;
        txtGeneratedPrompt.Multiline = true;
        txtGeneratedPrompt.Name = "txtGeneratedPrompt";
        txtGeneratedPrompt.ReadOnly = true;
        txtGeneratedPrompt.ScrollBars = ScrollBars.Both;
        txtGeneratedPrompt.Size = new Size(946, 330);
        txtGeneratedPrompt.TabIndex = 1;
        txtGeneratedPrompt.WordWrap = false;
        // 
        // tabReply
        // 
        tabReply.Controls.Add(txtPastedReply);
        tabReply.Controls.Add(panelReplyBar);
        tabReply.Location = new Point(4, 24);
        tabReply.Name = "tabReply";
        tabReply.Padding = new Padding(3);
        tabReply.Size = new Size(952, 376);
        tabReply.TabIndex = 1;
        tabReply.Text = "Resposta";
        tabReply.UseVisualStyleBackColor = true;
        // 
        // panelReplyBar
        // 
        panelReplyBar.Controls.Add(btnProcessReply);
        panelReplyBar.Controls.Add(lblReplyHint);
        panelReplyBar.Dock = DockStyle.Top;
        panelReplyBar.Location = new Point(3, 3);
        panelReplyBar.Name = "panelReplyBar";
        panelReplyBar.Size = new Size(946, 40);
        panelReplyBar.TabIndex = 0;
        // 
        // lblReplyHint
        // 
        lblReplyHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblReplyHint.Font = new Font("Segoe UI", 9F);
        lblReplyHint.Location = new Point(8, 10);
        lblReplyHint.Name = "lblReplyHint";
        lblReplyHint.Size = new Size(740, 20);
        lblReplyHint.TabIndex = 0;
        lblReplyHint.Text = "Cola aqui a resposta da IA. Ctrl+Enter também processa.";
        // 
        // btnProcessReply
        // 
        btnProcessReply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnProcessReply.Location = new Point(798, 5);
        btnProcessReply.Name = "btnProcessReply";
        btnProcessReply.Size = new Size(140, 30);
        btnProcessReply.TabIndex = 1;
        btnProcessReply.Text = "Montar patch";
        btnProcessReply.Click += btnProcessReply_Click;
        // 
        // txtPastedReply
        // 
        txtPastedReply.Dock = DockStyle.Fill;
        txtPastedReply.Font = new Font("Consolas", 9.5F);
        txtPastedReply.Location = new Point(3, 43);
        txtPastedReply.MaxLength = 0;
        txtPastedReply.Multiline = true;
        txtPastedReply.Name = "txtPastedReply";
        txtPastedReply.PlaceholderText = "Cola aqui a resposta da IA…";
        txtPastedReply.ScrollBars = ScrollBars.Both;
        txtPastedReply.Size = new Size(946, 330);
        txtPastedReply.TabIndex = 1;
        txtPastedReply.WordWrap = false;
        // 
        // FormAiPrompt
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Controls.Add(panelExternal);
        Controls.Add(webReply);
        Controls.Add(grpChain);
        Controls.Add(panelNotes);
        Controls.Add(panelMusic);
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
        tblMusic.ResumeLayout(false);
        tblMusic.PerformLayout();
        panelMusic.ResumeLayout(false);
        panelNotes.ResumeLayout(false);
        panelNotes.PerformLayout();
        grpChain.ResumeLayout(false);
        panelChainHead.ResumeLayout(false);
        panelChainHead.PerformLayout();
        panelPromptBar.ResumeLayout(false);
        panelReplyBar.ResumeLayout(false);
        tabPrompt.ResumeLayout(false);
        tabPrompt.PerformLayout();
        tabReply.ResumeLayout(false);
        tabReply.PerformLayout();
        tabsExternal.ResumeLayout(false);
        panelExternal.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel panelTop;
    private Button btnInicio;
    private Label lblTitle;
    private ComboBox cmbProvider;
    private Button btnSettings;
    private Button btnSend;
    private Panel panelBar;
    private CheckBox chkPatchMatribox;
    private CheckBox chkSendManual;
    private Panel panelMusic;
    private TableLayoutPanel tblMusic;
    private Label lblArtist;
    private Label lblAlbum;
    private Label lblSong;
    private SuggestBox sugArtist;
    private SuggestBox sugAlbum;
    private SuggestBox sugSong;
    private Label lblMusicStatus;
    private Panel panelNotes;
    private Label lblExtra;
    private TextBox txtPrompt;
    private GroupBox grpChain;
    private Panel panelChainHead;
    private Label lblPatchTitle;
    private Label lblPatchMeta;
    private Button btnExportPrst;
    private FlowLayoutPanel flowChain;
    private Label lblPatchNote;
    private WebBrowser webReply;
    private Button btnBuildPrompt;
    private Button btnEditReply;
    private Panel panelExternal;
    private TabControl tabsExternal;
    private TabPage tabPrompt;
    private TabPage tabReply;
    private Panel panelPromptBar;
    private Panel panelReplyBar;
    private Label lblPromptHint;
    private Label lblReplyHint;
    private Button btnCopyPrompt;
    private Button btnProcessReply;
    private TextBox txtGeneratedPrompt;
    private TextBox txtPastedReply;
}
