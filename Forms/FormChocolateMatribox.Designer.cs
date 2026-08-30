using ForeverButton = ReaLTaiizor.Controls.ForeverButton;
using ForeverToggle = ReaLTaiizor.Controls.ForeverToggle;

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
        panelTopActions = new FlowLayoutPanel();
        lblMidi = new Label();
        cmbChocolateMidi = new ComboBox();
        btnConnect = new ForeverButton();
        btnRead = new ForeverButton();
        btnWrite = new ForeverButton();
        lblChannel = new Label();
        numChannel = new NumericUpDown();
        btnInicio = new ForeverButton();
        lblTitle = new Label();
        panelBody = new Panel();
        panelChoco = new Panel();
        tabChoco = new Label();
        lblBanco = new Label();
        cmbChocoBank = new ComboBox();
        btnSwA = new ForeverButton();
        btnSwB = new ForeverButton();
        btnSwC = new ForeverButton();
        btnSwD = new ForeverButton();
        lblChocoHint = new Label();
        chkStomp = new ForeverToggle();
        lblStomp = new Label();
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
        flowPedals = new FlowLayoutPanel();
        panelTop.SuspendLayout();
        panelTopActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).BeginInit();
        panelBody.SuspendLayout();
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
        panelTop.Controls.Add(panelTopActions);
        panelTop.Controls.Add(lblTitle);
        panelTop.Controls.Add(btnInicio);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(16, 0, 0, 0);
        panelTop.Size = new Size(1180, 72);
        panelTop.TabIndex = 0;
        // 
        // panelTopActions
        // 
        panelTopActions.AutoSize = true;
        panelTopActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        panelTopActions.Controls.Add(lblMidi);
        panelTopActions.Controls.Add(cmbChocolateMidi);
        panelTopActions.Controls.Add(btnConnect);
        panelTopActions.Controls.Add(btnRead);
        panelTopActions.Controls.Add(btnWrite);
        panelTopActions.Controls.Add(lblChannel);
        panelTopActions.Controls.Add(numChannel);
        panelTopActions.Dock = DockStyle.Right;
        panelTopActions.Location = new Point(430, 0);
        panelTopActions.Name = "panelTopActions";
        panelTopActions.Padding = new Padding(8, 18, 16, 0);
        panelTopActions.Size = new Size(750, 72);
        panelTopActions.TabIndex = 2;
        panelTopActions.WrapContents = false;
        // 
        // lblMidi
        // 
        lblMidi.AutoSize = true;
        lblMidi.Location = new Point(11, 25);
        lblMidi.Margin = new Padding(3, 7, 6, 0);
        lblMidi.Name = "lblMidi";
        lblMidi.Size = new Size(33, 15);
        lblMidi.TabIndex = 0;
        lblMidi.Text = "MIDI";
        // 
        // cmbChocolateMidi
        // 
        cmbChocolateMidi.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbChocolateMidi.Location = new Point(53, 24);
        cmbChocolateMidi.Margin = new Padding(3, 6, 8, 0);
        cmbChocolateMidi.Name = "cmbChocolateMidi";
        cmbChocolateMidi.Size = new Size(188, 23);
        cmbChocolateMidi.TabIndex = 1;
        // 
        // btnConnect
        // 
        btnConnect.BaseColor = Color.FromArgb(36, 110, 176);
        btnConnect.Font = new Font("Segoe UI Semibold", 9.75F);
        btnConnect.Location = new Point(252, 18);
        btnConnect.Margin = new Padding(3, 0, 6, 0);
        btnConnect.Name = "btnConnect";
        btnConnect.Rounded = true;
        btnConnect.Size = new Size(108, 36);
        btnConnect.TabIndex = 2;
        btnConnect.Text = "Conectar";
        btnConnect.TextColor = Color.White;
        btnConnect.Click += btnConnect_Click;
        // 
        // btnRead
        // 
        btnRead.BaseColor = Color.FromArgb(40, 40, 46);
        btnRead.Font = new Font("Segoe UI Semibold", 9.75F);
        btnRead.Location = new Point(369, 18);
        btnRead.Margin = new Padding(3, 0, 6, 0);
        btnRead.Name = "btnRead";
        btnRead.Rounded = true;
        btnRead.Size = new Size(88, 36);
        btnRead.TabIndex = 3;
        btnRead.Text = "Ler";
        btnRead.TextColor = Color.White;
        btnRead.Click += btnRead_Click;
        // 
        // btnWrite
        // 
        btnWrite.BaseColor = Color.FromArgb(40, 40, 46);
        btnWrite.Font = new Font("Segoe UI Semibold", 9.75F);
        btnWrite.Location = new Point(466, 18);
        btnWrite.Margin = new Padding(3, 0, 8, 0);
        btnWrite.Name = "btnWrite";
        btnWrite.Rounded = true;
        btnWrite.Size = new Size(168, 36);
        btnWrite.TabIndex = 4;
        btnWrite.Text = "Gravar no Chocolate";
        btnWrite.TextColor = Color.White;
        btnWrite.Click += btnWrite_Click;
        // 
        // lblChannel
        // 
        lblChannel.AutoSize = true;
        lblChannel.Location = new Point(645, 25);
        lblChannel.Margin = new Padding(3, 7, 4, 0);
        lblChannel.Name = "lblChannel";
        lblChannel.Size = new Size(26, 15);
        lblChannel.TabIndex = 5;
        lblChannel.Text = "Ch";
        // 
        // numChannel
        // 
        numChannel.Location = new Point(678, 23);
        numChannel.Margin = new Padding(3, 5, 0, 0);
        numChannel.Maximum = 16;
        numChannel.Minimum = 1;
        numChannel.Name = "numChannel";
        numChannel.Size = new Size(48, 23);
        numChannel.TabIndex = 6;
        numChannel.Value = 1;
        numChannel.ValueChanged += numChannel_ValueChanged;
        // 
        // btnInicio
        // 
        btnInicio.BaseColor = Color.FromArgb(40, 40, 46);
        btnInicio.Font = new Font("Segoe UI Semibold", 9.75F);
        btnInicio.Location = new Point(16, 18);
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
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.Location = new Point(116, 22);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(220, 25);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Chocolate × Matribox";
        // 
        // panelBody
        // 
        panelBody.Controls.Add(panelMatribox);
        panelBody.Controls.Add(panelChoco);
        panelBody.Dock = DockStyle.Fill;
        panelBody.Location = new Point(0, 72);
        panelBody.Name = "panelBody";
        panelBody.Padding = new Padding(16, 8, 16, 16);
        panelBody.Size = new Size(1180, 748);
        panelBody.TabIndex = 1;
        // 
        // panelChoco
        // 
        panelChoco.Controls.Add(lblStomp);
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
        panelChoco.Location = new Point(16, 8);
        panelChoco.Name = "panelChoco";
        panelChoco.Padding = new Padding(20, 16, 20, 12);
        panelChoco.Size = new Size(1148, 168);
        panelChoco.TabIndex = 0;
        // 
        // tabChoco
        // 
        tabChoco.AutoSize = true;
        tabChoco.Font = new Font("Segoe UI Semibold", 11F);
        tabChoco.Location = new Point(20, 12);
        tabChoco.Name = "tabChoco";
        tabChoco.Size = new Size(86, 20);
        tabChoco.TabIndex = 0;
        tabChoco.Text = "Chocolate";
        // 
        // lblBanco
        // 
        lblBanco.AutoSize = true;
        lblBanco.Location = new Point(20, 48);
        lblBanco.Name = "lblBanco";
        lblBanco.Size = new Size(42, 15);
        lblBanco.TabIndex = 1;
        lblBanco.Text = "Banco";
        // 
        // cmbChocoBank
        // 
        cmbChocoBank.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbChocoBank.Location = new Point(72, 44);
        cmbChocoBank.Name = "cmbChocoBank";
        cmbChocoBank.Size = new Size(240, 23);
        cmbChocoBank.TabIndex = 2;
        cmbChocoBank.SelectedIndexChanged += cmbChocoBank_SelectedIndexChanged;
        // 
        // btnSwA
        // 
        btnSwA.BaseColor = Color.FromArgb(40, 40, 46);
        btnSwA.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwA.Location = new Point(20, 84);
        btnSwA.Name = "btnSwA";
        btnSwA.Rounded = true;
        btnSwA.Size = new Size(76, 48);
        btnSwA.TabIndex = 3;
        btnSwA.Text = "A";
        btnSwA.TextColor = Color.White;
        btnSwA.Click += btnSwitch_Click;
        // 
        // btnSwB
        // 
        btnSwB.BaseColor = Color.FromArgb(40, 40, 46);
        btnSwB.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwB.Location = new Point(104, 84);
        btnSwB.Name = "btnSwB";
        btnSwB.Rounded = true;
        btnSwB.Size = new Size(76, 48);
        btnSwB.TabIndex = 4;
        btnSwB.Text = "B";
        btnSwB.TextColor = Color.White;
        btnSwB.Click += btnSwitch_Click;
        // 
        // btnSwC
        // 
        btnSwC.BaseColor = Color.FromArgb(40, 40, 46);
        btnSwC.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwC.Location = new Point(188, 84);
        btnSwC.Name = "btnSwC";
        btnSwC.Rounded = true;
        btnSwC.Size = new Size(76, 48);
        btnSwC.TabIndex = 5;
        btnSwC.Text = "C";
        btnSwC.TextColor = Color.White;
        btnSwC.Click += btnSwitch_Click;
        // 
        // btnSwD
        // 
        btnSwD.BaseColor = Color.FromArgb(40, 40, 46);
        btnSwD.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnSwD.Location = new Point(272, 84);
        btnSwD.Name = "btnSwD";
        btnSwD.Rounded = true;
        btnSwD.Size = new Size(76, 48);
        btnSwD.TabIndex = 6;
        btnSwD.Text = "D";
        btnSwD.TextColor = Color.White;
        btnSwD.Click += btnSwitch_Click;
        // 
        // lblChocoHint
        // 
        lblChocoHint.AutoSize = true;
        lblChocoHint.Location = new Point(364, 98);
        lblChocoHint.MaximumSize = new Size(720, 40);
        lblChocoHint.Name = "lblChocoHint";
        lblChocoHint.Size = new Size(400, 15);
        lblChocoHint.TabIndex = 7;
        lblChocoHint.Text = "A · curto manda efeitos; A · longo escolhe o patch. B/C/D · curto liga/desliga efeitos.";
        // 
        // chkStomp
        // 
        chkStomp.BaseColor = Color.FromArgb(36, 110, 176);
        chkStomp.Location = new Point(332, 40);
        chkStomp.Name = "chkStomp";
        chkStomp.Options = ForeverToggle._Options.Style3;
        chkStomp.Size = new Size(76, 33);
        chkStomp.TabIndex = 8;
        chkStomp.CheckedChanged += chkStomp_CheckedChanged;
        // 
        // lblStomp
        // 
        lblStomp.AutoSize = true;
        lblStomp.Location = new Point(416, 48);
        lblStomp.Name = "lblStomp";
        lblStomp.Size = new Size(220, 15);
        lblStomp.TabIndex = 9;
        lblStomp.Text = "Stomp Mode (CC 29) no toque curto do A";
        // 
        // panelMatribox
        // 
        panelMatribox.Controls.Add(splitMatribox);
        panelMatribox.Controls.Add(tabMatribox);
        panelMatribox.Dock = DockStyle.Fill;
        panelMatribox.Location = new Point(16, 176);
        panelMatribox.Name = "panelMatribox";
        panelMatribox.Padding = new Padding(0, 28, 0, 0);
        panelMatribox.Size = new Size(1148, 556);
        panelMatribox.TabIndex = 1;
        // 
        // tabMatribox
        // 
        tabMatribox.AutoSize = true;
        tabMatribox.Font = new Font("Segoe UI Semibold", 11F);
        tabMatribox.Location = new Point(4, 4);
        tabMatribox.Name = "tabMatribox";
        tabMatribox.Size = new Size(122, 20);
        tabMatribox.TabIndex = 0;
        tabMatribox.Text = "Matribox II Pro";
        // 
        // splitMatribox
        // 
        splitMatribox.Dock = DockStyle.Fill;
        splitMatribox.Location = new Point(0, 28);
        splitMatribox.Name = "splitMatribox";
        // 
        // splitMatribox.Panel1
        // 
        splitMatribox.Panel1.Controls.Add(panelBancos);
        splitMatribox.Panel1.Padding = new Padding(0, 0, 6, 0);
        splitMatribox.Panel1MinSize = 260;
        // 
        // splitMatribox.Panel2
        // 
        splitMatribox.Panel2.Controls.Add(panelFx);
        splitMatribox.Panel2.Padding = new Padding(6, 0, 0, 0);
        splitMatribox.Panel2MinSize = 280;
        splitMatribox.Size = new Size(1148, 528);
        splitMatribox.SplitterDistance = 400;
        splitMatribox.SplitterWidth = 8;
        splitMatribox.TabIndex = 1;
        // 
        // panelBancos
        // 
        panelBancos.Controls.Add(lstPatches);
        panelBancos.Controls.Add(tabBancos);
        panelBancos.Dock = DockStyle.Fill;
        panelBancos.Location = new Point(0, 0);
        panelBancos.Name = "panelBancos";
        panelBancos.Padding = new Padding(12, 36, 12, 12);
        panelBancos.Size = new Size(394, 528);
        panelBancos.TabIndex = 0;
        // 
        // tabBancos
        // 
        tabBancos.AutoSize = true;
        tabBancos.Font = new Font("Segoe UI Semibold", 10F);
        tabBancos.Location = new Point(16, 10);
        tabBancos.Name = "tabBancos";
        tabBancos.Size = new Size(54, 19);
        tabBancos.TabIndex = 0;
        tabBancos.Text = "Bancos";
        // 
        // lstPatches
        // 
        lstPatches.BorderStyle = BorderStyle.None;
        lstPatches.Dock = DockStyle.Fill;
        lstPatches.Font = new Font("Segoe UI", 10F);
        lstPatches.IntegralHeight = false;
        lstPatches.ItemHeight = 28;
        lstPatches.Location = new Point(12, 36);
        lstPatches.Name = "lstPatches";
        lstPatches.Size = new Size(370, 480);
        lstPatches.TabIndex = 1;
        lstPatches.SelectedIndexChanged += lstPatches_SelectedIndexChanged;
        // 
        // panelFx
        // 
        panelFx.Controls.Add(panelFxScroll);
        panelFx.Controls.Add(lblFxHint);
        panelFx.Controls.Add(tabFx);
        panelFx.Dock = DockStyle.Fill;
        panelFx.Location = new Point(6, 0);
        panelFx.Name = "panelFx";
        panelFx.Padding = new Padding(12, 36, 12, 12);
        panelFx.Size = new Size(734, 528);
        panelFx.TabIndex = 0;
        // 
        // tabFx
        // 
        tabFx.AutoSize = true;
        tabFx.Font = new Font("Segoe UI Semibold", 10F);
        tabFx.Location = new Point(16, 10);
        tabFx.Name = "tabFx";
        tabFx.Size = new Size(52, 19);
        tabFx.TabIndex = 0;
        tabFx.Text = "Efeitos";
        // 
        // lblFxHint
        // 
        lblFxHint.Dock = DockStyle.Top;
        lblFxHint.Location = new Point(12, 36);
        lblFxHint.Name = "lblFxHint";
        lblFxHint.Size = new Size(710, 32);
        lblFxHint.TabIndex = 1;
        lblFxHint.Text = "Clica no pedal para ligar ou desligar o efeito no toque curto do Chocolate. ON = o efeito fica ligado.";
        // 
        // panelFxScroll
        // 
        panelFxScroll.AutoScroll = true;
        panelFxScroll.Controls.Add(flowPedals);
        panelFxScroll.Dock = DockStyle.Fill;
        panelFxScroll.Location = new Point(12, 68);
        panelFxScroll.Name = "panelFxScroll";
        panelFxScroll.Size = new Size(710, 448);
        panelFxScroll.TabIndex = 2;
        // 
        // flowPedals
        // 
        flowPedals.AutoScroll = false;
        flowPedals.AutoSize = true;
        flowPedals.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowPedals.Dock = DockStyle.Top;
        flowPedals.Location = new Point(0, 0);
        flowPedals.Name = "flowPedals";
        flowPedals.Padding = new Padding(8);
        flowPedals.Size = new Size(710, 180);
        flowPedals.TabIndex = 0;
        flowPedals.WrapContents = true;
        // 
        // FormChocolateMatribox
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 820);
        Controls.Add(panelBody);
        Controls.Add(panelTop);
        MinimumSize = new Size(1120, 720);
        Name = "FormChocolateMatribox";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chocolate × Matribox";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelTopActions.ResumeLayout(false);
        panelTopActions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numChannel).EndInit();
        panelBody.ResumeLayout(false);
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
    private FlowLayoutPanel panelTopActions;
    private Label lblMidi;
    private ForeverButton btnInicio;
    private Label lblTitle;
    private ComboBox cmbChocolateMidi;
    private ForeverButton btnConnect;
    private ForeverButton btnRead;
    private ForeverButton btnWrite;
    private Label lblChannel;
    private NumericUpDown numChannel;
    private Panel panelBody;
    private Panel panelChoco;
    private Label tabChoco;
    private Label lblBanco;
    private ComboBox cmbChocoBank;
    private ForeverButton btnSwA;
    private ForeverButton btnSwB;
    private ForeverButton btnSwC;
    private ForeverButton btnSwD;
    private Label lblChocoHint;
    private ForeverToggle chkStomp;
    private Label lblStomp;
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
    private FlowLayoutPanel flowPedals;
}
