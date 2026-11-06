namespace ChocolateEditor.Forms;

partial class FormAiSettings
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
        tabsSettings = new TabControl();
        tabKeys = new TabPage();
        tabInstructions = new TabPage();
        tabPatches = new TabPage();
        lblPrstHint = new Label();
        lblPrstFolder = new Label();
        txtPrstFolder = new TextBox();
        btnBrowsePrst = new Button();
        lblPrstStatus = new Label();
        lblHint = new Label();
        lblOpenAi = new Label();
        txtOpenAiKey = new TextBox();
        lblOpenAiModel = new Label();
        txtOpenAiModel = new TextBox();
        lblClaude = new Label();
        txtClaudeKey = new TextBox();
        lblClaudeModel = new Label();
        txtClaudeModel = new TextBox();
        lblGemini = new Label();
        txtGeminiKey = new TextBox();
        lblGeminiModel = new Label();
        txtGeminiModel = new TextBox();
        panelInstructionBar = new Panel();
        lblInstructionHint = new Label();
        btnResetInstructions = new Button();
        txtInstructions = new TextBox();
        panelButtons = new Panel();
        btnSave = new Button();
        btnCancel = new Button();
        tabsSettings.SuspendLayout();
        tabKeys.SuspendLayout();
        tabInstructions.SuspendLayout();
        tabPatches.SuspendLayout();
        panelInstructionBar.SuspendLayout();
        panelButtons.SuspendLayout();
        SuspendLayout();
        // 
        // tabsSettings
        // 
        tabsSettings.Controls.Add(tabKeys);
        tabsSettings.Controls.Add(tabInstructions);
        tabsSettings.Controls.Add(tabPatches);
        tabsSettings.Dock = DockStyle.Fill;
        tabsSettings.Location = new Point(0, 0);
        tabsSettings.Name = "tabsSettings";
        tabsSettings.SelectedIndex = 0;
        tabsSettings.Size = new Size(720, 588);
        tabsSettings.TabIndex = 0;
        // 
        // tabKeys
        // 
        tabKeys.Controls.Add(txtGeminiModel);
        tabKeys.Controls.Add(lblGeminiModel);
        tabKeys.Controls.Add(txtGeminiKey);
        tabKeys.Controls.Add(lblGemini);
        tabKeys.Controls.Add(txtClaudeModel);
        tabKeys.Controls.Add(lblClaudeModel);
        tabKeys.Controls.Add(txtClaudeKey);
        tabKeys.Controls.Add(lblClaude);
        tabKeys.Controls.Add(txtOpenAiModel);
        tabKeys.Controls.Add(lblOpenAiModel);
        tabKeys.Controls.Add(txtOpenAiKey);
        tabKeys.Controls.Add(lblOpenAi);
        tabKeys.Controls.Add(lblHint);
        tabKeys.Location = new Point(4, 24);
        tabKeys.Name = "tabKeys";
        tabKeys.Padding = new Padding(3);
        tabKeys.Size = new Size(712, 560);
        tabKeys.TabIndex = 0;
        tabKeys.Text = "Chaves";
        tabKeys.UseVisualStyleBackColor = true;
        // 
        // lblHint
        // 
        lblHint.Location = new Point(16, 12);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(660, 48);
        lblHint.TabIndex = 0;
        lblHint.Text = "As chaves ficam só neste PC (%LocalAppData%\\ChocolateEditor\\ai-settings.json) e nunca vão no instalador.\nChatGPT: platform.openai.com  ·  Claude: console.anthropic.com  ·  Gemini: aistudio.google.com";
        // 
        // lblOpenAi
        // 
        lblOpenAi.AutoSize = true;
        lblOpenAi.Location = new Point(16, 72);
        lblOpenAi.Name = "lblOpenAi";
        lblOpenAi.Size = new Size(120, 15);
        lblOpenAi.TabIndex = 1;
        lblOpenAi.Text = "ChatGPT · API key";
        // 
        // txtOpenAiKey
        // 
        txtOpenAiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtOpenAiKey.Location = new Point(16, 92);
        txtOpenAiKey.Name = "txtOpenAiKey";
        txtOpenAiKey.PlaceholderText = "sk-...";
        txtOpenAiKey.Size = new Size(660, 23);
        txtOpenAiKey.TabIndex = 2;
        txtOpenAiKey.UseSystemPasswordChar = true;
        // 
        // lblOpenAiModel
        // 
        lblOpenAiModel.AutoSize = true;
        lblOpenAiModel.Location = new Point(16, 128);
        lblOpenAiModel.Name = "lblOpenAiModel";
        lblOpenAiModel.Size = new Size(80, 15);
        lblOpenAiModel.TabIndex = 3;
        lblOpenAiModel.Text = "Modelo OpenAI";
        // 
        // txtOpenAiModel
        // 
        txtOpenAiModel.Location = new Point(16, 148);
        txtOpenAiModel.Name = "txtOpenAiModel";
        txtOpenAiModel.Size = new Size(240, 23);
        txtOpenAiModel.TabIndex = 4;
        // 
        // lblClaude
        // 
        lblClaude.AutoSize = true;
        lblClaude.Location = new Point(16, 192);
        lblClaude.Name = "lblClaude";
        lblClaude.Size = new Size(108, 15);
        lblClaude.TabIndex = 5;
        lblClaude.Text = "Claude · API key";
        // 
        // txtClaudeKey
        // 
        txtClaudeKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtClaudeKey.Location = new Point(16, 212);
        txtClaudeKey.Name = "txtClaudeKey";
        txtClaudeKey.PlaceholderText = "sk-ant-...";
        txtClaudeKey.Size = new Size(660, 23);
        txtClaudeKey.TabIndex = 6;
        txtClaudeKey.UseSystemPasswordChar = true;
        // 
        // lblClaudeModel
        // 
        lblClaudeModel.AutoSize = true;
        lblClaudeModel.Location = new Point(16, 248);
        lblClaudeModel.Name = "lblClaudeModel";
        lblClaudeModel.Size = new Size(84, 15);
        lblClaudeModel.TabIndex = 7;
        lblClaudeModel.Text = "Modelo Claude";
        // 
        // txtClaudeModel
        // 
        txtClaudeModel.Location = new Point(16, 268);
        txtClaudeModel.Name = "txtClaudeModel";
        txtClaudeModel.Size = new Size(240, 23);
        txtClaudeModel.TabIndex = 8;
        // 
        // lblGemini
        // 
        lblGemini.AutoSize = true;
        lblGemini.Location = new Point(16, 312);
        lblGemini.Name = "lblGemini";
        lblGemini.Size = new Size(108, 15);
        lblGemini.TabIndex = 9;
        lblGemini.Text = "Gemini · API key";
        // 
        // txtGeminiKey
        // 
        txtGeminiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtGeminiKey.Location = new Point(16, 332);
        txtGeminiKey.Name = "txtGeminiKey";
        txtGeminiKey.PlaceholderText = "AIza...";
        txtGeminiKey.Size = new Size(660, 23);
        txtGeminiKey.TabIndex = 10;
        txtGeminiKey.UseSystemPasswordChar = true;
        // 
        // lblGeminiModel
        // 
        lblGeminiModel.AutoSize = true;
        lblGeminiModel.Location = new Point(16, 368);
        lblGeminiModel.Name = "lblGeminiModel";
        lblGeminiModel.Size = new Size(88, 15);
        lblGeminiModel.TabIndex = 11;
        lblGeminiModel.Text = "Modelo Gemini";
        // 
        // txtGeminiModel
        // 
        txtGeminiModel.Location = new Point(16, 388);
        txtGeminiModel.Name = "txtGeminiModel";
        txtGeminiModel.Size = new Size(240, 23);
        txtGeminiModel.TabIndex = 12;
        // 
        // tabInstructions
        // 
        tabInstructions.Controls.Add(txtInstructions);
        tabInstructions.Controls.Add(panelInstructionBar);
        tabInstructions.Location = new Point(4, 24);
        tabInstructions.Name = "tabInstructions";
        tabInstructions.Padding = new Padding(8);
        tabInstructions.Size = new Size(712, 560);
        tabInstructions.TabIndex = 1;
        tabInstructions.Text = "Instruções";
        tabInstructions.UseVisualStyleBackColor = true;
        // 
        // panelInstructionBar
        // 
        panelInstructionBar.Controls.Add(btnResetInstructions);
        panelInstructionBar.Controls.Add(lblInstructionHint);
        panelInstructionBar.Dock = DockStyle.Top;
        panelInstructionBar.Location = new Point(8, 8);
        panelInstructionBar.Name = "panelInstructionBar";
        panelInstructionBar.Size = new Size(696, 48);
        panelInstructionBar.TabIndex = 0;
        // 
        // lblInstructionHint
        // 
        lblInstructionHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblInstructionHint.Location = new Point(0, 4);
        lblInstructionHint.Name = "lblInstructionHint";
        lblInstructionHint.Size = new Size(540, 40);
        lblInstructionHint.TabIndex = 0;
        lblInstructionHint.Text = "Texto das instruções do prompt do patch. Enviar e Gerar prompt usam o que guardares aqui.";
        // 
        // btnResetInstructions
        // 
        btnResetInstructions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnResetInstructions.Location = new Point(556, 8);
        btnResetInstructions.Name = "btnResetInstructions";
        btnResetInstructions.Size = new Size(132, 30);
        btnResetInstructions.TabIndex = 1;
        btnResetInstructions.Text = "Repor original";
        btnResetInstructions.Click += btnResetInstructions_Click;
        // 
        // txtInstructions
        // 
        txtInstructions.AcceptsReturn = true;
        txtInstructions.Dock = DockStyle.Fill;
        txtInstructions.Font = new Font("Segoe UI", 10F);
        txtInstructions.Location = new Point(8, 56);
        txtInstructions.MaxLength = 0;
        txtInstructions.Multiline = true;
        txtInstructions.Name = "txtInstructions";
        txtInstructions.ScrollBars = ScrollBars.Vertical;
        txtInstructions.Size = new Size(696, 496);
        txtInstructions.TabIndex = 1;
        // 
        // tabPatches
        // 
        tabPatches.Controls.Add(lblPrstStatus);
        tabPatches.Controls.Add(btnBrowsePrst);
        tabPatches.Controls.Add(txtPrstFolder);
        tabPatches.Controls.Add(lblPrstFolder);
        tabPatches.Controls.Add(lblPrstHint);
        tabPatches.Location = new Point(4, 24);
        tabPatches.Name = "tabPatches";
        tabPatches.Padding = new Padding(16);
        tabPatches.Size = new Size(712, 560);
        tabPatches.TabIndex = 2;
        tabPatches.Text = "Patches";
        tabPatches.UseVisualStyleBackColor = true;
        // 
        // lblPrstHint
        // 
        lblPrstHint.Location = new Point(16, 16);
        lblPrstHint.Name = "lblPrstHint";
        lblPrstHint.Size = new Size(660, 48);
        lblPrstHint.TabIndex = 0;
        lblPrstHint.Text = "Pasta onde estão os .prst do editor Sonicake. Ao guardar um patch, o primeiro ficheiro desta pasta serve de modelo.";
        // 
        // lblPrstFolder
        // 
        lblPrstFolder.AutoSize = true;
        lblPrstFolder.Location = new Point(16, 76);
        lblPrstFolder.Name = "lblPrstFolder";
        lblPrstFolder.Size = new Size(110, 15);
        lblPrstFolder.TabIndex = 1;
        lblPrstFolder.Text = "Pasta dos .prst";
        // 
        // txtPrstFolder
        // 
        txtPrstFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPrstFolder.Location = new Point(16, 96);
        txtPrstFolder.Name = "txtPrstFolder";
        txtPrstFolder.PlaceholderText = @"C:\Programas\Sonicake\Matribox 2 Pro\Patchs";
        txtPrstFolder.Size = new Size(540, 23);
        txtPrstFolder.TabIndex = 2;
        txtPrstFolder.TextChanged += txtPrstFolder_TextChanged;
        // 
        // btnBrowsePrst
        // 
        btnBrowsePrst.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowsePrst.Location = new Point(564, 94);
        btnBrowsePrst.Name = "btnBrowsePrst";
        btnBrowsePrst.Size = new Size(112, 27);
        btnBrowsePrst.TabIndex = 3;
        btnBrowsePrst.Text = "Procurar…";
        btnBrowsePrst.Click += btnBrowsePrst_Click;
        // 
        // lblPrstStatus
        // 
        lblPrstStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblPrstStatus.ForeColor = Color.FromArgb(80, 80, 80);
        lblPrstStatus.Location = new Point(16, 132);
        lblPrstStatus.Name = "lblPrstStatus";
        lblPrstStatus.Size = new Size(660, 36);
        lblPrstStatus.TabIndex = 4;
        lblPrstStatus.Text = "";
        // 
        // panelButtons
        // 
        panelButtons.Controls.Add(btnCancel);
        panelButtons.Controls.Add(btnSave);
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Location = new Point(0, 588);
        panelButtons.Name = "panelButtons";
        panelButtons.Size = new Size(720, 52);
        panelButtons.TabIndex = 1;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(516, 10);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(88, 32);
        btnSave.TabIndex = 0;
        btnSave.Text = "Guardar";
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(612, 10);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(92, 32);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancelar";
        btnCancel.Click += btnCancel_Click;
        // 
        // FormAiSettings
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(720, 640);
        Controls.Add(tabsSettings);
        Controls.Add(panelButtons);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAiSettings";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Configuração";
        tabsSettings.ResumeLayout(false);
        tabKeys.ResumeLayout(false);
        tabKeys.PerformLayout();
        panelInstructionBar.ResumeLayout(false);
        tabInstructions.ResumeLayout(false);
        tabInstructions.PerformLayout();
        tabPatches.ResumeLayout(false);
        tabPatches.PerformLayout();
        panelButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TabControl tabsSettings;
    private TabPage tabKeys;
    private TabPage tabInstructions;
    private TabPage tabPatches;
    private Label lblPrstHint;
    private Label lblPrstFolder;
    private TextBox txtPrstFolder;
    private Button btnBrowsePrst;
    private Label lblPrstStatus;
    private Label lblHint;
    private Label lblOpenAi;
    private TextBox txtOpenAiKey;
    private Label lblOpenAiModel;
    private TextBox txtOpenAiModel;
    private Label lblClaude;
    private TextBox txtClaudeKey;
    private Label lblClaudeModel;
    private TextBox txtClaudeModel;
    private Label lblGemini;
    private TextBox txtGeminiKey;
    private Label lblGeminiModel;
    private TextBox txtGeminiModel;
    private Panel panelInstructionBar;
    private Label lblInstructionHint;
    private Button btnResetInstructions;
    private TextBox txtInstructions;
    private Panel panelButtons;
    private Button btnSave;
    private Button btnCancel;
}
