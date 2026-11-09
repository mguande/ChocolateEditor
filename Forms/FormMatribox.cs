using ChocolateEditor.Matribox;
using ChocolateEditor.Models;
using ChocolateEditor.Protocol;

namespace ChocolateEditor.Forms;

public partial class FormMatribox : Form
{
    private readonly ChocolateUsbClient _chocolate = new();
    private readonly MatriboxMidiClient _matribox = new();
    private DeviceConfiguration _config = DeviceConfiguration.CreateStarter();
    private ComboBox[] _fx = [];
    private Label[] _fxLabels = [];
    private bool _loadingTree;
    private bool _busy;

    public FormMatribox()
    {
        InitializeComponent();
        _chocolate.Log += (_, line) => BeginInvoke(() => Log(line));
        _matribox.Log += (_, line) => BeginInvoke(() => Log(line));
        _matribox.SnapshotChanged += (_, _) => BeginInvoke(OnMatriboxSnapshot);
        _matribox.Progress += (_, p) => BeginInvoke(() => ShowProgress(p.Current, p.Total, p.Message));
        BuildEffectRows();
        LoadLocalPresets();
        FillMatriboxTree();
        RefreshPorts();
        RebuildChocolateGroups();
        cmbSwitch.Items.AddRange(["Switch A", "Switch B", "Switch C", "Switch D"]);
        cmbSwitch.SelectedIndex = 0;
        cmbSwitch.SelectedIndexChanged += (_, _) => RefreshPreview();
        lstChoco.SelectedIndexChanged += (_, _) => RefreshPreview();
        numChannel.ValueChanged += (_, _) => _matribox.Channel = (int)numChannel.Value;
        chkStomp.Checked = true;
        ClientSize = new Size(1100, 800);
        Shown += (_, _) =>
        {
            if (_matribox.Presets.Any(p => !MatriboxPresetStore.HasChain(p.Chain)))
            {
                string? folder = MatriboxPrstImport.FirstExistingDefaultFolder();
                if (folder is not null)
                {
                    ImportPrstFolder(folder, quiet: true);
                }
            }

            if (!File.Exists(MatriboxPresetStore.FilePath))
            {
                Log("Liga a Matribox por USB. Global → MIDI → MIDI In Source = USB ou Mixed. Fecha o editor oficial da Sonicake.");
            }
        };
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        SaveLocalPresets();
        _chocolate.Dispose();
        _matribox.Dispose();
        base.OnFormClosed(e);
    }

    private void LoadLocalPresets()
    {
        var list = MatriboxPresetStore.Load();
        _matribox.ReplacePresets(list);
        int named = list.Count(MatriboxPresetStore.HasUsefulData);
        if (File.Exists(MatriboxPresetStore.FilePath))
        {
            Log(named == 0
                ? "Banco local sem nomes. A leitura MIDI não os preenche — duplo clique na árvore."
                : $"Lista local: {named} patches com nome ou cadeia. {MatriboxPresetStore.FilePath}");
        }
    }

    private void SaveLocalPresets()
    {
        try
        {
            MatriboxPresetStore.Save(_matribox.Presets);
        }
        catch (Exception ex)
        {
            Log("Não gravou a lista local: " + ex.Message);
        }
    }

    private void BuildEffectRows()
    {
        _fx = new ComboBox[MatriboxMidi.ModuleNames.Length];
        _fxLabels = new Label[MatriboxMidi.ModuleNames.Length];
        tableFx.RowCount = MatriboxMidi.ModuleNames.Length;
        tableFx.ColumnCount = 2;
        tableFx.ColumnStyles.Clear();
        tableFx.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        tableFx.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        tableFx.RowStyles.Clear();
        for (int i = 0; i < _fx.Length; i++)
        {
            tableFx.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            var lbl = new Label
            {
                Text = $"--  (CC {MatriboxMidi.ModuleCc(i)})",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            cmb.Items.AddRange(["Off", "On"]);
            cmb.SelectedIndex = 0;
            cmb.SelectedIndexChanged += (_, _) => RefreshPreview();
            _fx[i] = cmb;
            _fxLabels[i] = lbl;
            tableFx.Controls.Add(lbl, 0, i);
            tableFx.Controls.Add(cmb, 1, i);
        }

        tableFx.Height = _fx.Length * 28;
    }

    private void FillMatriboxTree()
    {
        (int bank, int slot)? keep = treePresets.SelectedNode?.Tag is ValueTuple<int, int> t ? t : null;
        _loadingTree = true;
        treePresets.BeginUpdate();
        treePresets.Nodes.Clear();
        var dumped = _matribox.Presets;
        TreeNode? restore = null;
        for (int bank = 1; bank <= MatriboxMidi.BankCount; bank++)
        {
            var node = new TreeNode($"Banco {bank:00}") { Tag = bank };
            for (int slot = 0; slot < MatriboxMidi.PresetsPerBank; slot++)
            {
                int index = (bank - 1) * 4 + slot;
                string title = MatriboxMidi.PresetLabel(bank, slot);
                if (index < dumped.Count && !string.IsNullOrWhiteSpace(dumped[index].Name))
                {
                    title = dumped[index].Display;
                }

                var child = new TreeNode(title) { Tag = (bank, slot) };
                node.Nodes.Add(child);
                if (keep is { } k && k.bank == bank && k.slot == slot)
                {
                    restore = child;
                }
            }

            treePresets.Nodes.Add(node);
        }

        treePresets.EndUpdate();
        if (restore is not null)
        {
            restore.Parent?.Expand();
            treePresets.SelectedNode = restore;
        }
        else
        {
            treePresets.Nodes[0].Expand();
            treePresets.SelectedNode = treePresets.Nodes[0].Nodes[0];
        }

        _loadingTree = false;
    }

    private void OnMatriboxSnapshot()
    {
        FillMatriboxTree();
        if (_matribox.CurrentChain.Count > 0)
        {
            ApplyChainToUi(_matribox.CurrentChain);
        }
    }

    private void ApplyChainToUi(IReadOnlyList<MatriboxChainSlot> chain)
    {
        for (int i = 0; i < _fxLabels.Length; i++)
        {
            if (i < chain.Count && !chain[i].Empty)
            {
                _fxLabels[i].Text = chain[i].Label;
                _fx[i].SelectedIndex = chain[i].On ? 1 : 0;
            }
            else
            {
                _fxLabels[i].Text = $"--  (CC {MatriboxMidi.ModuleCc(i)})";
                _fx[i].SelectedIndex = 0;
            }
        }

        tableFx.Height = _fx.Length * 28;
        RefreshPreview();
    }

    private void RefreshPorts()
    {
        var ports = ChocolateUsbClient.ListPorts();
        FillCombo(cmbMatriboxMidi, ports, p => MatriboxMidiClient.LooksLikeMatribox(p.Name));
        FillCombo(cmbChocolateMidi, ports, p => p.LooksLikeChocolate);
        lblMatriboxHint.Text = cmbMatriboxMidi.Items.Count == 0
            ? "Nenhum MIDI. USB na Matribox + Chocolate."
            : "Matribox IN+OUT. Fecha o editor Sonicake. MIDI In = USB/Mixed.";
    }

    private static void FillCombo(ComboBox combo, IReadOnlyList<MidiPortInfo> ports, Func<MidiPortInfo, bool> hint)
    {
        string? previous = combo.SelectedItem as string;
        combo.Items.Clear();
        int preferred = -1;
        foreach (var port in ports.Where(p => p.IsOutput))
        {
            int i = combo.Items.Add(port.Name);
            if (preferred < 0 && hint(port))
            {
                preferred = i;
            }
        }

        if (previous is not null && combo.Items.Contains(previous))
        {
            combo.SelectedItem = previous;
        }
        else if (combo.Items.Count > 0)
        {
            combo.SelectedIndex = preferred >= 0 ? preferred : 0;
        }
    }

    private void RebuildChocolateGroups()
    {
        int selected = lstChoco.SelectedIndex;
        lstChoco.Items.Clear();
        _config.EnsureBankCount();
        foreach (var bank in _config.Banks.Take(_config.BankCount))
        {
            lstChoco.Items.Add(bank.ListLabel);
        }

        lstChoco.SelectedIndex = selected >= 0 && selected < lstChoco.Items.Count ? selected : 0;
    }

    private bool TrySelection(out int chocoGroup, out int switchIndex, out int matriboxBank, out int matriboxSlot)
    {
        chocoGroup = lstChoco.SelectedIndex;
        switchIndex = cmbSwitch.SelectedIndex;
        matriboxBank = 1;
        matriboxSlot = 0;
        if (chocoGroup < 0 || switchIndex < 0)
        {
            MessageBox.Show(this, "Escolhe o grupo e o switch do Chocolate.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        if (treePresets.SelectedNode?.Tag is not ValueTuple<int, int> tag)
        {
            MessageBox.Show(this, "Escolhe um patch da Matribox (ex.: 01-A).", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        matriboxBank = tag.Item1;
        matriboxSlot = tag.Item2;
        return true;
    }

    private List<MidiCommand> BuildLongCommands(int bank, int slot)
    {
        int ch = (int)numChannel.Value;
        return MatriboxMidi.SelectPreset(bank, slot, ch);
    }

    private List<MidiCommand> BuildShortCommands()
    {
        int ch = (int)numChannel.Value;
        var list = new List<MidiCommand>();
        if (chkStomp.Checked)
        {
            list.Add(MatriboxMidi.StompMode(ch));
        }

        for (int i = 0; i < _fx.Length; i++)
        {
            list.Add(MatriboxMidi.ModuleOnOff(i, _fx[i].SelectedIndex == 1, ch));
        }

        return list;
    }

    private void RefreshPreview()
    {
        if (treePresets.SelectedNode?.Tag is not ValueTuple<int, int> tag)
        {
            return;
        }

        var longs = BuildLongCommands(tag.Item1, tag.Item2);
        var shorts = BuildShortCommands();
        txtPreview.Text =
            "B Bank (soltar / longo) — escolhe o patch na Matribox:" + Environment.NewLine
            + string.Join(Environment.NewLine, longs.Select(c => "  " + c.Summary))
            + Environment.NewLine + Environment.NewLine
            + "A Bank (pressionar / curto) — estado dos efeitos:" + Environment.NewLine
            + (shorts.Count == 0
                ? "  (nenhum CC de efeito — escolhe Off ou On à direita)"
                : string.Join(Environment.NewLine, shorts.Select(c => "  " + c.Summary)));
    }

    private FootswitchConfig CurrentSwitch()
    {
        var bank = _config.GetBank(Math.Max(0, lstChoco.SelectedIndex));
        return cmbSwitch.SelectedIndex switch
        {
            1 => bank.B,
            2 => bank.C,
            3 => bank.D,
            _ => bank.A
        };
    }

    private void ApplyToCurrentSwitch()
    {
        if (!TrySelection(out _, out _, out int mb, out int slot))
        {
            return;
        }

        var sw = CurrentSwitch();
        sw.Behavior = SwitchBehavior.ShortAndLong;
        sw.LongPress = BuildLongCommands(mb, slot);
        sw.ShortPress = BuildShortCommands();
        var group = _config.GetBank(lstChoco.SelectedIndex);
        group.Name = $"Matribox {mb:00}";
        RebuildChocolateGroups();
        Log($"Aplicado no Chocolate grupo {lstChoco.SelectedIndex + 1} / {cmbSwitch.Text}: patch {MatriboxMidi.PresetLabel(mb, slot)}, {sw.ShortPress.Count} CCs de efeito.");
    }

    private void btnRefresh_Click(object sender, EventArgs e) => RefreshPorts();

    private void btnConnectMatribox_Click(object sender, EventArgs e)
    {
        if (_matribox.IsConnected)
        {
            _matribox.Disconnect();
            btnConnectMatribox.Text = "Conectar Matribox";
            return;
        }

        if (cmbMatriboxMidi.SelectedItem is not string name)
        {
            MessageBox.Show(this, "Selecione o MIDI da Matribox.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            _matribox.Channel = (int)numChannel.Value;
            _matribox.Connect(name);
            btnConnectMatribox.Text = "Desconectar Matribox";
            ApplyChainToUi(_matribox.CurrentChain.Count > 0 ? _matribox.CurrentChain : HotoneSysex.EmptyChain());
            int named = _matribox.Presets.Count(MatriboxPresetStore.HasUsefulData);
            Log(named == 0
                ? "MIDI ligado. Esta porta não envia nomes. Duplo clique na árvore para nomear cada patch (fica no PC)."
                : $"MIDI ligado. Banco local: {named} patches com nome ou cadeia.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Matribox MIDI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnConnectChocolate_Click(object sender, EventArgs e)
    {
        if (_chocolate.IsConnected)
        {
            _chocolate.Disconnect();
            btnConnectChocolate.Text = "Conectar Chocolate";
            return;
        }

        if (cmbChocolateMidi.SelectedItem is not string name)
        {
            MessageBox.Show(this, "Selecione o MIDI do Chocolate (FootCtrlPlus).", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            SetBusy(true, "A ligar o Chocolate…");
            _chocolate.Connect(name);
            await _chocolate.HandshakeAsync();
            btnConnectChocolate.Text = "Desconectar Chocolate";
        }
        catch (Exception ex)
        {
            _chocolate.Disconnect();
            MessageBox.Show(this, ex.Message, "Chocolate USB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnReadChocolate_Click(object sender, EventArgs e)
    {
        if (!_chocolate.IsConnected)
        {
            MessageBox.Show(this, "Conecte o Chocolate primeiro.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            SetBusy(true, "A ler o Chocolate…");
            _config = await _chocolate.ReadConfigurationAsync();
            RebuildChocolateGroups();
            Log("Chocolate lido. Podes aplicar cenas Matribox por cima dos switches.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ler Chocolate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnWriteChocolate_Click(object sender, EventArgs e)
    {
        if (!_chocolate.IsConnected)
        {
            MessageBox.Show(this, "Conecte o Chocolate primeiro.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show(this, "Gravar estas cenas no Chocolate Plus?", Text,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        {
            return;
        }

        try
        {
            SetBusy(true, "A gravar no Chocolate…");
            _config.Mode = DeviceMode.AdvancedCustom;
            _config.TrsMode = TrsJackMode.Midi;
            await _chocolate.WriteConfigurationAsync(_config);
            Log("Gravado no Chocolate. Testa curto = efeitos, longo = patch da Matribox.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Gravar Chocolate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnTestMatribox_Click(object sender, EventArgs e)
    {
        if (!_matribox.IsConnected)
        {
            MessageBox.Show(this, "Conecte a Matribox (MIDI OUT) para testar.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (treePresets.SelectedNode?.Tag is not ValueTuple<int, int> tag)
        {
            return;
        }

        try
        {
            _matribox.Channel = (int)numChannel.Value;
            int index = (tag.Item1 - 1) * 4 + tag.Item2;
            await _matribox.RequestPresetAsync(index);
            _matribox.Send(BuildShortCommands());
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Testar Matribox", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnApply_Click(object sender, EventArgs e) => ApplyToCurrentSwitch();

    private void btnMapBank_Click(object sender, EventArgs e)
    {
        int group = lstChoco.SelectedIndex;
        if (group < 0)
        {
            MessageBox.Show(this, "Escolhe o grupo do Chocolate.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int matriboxBank = treePresets.SelectedNode?.Tag is ValueTuple<int, int> t
            ? t.Item1
            : treePresets.SelectedNode?.Tag is int b ? b : 1;

        var bank = _config.GetBank(group);
        bank.Name = $"Matribox {matriboxBank:00}";
        int s = 0;
        foreach (var sw in bank.Switches)
        {
            sw.Behavior = SwitchBehavior.ShortAndLong;
            sw.LongPress = BuildLongCommands(matriboxBank, s);
            sw.ShortPress = BuildShortCommands();
            s++;
        }

        RebuildChocolateGroups();
        Log($"Grupo Chocolate {group + 1}: switches A–D = Matribox {matriboxBank:00}-A até {matriboxBank:00}-D.");
    }

    private void btnAllOn_Click(object sender, EventArgs e)
    {
        foreach (var cmb in _fx)
        {
            cmb.SelectedIndex = 1;
        }
    }

    private void btnAllOff_Click(object sender, EventArgs e)
    {
        foreach (var cmb in _fx)
        {
            cmb.SelectedIndex = 0;
        }
    }

    private void btnImportPrst_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "Pasta dos .prst exportados pelo editor Sonicake",
            UseDescriptionForTitle = true
        };
        string? hint = MatriboxPrstImport.FirstExistingDefaultFolder();
        if (hint is not null)
        {
            dlg.InitialDirectory = hint;
            dlg.SelectedPath = hint;
        }

        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ImportPrstFolder(dlg.SelectedPath, quiet: false);
    }

    private void ImportPrstFolder(string folder, bool quiet)
    {
        var list = _matribox.Presets.ToList();
        if (list.Count < HotoneSysex.PresetCount)
        {
            list = MatriboxPresetStore.EmptyList();
            _matribox.ReplacePresets(list);
            list = _matribox.Presets.ToList();
        }

        int got = MatriboxPrstImport.ImportFolder(folder, list);
        if (got == 0)
        {
            if (!quiet)
            {
                MessageBox.Show(this,
                    "Nenhum .prst com nome no formato 01-A Nome.prst nesta pasta.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return;
        }

        _matribox.ReplacePresets(list);
        SaveLocalPresets();
        FillMatriboxTree();
        int withChain = list.Count(p => MatriboxPresetStore.HasChain(p.Chain));
        Log($"Importados {got} patches ({withChain} com cadeia) de {folder}");
    }

    private async void btnScanMatribox_Click(object sender, EventArgs e)
    {
        if (!_matribox.IsConnected)
        {
            MessageBox.Show(this, "Conecta a Matribox primeiro.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show(this,
                "A porta MIDI da Matribox não envia nomes nem a cadeia de efeitos.\n\nO percurso só confirma que o USB responde (240 patches, ~2–3 min, o som muda). Os nomes que já escreveste no PC mantêm-se.",
                Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            != DialogResult.OK)
        {
            return;
        }

        await RefreshFromPedalAsync();
    }

    private async Task RefreshFromPedalAsync()
    {
        SetBusy(true, "A ler patches…", 0, HotoneSysex.PresetCount);
        try
        {
            await _matribox.DumpPresetListAsync();
            int useful = _matribox.Presets.Count(MatriboxPresetStore.HasUsefulData);
            SaveLocalPresets();
            FillMatriboxTree();
            Log(useful == 0
                ? "Nenhum nome/cadeia veio da pedaleira. O JSON não foi preenchido (e não apaga nomes já gravados). Duplo clique na árvore para nomear."
                : $"Banco local: {useful} patches com dados. {MatriboxPresetStore.FilePath}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Atualizar da pedaleira", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void treePresets_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.Node?.Tag is not ValueTuple<int, int>)
        {
            e.CancelEdit = true;
        }
    }

    private void treePresets_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.CancelEdit || e.Node?.Tag is not ValueTuple<int, int> tag || e.Label is null)
        {
            return;
        }

        string typed = e.Label.Trim();
        string prefix = MatriboxMidi.PresetLabel(tag.Item1, tag.Item2);
        string name = typed.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? typed[prefix.Length..].TrimStart(' ', '-')
            : typed;
        int index = (tag.Item1 - 1) * 4 + tag.Item2;
        _matribox.RenamePreset(index, name);
        SaveLocalPresets();
        e.Node.Text = string.IsNullOrWhiteSpace(name) ? prefix : $"{prefix}  {name}";
        e.CancelEdit = true;
    }

    private void btnInicio_Click(object sender, EventArgs e) => Close();

    private void treePresets_AfterSelect(object sender, TreeViewEventArgs e)
    {
        RefreshPreview();
        if (_loadingTree)
        {
            return;
        }

        if (treePresets.SelectedNode?.Tag is not ValueTuple<int, int> tag)
        {
            ApplyChainToUi(HotoneSysex.EmptyChain());
            return;
        }

        int index = (tag.Item1 - 1) * 4 + tag.Item2;
        if (index < _matribox.Presets.Count
            && _matribox.Presets[index].Chain is { Count: > 0 } cached)
        {
            ApplyChainToUi(cached);
            return;
        }

        ApplyChainToUi(HotoneSysex.EmptyChain());
    }

    private void SetBusy(bool busy, string? text = null, int current = 0, int total = 0)
    {
        _busy = busy;
        panelScan.Visible = busy;
        if (busy)
        {
            lblScan.Text = text ?? "A ler…";
            if (total > 0)
            {
                progressBarScan.Style = ProgressBarStyle.Continuous;
                progressBarScan.Maximum = Math.Max(1, total);
                progressBarScan.Value = Math.Clamp(current, 0, progressBarScan.Maximum);
            }
            else
            {
                progressBarScan.Style = ProgressBarStyle.Marquee;
            }
        }

        treePresets.Enabled = !busy;
        lstChoco.Enabled = !busy;
        panelFxScroll.Enabled = !busy;
        panelFxTop.Enabled = !busy;
        panelBtnRow1.Enabled = !busy;
        panelBtnRow2.Enabled = !busy;
        btnInicio.Enabled = !busy;
        btnRefresh.Enabled = !busy;
        btnConnectMatribox.Enabled = !busy;
        btnConnectChocolate.Enabled = !busy;
        cmbMatriboxMidi.Enabled = !busy;
        cmbChocolateMidi.Enabled = !busy;
        numChannel.Enabled = !busy;
        txtLog.Enabled = true;
        UseWaitCursor = busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
        Cursor.Current = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void ShowProgress(int current, int total, string message)
    {
        if (!_busy)
        {
            return;
        }

        progressBarScan.Style = ProgressBarStyle.Continuous;
        progressBarScan.Maximum = Math.Max(1, total);
        progressBarScan.Value = Math.Clamp(current, 0, progressBarScan.Maximum);
        lblScan.Text = $"{current} / {total}  {message}";
    }

    private void numChannel_ValueChanged(object sender, EventArgs e) => RefreshPreview();

    private void chkStomp_CheckedChanged(object sender, EventArgs e) => RefreshPreview();

    private void Log(string line)
    {
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
    }
}
