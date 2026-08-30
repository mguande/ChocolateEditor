namespace ChocolateEditor.Forms;

partial class FormLauncher
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLauncher));
        lblTitle = new Label();
        lblSub = new Label();
        btnChocolate = new Button();
        btnScene = new Button();
        btnMatribox = new Button();
        btnAi = new Button();
        btnConfig = new Button();
        lblHint = new Label();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.Location = new Point(40, 28);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(640, 44);
        lblTitle.TabIndex = 3;
        lblTitle.Text = "Chocolate Plus Editor";
        // 
        // lblSub
        // 
        lblSub.Font = new Font("Segoe UI", 11F);
        lblSub.ForeColor = Color.FromArgb(80, 70, 60);
        lblSub.Location = new Point(40, 76);
        lblSub.Name = "lblSub";
        lblSub.Size = new Size(640, 40);
        lblSub.TabIndex = 2;
        lblSub.Text = "Escolhe o que queres editar. O Chocolate controla a Matribox por MIDI.";
        // 
        // btnChocolate
        // 
        btnChocolate.BackColor = Color.FromArgb(92, 51, 23);
        btnChocolate.FlatAppearance.BorderSize = 0;
        btnChocolate.FlatStyle = FlatStyle.Flat;
        btnChocolate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnChocolate.ForeColor = Color.White;
        btnChocolate.Location = new Point(40, 132);
        btnChocolate.Name = "btnChocolate";
        btnChocolate.Size = new Size(200, 140);
        btnChocolate.TabIndex = 0;
        btnChocolate.Text = "Chocolate Plus\n\nEditor USB\nbancos e MIDI";
        btnChocolate.UseVisualStyleBackColor = false;
        btnChocolate.Click += btnChocolate_Click;
        // 
        // btnScene
        // 
        btnScene.BackColor = Color.FromArgb(20, 72, 120);
        btnScene.FlatAppearance.BorderSize = 0;
        btnScene.FlatStyle = FlatStyle.Flat;
        btnScene.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnScene.ForeColor = Color.White;
        btnScene.Location = new Point(256, 132);
        btnScene.Name = "btnScene";
        btnScene.Size = new Size(200, 140);
        btnScene.TabIndex = 1;
        btnScene.Text = "Chocolate × Matribox\n\nSwitch A = patch\nB/C/D = efeitos";
        btnScene.UseVisualStyleBackColor = false;
        btnScene.Click += btnScene_Click;
        // 
        // btnMatribox
        // 
        btnMatribox.BackColor = Color.FromArgb(16, 92, 92);
        btnMatribox.FlatAppearance.BorderSize = 0;
        btnMatribox.FlatStyle = FlatStyle.Flat;
        btnMatribox.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnMatribox.ForeColor = Color.White;
        btnMatribox.Location = new Point(472, 132);
        btnMatribox.Name = "btnMatribox";
        btnMatribox.Size = new Size(200, 140);
        btnMatribox.TabIndex = 2;
        btnMatribox.Text = "Configuração\nMatribox\n\nImportar .prst\nnomes e cadeias";
        btnMatribox.UseVisualStyleBackColor = false;
        btnMatribox.Click += btnMatribox_Click;
        // 
        // btnAi
        // 
        btnAi.BackColor = Color.FromArgb(52, 52, 64);
        btnAi.FlatAppearance.BorderSize = 0;
        btnAi.FlatStyle = FlatStyle.Flat;
        btnAi.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnAi.ForeColor = Color.White;
        btnAi.Location = new Point(40, 288);
        btnAi.Name = "btnAi";
        btnAi.Size = new Size(200, 88);
        btnAi.TabIndex = 3;
        btnAi.Text = "Prompt IA\n\nChatGPT / Claude / Gemini";
        btnAi.UseVisualStyleBackColor = false;
        btnAi.Click += btnAi_Click;
        // 
        // btnConfig
        // 
        btnConfig.BackColor = Color.FromArgb(72, 64, 56);
        btnConfig.FlatAppearance.BorderSize = 0;
        btnConfig.FlatStyle = FlatStyle.Flat;
        btnConfig.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnConfig.ForeColor = Color.White;
        btnConfig.Location = new Point(256, 288);
        btnConfig.Name = "btnConfig";
        btnConfig.Size = new Size(200, 88);
        btnConfig.TabIndex = 4;
        btnConfig.Text = "Configuração\n\nChaves IA e opções";
        btnConfig.UseVisualStyleBackColor = false;
        btnConfig.Click += btnConfig_Click;
        // 
        // lblHint
        // 
        lblHint.ForeColor = Color.FromArgb(90, 90, 90);
        lblHint.Location = new Point(40, 392);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(640, 48);
        lblHint.TabIndex = 0;
        lblHint.Text = "Na primeira execução abre a configuração. Podes alterar depois neste ecrã.\nChocolate × Matribox: mapeia os switches. Prompt IA: gera patches. Chocolate: switch U + USB-C de dados.";
        // 
        // FormLauncher
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 244, 238);
        ClientSize = new Size(720, 460);
        Controls.Add(lblHint);
        Controls.Add(btnConfig);
        Controls.Add(btnAi);
        Controls.Add(btnMatribox);
        Controls.Add(btnScene);
        Controls.Add(btnChocolate);
        Controls.Add(lblSub);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "FormLauncher";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chocolate Plus Editor";
        Shown += FormLauncher_Shown;
        ResumeLayout(false);
    }

    private Label lblTitle;
    private Label lblSub;
    private Button btnChocolate;
    private Button btnScene;
    private Button btnMatribox;
    private Button btnAi;
    private Button btnConfig;
    private Label lblHint;
}
