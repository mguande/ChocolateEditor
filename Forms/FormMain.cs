using ChocolateEditor.Controls;
using ChocolateEditor.Models;
using ChocolateEditor.Protocol;

using ChocolateEditor.Ui;

namespace ChocolateEditor.Forms;

public partial class FormMain : DarkForm
{
    private readonly ChocolateUsbClient _usb = new();
    private DeviceConfiguration _config = DeviceConfiguration.CreateStarter();
    private string? _filePath;
    private bool _loading;
    private bool _dirty;
    private int _currentBank;

    public FormMain()
    {
        InitializeComponent();
        var mnuInicio = new ToolStripMenuItem("← Início");
        mnuInicio.Click += (_, _) => Close();
        menuStrip.Items.Insert(0, mnuInicio);
        _usb.Log += (_, line) => BeginInvoke(() => AppendLog(line));
        LoadModes();
        RefreshMidiPorts();
        BindConfig(DeviceConfiguration.CreateStarter(), dirty: false);
        Shown += (_, _) => AppendLog("Ligue o Chocolate (switch U), USB-C com cabo de dados, e clique Conectar.");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _usb.Dispose();
        base.OnFormClosed(e);
    }

    private void LoadModes()
    {
        cmbMode.Items.Clear();
        foreach (DeviceMode mode in Enum.GetValues<DeviceMode>())
        {
            cmbMode.Items.Add(UiLabels.Mode(mode));
        }
    }

    private void BindConfig(DeviceConfiguration config, bool dirty)
    {
        _loading = true;
        _config = config;
        _config.EnsureBankCount();
        cmbMode.SelectedIndex = IndexOfMode(_config.Mode);
        numBankCount.Value = _config.BankCount;
        cmbTrs.SelectedIndex = _config.TrsMode == TrsJackMode.Midi ? 1 : 0;
        RebuildBankList();
        lstBanks.SelectedIndex = 0;
        LoadCurrentBankToUi();
        _loading = false;
        SetDirty(dirty);
    }

    private static int IndexOfMode(DeviceMode mode)
    {
        var values = Enum.GetValues<DeviceMode>();
        int i = Array.IndexOf(values, mode);
        return i < 0 ? Array.IndexOf(values, DeviceMode.AdvancedCustom) : i;
    }

    private void RebuildBankList()
    {
        int selected = lstBanks.SelectedIndex;
        lstBanks.Items.Clear();
        foreach (var bank in _config.Banks.Take(_config.BankCount))
        {
            lstBanks.Items.Add(bank.ListLabel);
        }

        if (selected >= 0 && selected < lstBanks.Items.Count)
        {
            lstBanks.SelectedIndex = selected;
        }
    }

    private void SaveUiToCurrentBank()
    {
        if (_loading)
        {
            return;
        }

        var bank = _config.GetBank(_currentBank);
        bank.Name = string.IsNullOrWhiteSpace(txtBankName.Text) ? $"Banco {_currentBank + 1}" : txtBankName.Text.Trim();
        switchA.SaveTo(bank.A);
        switchB.SaveTo(bank.B);
        switchC.SaveTo(bank.C);
        switchD.SaveTo(bank.D);
        _config.Mode = Enum.GetValues<DeviceMode>()[Math.Max(0, cmbMode.SelectedIndex)];
        _config.BankCount = (int)numBankCount.Value;
        _config.TrsMode = cmbTrs.SelectedIndex == 1 ? TrsJackMode.Midi : TrsJackMode.Expression;
    }

    private void LoadCurrentBankToUi()
    {
        _loading = true;
        var bank = _config.GetBank(_currentBank);
        txtBankName.Text = bank.Name;
        switchA.LoadFrom(bank.A);
        switchB.LoadFrom(bank.B);
        switchC.LoadFrom(bank.C);
        switchD.LoadFrom(bank.D);
        lblBankTitle.Text = $"Banco {_currentBank + 1} de {_config.BankCount}  (E/F no pedal mudam de banco)";
        _loading = false;
    }

    private void SetDirty(bool dirty)
    {
        _dirty = dirty;
        Text = (_dirty ? "• " : "") + "Chocolate Plus Editor" + (_filePath is null ? "" : " — " + Path.GetFileName(_filePath));
        lblStatus.Text = _usb.IsConnected
            ? "USB: " + _usb.ConnectedName
            : "USB: desconectado";
        UiTheme.StyleAction(btnWrite, dirty ? UiTheme.Accent : UiTheme.Surface2);
        btnWrite.Text = dirty ? "Gravar no Chocolate •" : "Gravar no Chocolate";
    }

    private void StyleConnect()
    {
        if (_usb.IsConnected)
        {
            btnConnect.Text = "Desconectar";
            UiTheme.StyleAction(btnConnect, UiTheme.Accent);
        }
        else
        {
            btnConnect.Text = "Conectar";
            UiTheme.StyleAction(btnConnect, UiTheme.Blue);
        }
    }

    private void MarkDirty()
    {
        if (!_loading)
        {
            SetDirty(true);
        }
    }

    private void lstBanks_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loading || lstBanks.SelectedIndex < 0)
        {
            return;
        }

        SaveUiToCurrentBank();
        _currentBank = lstBanks.SelectedIndex;
        LoadCurrentBankToUi();
    }

    private void txtBankName_TextChanged(object sender, EventArgs e)
    {
        if (_loading)
        {
            return;
        }

        _config.GetBank(_currentBank).Name = txtBankName.Text;
        MarkDirty();
        int i = lstBanks.SelectedIndex;
        if (i >= 0)
        {
            lstBanks.Items[i] = _config.GetBank(i).ListLabel;
        }
    }

    private void ControlChanged(object sender, EventArgs e) => MarkDirty();

    private void mnuNovo_Click(object sender, EventArgs e)
    {
        if (!ConfirmDiscard())
        {
            return;
        }

        _filePath = null;
        BindConfig(DeviceConfiguration.CreateStarter(), dirty: false);
        AppendLog("Novo conjunto a partir do banco modelo (PC 0–63 + CC 80–83 no longo).");
    }

    private void mnuAbrir_Click(object sender, EventArgs e)
    {
        if (!ConfirmDiscard())
        {
            return;
        }

        using var dlg = new OpenFileDialog { Filter = DeviceConfiguration.FileFilter };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            BindConfig(DeviceConfiguration.LoadFromFile(dlg.FileName), dirty: false);
            _filePath = dlg.FileName;
            SetDirty(false);
            AppendLog("Aberto: " + dlg.FileName);
        }
    }

    private void mnuSalvar_Click(object sender, EventArgs e) => Save(false);

    private void mnuSalvarComo_Click(object sender, EventArgs e) => Save(true);

    private void Save(bool saveAs)
    {
        SaveUiToCurrentBank();
        if (saveAs || _filePath is null)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = DeviceConfiguration.FileFilter,
                FileName = "chocolate-bancos.choco.json"
            };
            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _filePath = dlg.FileName;
        }

        _config.SaveToFile(_filePath);
        SetDirty(false);
        AppendLog("Salvo: " + _filePath);
    }

    private void mnuSair_Click(object sender, EventArgs e) => Close();

    private void mnuCopiarBanco_Click(object sender, EventArgs e)
    {
        SaveUiToCurrentBank();
        using var dlg = new FormCopyBank(_config, _currentBank);
        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (dlg.CopyRange)
        {
            _config.CopyBankToRange(dlg.FromIndex, dlg.ToIndex, dlg.ToEndIndex,
                dlg.CopyA, dlg.CopyB, dlg.CopyC, dlg.CopyD, dlg.CopyShort, dlg.CopyLong);
        }
        else
        {
            _config.CopyBank(dlg.FromIndex, dlg.ToIndex,
                dlg.CopyA, dlg.CopyB, dlg.CopyC, dlg.CopyD, dlg.CopyShort, dlg.CopyLong);
        }

        RebuildBankList();
        lstBanks.SelectedIndex = dlg.CopyRange ? dlg.ToIndex : dlg.ToIndex;
        _currentBank = lstBanks.SelectedIndex;
        LoadCurrentBankToUi();
        SetDirty(true);
        AppendLog($"Banco {dlg.FromIndex + 1} copiado.");
    }

    private void btnRefreshMidi_Click(object sender, EventArgs e) => RefreshMidiPorts();

    private void RefreshMidiPorts()
    {
        string? previous = cmbMidiDevice.SelectedItem as string;
        cmbMidiDevice.Items.Clear();
        var ports = ChocolateUsbClient.ListPorts().ToList();
        foreach (var port in ports)
        {
            cmbMidiDevice.Items.Add(port.Name);
        }

        int hint = ports.FindIndex(p => p.LooksLikeChocolate);
        if (previous is not null)
        {
            int i = cmbMidiDevice.Items.IndexOf(previous);
            cmbMidiDevice.SelectedIndex = i >= 0 ? i : Math.Max(hint, 0);
        }
        else if (cmbMidiDevice.Items.Count > 0)
        {
            cmbMidiDevice.SelectedIndex = hint >= 0 ? hint : 0;
        }

        lblMidiHint.Text = ports.Count == 0
            ? "Nenhum MIDI USB. Cabo de dados + switch U."
            : hint >= 0 ? "Chocolate detectado." : "Selecione SINCO / FootCtrl / Chocolate.";
    }

    private async void btnConnect_Click(object sender, EventArgs e)
    {
        if (_usb.IsConnected)
        {
            _usb.Disconnect();
            StyleConnect();
            SetDirty(_dirty);
            return;
        }

        if (cmbMidiDevice.SelectedItem is not string name)
        {
            MessageBox.Show(this, "Selecione o dispositivo MIDI USB.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            UseWaitCursor = true;
            _usb.Connect(name);
            string? id = await _usb.HandshakeAsync();
            StyleConnect();
            SetDirty(_dirty);
            AppendLog("Conectado" + (id is null ? "." : ": " + id) + "  (se o handshake falhar, ainda assim tente Ler/Gravar)");
        }
        catch (Exception ex)
        {
            _usb.Disconnect();
            StyleConnect();
            MessageBox.Show(this, ex.Message, "USB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private async void btnRead_Click(object sender, EventArgs e)
    {
        if (!ConfirmDiscard())
        {
            return;
        }

        if (!_usb.IsConnected)
        {
            MessageBox.Show(this, "Conecte o Chocolate primeiro (USB-C, switch U).", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            UseWaitCursor = true;
            var config = await _usb.ReadConfigurationAsync();
            _filePath = null;
            BindConfig(config, dirty: false);
            int n = ChocolateMemoryMap.CountMessages(config);
            AppendLog($"Modo lido: {UiLabels.Mode(config.Mode)}");
            AppendLog(n == 0
                ? "Leitura USB ok, mas não achei listas MIDI no formato CubeSuite. O dump está em " + AppConfig.File("last-dump.bin") + "."
                : $"Leitura USB: {n} mensagens. Banco 1 = Switches A–D; os outros bancos vêm a seguir no dump.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ler do Chocolate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private async void btnWrite_Click(object sender, EventArgs e)
    {
        SaveUiToCurrentBank();
        if (!_usb.IsConnected)
        {
            MessageBox.Show(this, "Conecte o Chocolate primeiro (USB-C, switch U).", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show(this,
                "Gravar estes bancos no Chocolate Plus?\n\nO pedal precisa estar em U (USB device), cabo Type-C com dados.",
                "Gravar no Chocolate", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        {
            return;
        }

        try
        {
            UseWaitCursor = true;
            await _usb.WriteConfigurationAsync(_config);
            AppendLog("Gravação USB concluída. Troque de banco no pedal (A+B / C+D) para conferir.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Gravar no Chocolate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private bool ConfirmDiscard()
    {
        if (!_dirty)
        {
            return true;
        }

        var r = MessageBox.Show(this, "Há alterações não salvas. Continuar?", Text,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return r == DialogResult.Yes;
    }

    private void AppendLog(string line)
    {
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!ConfirmDiscard())
        {
            e.Cancel = true;
        }

        base.OnFormClosing(e);
    }
}
