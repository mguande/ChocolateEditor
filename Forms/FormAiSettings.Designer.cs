using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

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
        btnSave = new ForeverButton();
        btnCancel = new ForeverButton();
        SuspendLayout();
        // 
        // lblHint
        // 
        lblHint.Location = new Point(20, 16);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(520, 48);
        lblHint.TabIndex = 0;
        lblHint.Text = "As chaves ficam só neste PC (%LocalAppData%\\ChocolateEditor\\ai-settings.json) e nunca vão no instalador.\nChatGPT: platform.openai.com  ·  Claude: console.anthropic.com  ·  Gemini: aistudio.google.com";
        // 
        // lblOpenAi
        // 
        lblOpenAi.AutoSize = true;
        lblOpenAi.Location = new Point(20, 76);
        lblOpenAi.Name = "lblOpenAi";
        lblOpenAi.Size = new Size(120, 15);
        lblOpenAi.TabIndex = 1;
        lblOpenAi.Text = "ChatGPT · API key";
        // 
        // txtOpenAiKey
        // 
        txtOpenAiKey.Location = new Point(20, 96);
        txtOpenAiKey.Name = "txtOpenAiKey";
        txtOpenAiKey.PlaceholderText = "sk-...";
        txtOpenAiKey.Size = new Size(520, 23);
        txtOpenAiKey.TabIndex = 2;
        txtOpenAiKey.UseSystemPasswordChar = true;
        // 
        // lblOpenAiModel
        // 
        lblOpenAiModel.AutoSize = true;
        lblOpenAiModel.Location = new Point(20, 132);
        lblOpenAiModel.Name = "lblOpenAiModel";
        lblOpenAiModel.Size = new Size(80, 15);
        lblOpenAiModel.TabIndex = 3;
        lblOpenAiModel.Text = "Modelo OpenAI";
        // 
        // txtOpenAiModel
        // 
        txtOpenAiModel.Location = new Point(20, 152);
        txtOpenAiModel.Name = "txtOpenAiModel";
        txtOpenAiModel.Size = new Size(240, 23);
        txtOpenAiModel.TabIndex = 4;
        // 
        // lblClaude
        // 
        lblClaude.AutoSize = true;
        lblClaude.Location = new Point(20, 196);
        lblClaude.Name = "lblClaude";
        lblClaude.Size = new Size(108, 15);
        lblClaude.TabIndex = 5;
        lblClaude.Text = "Claude · API key";
        // 
        // txtClaudeKey
        // 
        txtClaudeKey.Location = new Point(20, 216);
        txtClaudeKey.Name = "txtClaudeKey";
        txtClaudeKey.PlaceholderText = "sk-ant-...";
        txtClaudeKey.Size = new Size(520, 23);
        txtClaudeKey.TabIndex = 6;
        txtClaudeKey.UseSystemPasswordChar = true;
        // 
        // lblClaudeModel
        // 
        lblClaudeModel.AutoSize = true;
        lblClaudeModel.Location = new Point(20, 252);
        lblClaudeModel.Name = "lblClaudeModel";
        lblClaudeModel.Size = new Size(84, 15);
        lblClaudeModel.TabIndex = 7;
        lblClaudeModel.Text = "Modelo Claude";
        // 
        // txtClaudeModel
        // 
        txtClaudeModel.Location = new Point(20, 272);
        txtClaudeModel.Name = "txtClaudeModel";
        txtClaudeModel.Size = new Size(240, 23);
        txtClaudeModel.TabIndex = 8;
        // 
        // lblGemini
        // 
        lblGemini.AutoSize = true;
        lblGemini.Location = new Point(20, 316);
        lblGemini.Name = "lblGemini";
        lblGemini.Size = new Size(108, 15);
        lblGemini.TabIndex = 9;
        lblGemini.Text = "Gemini · API key";
        // 
        // txtGeminiKey
        // 
        txtGeminiKey.Location = new Point(20, 336);
        txtGeminiKey.Name = "txtGeminiKey";
        txtGeminiKey.PlaceholderText = "AIza...";
        txtGeminiKey.Size = new Size(520, 23);
        txtGeminiKey.TabIndex = 10;
        txtGeminiKey.UseSystemPasswordChar = true;
        // 
        // lblGeminiModel
        // 
        lblGeminiModel.AutoSize = true;
        lblGeminiModel.Location = new Point(20, 372);
        lblGeminiModel.Name = "lblGeminiModel";
        lblGeminiModel.Size = new Size(88, 15);
        lblGeminiModel.TabIndex = 11;
        lblGeminiModel.Text = "Modelo Gemini";
        // 
        // txtGeminiModel
        // 
        txtGeminiModel.Location = new Point(20, 392);
        txtGeminiModel.Name = "txtGeminiModel";
        txtGeminiModel.Size = new Size(240, 23);
        txtGeminiModel.TabIndex = 12;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(348, 444);
        btnSave.Name = "btnSave";
        btnSave.Rounded = true;
        btnSave.Size = new Size(100, 36);
        btnSave.TabIndex = 14;
        btnSave.Text = "Guardar";
        btnSave.TextColor = Color.White;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(454, 444);
        btnCancel.Name = "btnCancel";
        btnCancel.Rounded = true;
        btnCancel.Size = new Size(90, 36);
        btnCancel.TabIndex = 13;
        btnCancel.Text = "Cancelar";
        btnCancel.TextColor = Color.White;
        btnCancel.Click += btnCancel_Click;
        // 
        // FormAiSettings
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(564, 500);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtGeminiModel);
        Controls.Add(lblGeminiModel);
        Controls.Add(txtGeminiKey);
        Controls.Add(lblGemini);
        Controls.Add(txtClaudeModel);
        Controls.Add(lblClaudeModel);
        Controls.Add(txtClaudeKey);
        Controls.Add(lblClaude);
        Controls.Add(txtOpenAiModel);
        Controls.Add(lblOpenAiModel);
        Controls.Add(txtOpenAiKey);
        Controls.Add(lblOpenAi);
        Controls.Add(lblHint);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAiSettings";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Configuração";
        ResumeLayout(false);
        PerformLayout();
    }

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
    private ForeverButton btnSave;
    private ForeverButton btnCancel;
}
