using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

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
        btnChocolate = new ForeverButton();
        btnScene = new ForeverButton();
        btnMatribox = new ForeverButton();
        btnAi = new ForeverButton();
        btnConfig = new ForeverButton();
        lblHint = new Label();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(40, 28);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(640, 44);
        lblTitle.TabIndex = 3;
        lblTitle.Text = "Chocolate Plus Editor";
        // 
        // lblSub
        // 
        lblSub.Font = new Font("Segoe UI", 11F);
        lblSub.ForeColor = Color.FromArgb(168, 168, 176);
        lblSub.Location = new Point(40, 76);
        lblSub.Name = "lblSub";
        lblSub.Size = new Size(640, 40);
        lblSub.TabIndex = 2;
        lblSub.Text = "Escolhe o que queres editar. O Chocolate controla a Matribox por MIDI.";
        // 
        // btnChocolate
        // 
        btnChocolate.BaseColor = Color.FromArgb(168, 78, 36);
        btnChocolate.Font = new Font("Segoe UI Semibold", 11F);
        btnChocolate.Location = new Point(40, 132);
        btnChocolate.Name = "btnChocolate";
        btnChocolate.Rounded = true;
        btnChocolate.Size = new Size(200, 140);
        btnChocolate.TabIndex = 0;
        btnChocolate.Text = "Chocolate Plus\n\nEditor USB\nbancos e MIDI";
        btnChocolate.TextColor = Color.White;
        btnChocolate.Click += btnChocolate_Click;
        // 
        // btnScene
        // 
        btnScene.BaseColor = Color.FromArgb(36, 110, 176);
        btnScene.Font = new Font("Segoe UI Semibold", 11F);
        btnScene.Location = new Point(256, 132);
        btnScene.Name = "btnScene";
        btnScene.Rounded = true;
        btnScene.Size = new Size(200, 140);
        btnScene.TabIndex = 1;
        btnScene.Text = "Chocolate × Matribox\n\nSwitch A = patch\nB/C/D = efeitos";
        btnScene.TextColor = Color.White;
        btnScene.Click += btnScene_Click;
        // 
        // btnMatribox
        // 
        btnMatribox.BaseColor = Color.FromArgb(24, 120, 118);
        btnMatribox.Font = new Font("Segoe UI Semibold", 11F);
        btnMatribox.Location = new Point(472, 132);
        btnMatribox.Name = "btnMatribox";
        btnMatribox.Rounded = true;
        btnMatribox.Size = new Size(200, 140);
        btnMatribox.TabIndex = 2;
        btnMatribox.Text = "Configuração\nMatribox\n\nImportar .prst\nnomes e cadeias";
        btnMatribox.TextColor = Color.White;
        btnMatribox.Click += btnMatribox_Click;
        // 
        // btnAi
        // 
        btnAi.BaseColor = Color.FromArgb(58, 58, 78);
        btnAi.Font = new Font("Segoe UI Semibold", 11F);
        btnAi.Location = new Point(40, 288);
        btnAi.Name = "btnAi";
        btnAi.Rounded = true;
        btnAi.Size = new Size(200, 88);
        btnAi.TabIndex = 3;
        btnAi.Text = "Prompt IA\n\nChatGPT / Claude / Gemini";
        btnAi.TextColor = Color.White;
        btnAi.Click += btnAi_Click;
        // 
        // btnConfig
        // 
        btnConfig.BaseColor = Color.FromArgb(78, 68, 58);
        btnConfig.Font = new Font("Segoe UI Semibold", 11F);
        btnConfig.Location = new Point(256, 288);
        btnConfig.Name = "btnConfig";
        btnConfig.Rounded = true;
        btnConfig.Size = new Size(200, 88);
        btnConfig.TabIndex = 4;
        btnConfig.Text = "Configuração\n\nChaves IA e opções";
        btnConfig.TextColor = Color.White;
        btnConfig.Click += btnConfig_Click;
        // 
        // lblHint
        // 
        lblHint.ForeColor = Color.FromArgb(168, 168, 176);
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
        BackColor = Color.FromArgb(18, 18, 20);
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
    private ForeverButton btnChocolate;
    private ForeverButton btnScene;
    private ForeverButton btnMatribox;
    private ForeverButton btnAi;
    private ForeverButton btnConfig;
    private Label lblHint;
}
