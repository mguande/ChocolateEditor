using ChocolateEditor.Controls;
using ChocolateEditor.Matribox;
using ChocolateEditor.Models;
using ChocolateEditor.Protocol;

using ChocolateEditor.Ui;
using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

namespace ChocolateEditor.Forms;

public partial class FormChocolateMatribox : DarkForm
{
    private readonly ChocolateUsbClient _usb = new();
    private readonly List<MatriboxPresetInfo> _presets = MatriboxPresetStore.Load();
    private DeviceConfiguration _config = DeviceConfiguration.CreateStarter();
    private ChainStompPedal?[] _pedals = new ChainStompPedal?[HotoneSysex.SlotCount];
    private ForeverButton[] _sw = [];
    private int _switchIndex;
    private bool _loading = true;
    private bool _dirty;

    public FormChocolateMatribox()
    {
        InitializeComponent();
        ApplyTheme();
        _usb.Log += (_, _) => { };
        _sw = [btnSwA, btnSwB, btnSwC, btnSwD];
        BuildPedalStrip();
        FillChocolateBanks();
        FillPatchList();
        RefreshPorts();
        SelectSwitch(0);
        _loading = false;
        SetWorkspaceEnabled(false);
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
    }

    protected override bool AutoThemeControls => false;

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _usb.Dispose();
        base.OnFormClosed(e);
    }

    private void ApplyTheme()
    {
        BackColor = UiTheme.Bg;
        ForeColor = UiTheme.Text;
        panelTop.BackColor = UiTheme.Surface;
        panelTopActions.BackColor = UiTheme.Surface;
        panelBody.BackColor = UiTheme.Bg;
        panelMatribox.BackColor = UiTheme.Bg;
        splitMatribox.BackColor = UiTheme.Bg;
        splitMatribox.Panel1.BackColor = UiTheme.Bg;
        splitMatribox.Panel2.BackColor = UiTheme.Bg;
        foreach (var header in new[] { tabChoco, tabMatribox, tabBancos, tabFx })
        {
            header.BackColor = Color.Transparent;
            header.ForeColor = UiTheme.Text;
            header.Font = new Font("Segoe UI Semibold", 11F);
        }

        tabChoco.BackColor = UiTheme.Surface;
        tabBancos.BackColor = UiTheme.Surface;
        tabFx.BackColor = UiTheme.Surface;
        tabMatribox.BackColor = UiTheme.Bg;
        tabBancos.Font = new Font("Segoe UI Semibold", 10F);
        tabFx.Font = new Font("Segoe UI Semibold", 10F);
        panelFxScroll.BackColor = UiTheme.Surface;
        flowPedals.BackColor = UiTheme.Surface;
        lstPatches.BackColor = UiTheme.Surface;
        lstPatches.ForeColor = UiTheme.Text;
        lstPatches.DrawMode = DrawMode.OwnerDrawFixed;
        lstPatches.ItemHeight = 28;
        lstPatches.DrawItem -= lstPatches_DrawItem;
        lstPatches.DrawItem += lstPatches_DrawItem;
        foreach (ComboBox cmb in new[] { cmbChocolateMidi, cmbChocoBank })
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = UiTheme.Surface2;
            cmb.ForeColor = UiTheme.Text;
        }

        numChannel.BackColor = UiTheme.Surface2;
        numChannel.ForeColor = UiTheme.Text;
        lblTitle.ForeColor = UiTheme.Text;
        lblBanco.ForeColor = UiTheme.Muted;
        lblMidi.ForeColor = UiTheme.Muted;
        lblChannel.ForeColor = UiTheme.Muted;
        lblStomp.ForeColor = UiTheme.Text;
        lblChocoHint.ForeColor = UiTheme.Muted;
        lblFxHint.ForeColor = UiTheme.Muted;
        UiTheme.StyleAction(btnInicio, UiTheme.Surface2);
        UiTheme.StyleAction(btnRead, UiTheme.Surface2);
        StyleConnect();
        foreach (var sw in new[] { btnSwA, btnSwB, btnSwC, btnSwD })
        {
            sw.Rounded = true;
            sw.Cursor = Cursors.Hand;
            sw.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            sw.TextColor = Color.White;
        }

        chkStomp.BaseColor = UiTheme.Chocolate;
        chkStomp.BGColor = UiTheme.Surface2;
        chkStomp.ToggleColor = UiTheme.Surface;
        StyleSave(false);
        MakeRoundedCard(panelChoco);
        MakeRoundedCard(panelBancos);
        MakeRoundedCard(panelFx);
    }

    private void lstPatches_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0)
        {
            return;
        }

        bool sel = (e.State & DrawItemState.Selected) != 0;
        using var bg = new SolidBrush(sel ? UiTheme.Teal : UiTheme.Surface);
        e.Graphics.FillRectangle(bg, e.Bounds);
        string text = lstPatches.Items[e.Index]?.ToString() ?? "";
        var bounds = Rectangle.Inflate(e.Bounds, -8, 0);
        TextRenderer.DrawText(
            e.Graphics,
            text,
            lstPatches.Font,
            bounds,
            sel ? Color.White : UiTheme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void MakeRoundedCard(Panel panel)
    {
        panel.BackColor = UiTheme.Surface;
        void Clip()
        {
            var r = panel.ClientRectangle;
            if (r.Width < 8 || r.Height < 8)
            {
                return;
            }

            using var path = GraphicsRound.CreatePath(r, 14);
            panel.Region?.Dispose();
            panel.Region = new Region(path);
        }

        Clip();
        panel.Resize += (_, _) => Clip();
    }

    private void StyleSave(bool dirty)
    {
        _dirty = dirty;
        UiTheme.StyleAction(btnWrite, dirty ? UiTheme.Accent : UiTheme.Surface2);
        btnWrite.Text = dirty ? "Gravar no Chocolate •" : "Gravar no Chocolate";
    }

    private void StyleConnect()
    {
        if (_usb.IsConnected)
        {
            btnConnect.Text = "Desligar";
            UiTheme.StyleAction(btnConnect, UiTheme.Accent);
        }
        else
        {
            btnConnect.Text = "Conectar";
            UiTheme.StyleAction(btnConnect, UiTheme.Blue);
        }
    }

    private void BuildPedalStrip()
    {
        flowPedals.WrapContents = true;
        flowPedals.BackColor = UiTheme.Surface;
        RefreshPedalStrip();
    }

    private void RefreshPedalStrip(bool fromChocolateMapping = true)
    {
        var chain = CurrentPatchChain();
        var cmds = TryCurrentGroup(out _)
            ? SwitchCommands(CurrentSwitch()).ToList()
            : [];
        bool prev = _loading;
        _loading = true;
        flowPedals.SuspendLayout();
        flowPedals.Controls.Clear();
        _pedals = new ChainStompPedal?[HotoneSysex.SlotCount];
        var slots = chain?
            .Where(s => !s.Empty)
            .OrderBy(s => s.Position)
            .ToList() ?? [];
        if (slots.Count == 0)
        {
            flowPedals.Controls.Add(new Label
            {
                AutoSize = true,
                MaximumSize = new Size(520, 0),
                ForeColor = UiTheme.Muted,
                Padding = new Padding(8),
                Text = "Escolhe um patch da Matribox para ver os pedais da cadeia. Clica num pedal para o ligar ou desligar no Chocolate."
            });
        }
        else
        {
            for (int n = 0; n < slots.Count; n++)
            {
                if (n > 0)
                {
                    flowPedals.Controls.Add(new ChainCable());
                }

                var slot = slots[n];
                string alg = MatriboxAlgCatalog.Name(slot.Algorithm);
                bool engaged = fromChocolateMapping
                    ? PedalEngagedFromMapping(cmds, chain, slot.Position)
                    : slot.On;
                var pedal = new ChainStompPedal(slot.Position, slot.ModuleType, alg, engaged)
                {
                    Enabled = _usb.IsConnected && TryCurrentGroup(out _)
                };
                pedal.EngagedChanged += (_, _) => CommitCurrentSwitch();
                _pedals[slot.Position] = pedal;
                flowPedals.Controls.Add(pedal);
            }
        }

        flowPedals.ResumeLayout();
        _loading = prev;
    }

    private static bool PedalEngagedFromMapping(
        IEnumerable<MidiCommand> commands,
        IReadOnlyList<MatriboxChainSlot>? chain,
        int slot)
    {
        int stored = MatriboxMidi.EffectChoice(commands, slot);
        if (stored == 2)
        {
            return true;
        }

        if (stored == 1)
        {
            return false;
        }

        return MatriboxMidi.PresetEffectChoice(chain, slot) == 2;
    }

    private const string ChocoBankPlaceholder = "Selecione";
    private const string UnconfiguredLabel = "Não configurado";

    private void FillChocolateBanks()
    {
        cmbChocoBank.Items.Clear();
        cmbChocoBank.Items.Add(ChocoBankPlaceholder);
        _config.EnsureBankCount();
        foreach (var bank in _config.Banks.Take(_config.BankCount))
        {
            cmbChocoBank.Items.Add(bank.ListLabel);
        }

        cmbChocoBank.SelectedIndex = 0;
    }

    private void FillPatchList()
    {
        lstPatches.BeginUpdate();
        lstPatches.Items.Clear();
        lstPatches.Items.Add(UnconfiguredLabel);
        for (int i = 0; i < HotoneSysex.PresetCount; i++)
        {
            var p = i < _presets.Count ? _presets[i] : new MatriboxPresetInfo { Index = i };
            lstPatches.Items.Add(p.Display);
        }

        lstPatches.EndUpdate();
        lstPatches.SelectedIndex = -1;
    }

    private int SelectedPresetIndex => lstPatches.SelectedIndex - 1;

    private void RefreshPorts()
    {
        var ports = ChocolateUsbClient.ListPorts();
        cmbChocolateMidi.Items.Clear();
        foreach (var port in ports)
        {
            cmbChocolateMidi.Items.Add(port.Name);
        }

        int hit = ports.ToList().FindIndex(p => p.LooksLikeChocolate);
        if (cmbChocolateMidi.Items.Count > 0)
        {
            cmbChocolateMidi.SelectedIndex = hit >= 0 ? hit : 0;
        }
    }

    private int ChocolateGroupIndex => cmbChocoBank.SelectedIndex - 1;

    private bool TryCurrentGroup(out Bank group)
    {
        int i = ChocolateGroupIndex;
        if (i < 0)
        {
            group = null!;
            return false;
        }

        group = _config.GetBank(i);
        return true;
    }

    private FootswitchConfig CurrentSwitch()
        => TryCurrentGroup(out var group)
            ? group[_switchIndex switch
            {
                1 => FootswitchId.B,
                2 => FootswitchId.C,
                3 => FootswitchId.D,
                _ => FootswitchId.A
            }]
            : new FootswitchConfig();

    private void btnSwitch_Click(object? sender, EventArgs e)
    {
        int i = Array.IndexOf(_sw, sender);
        if (i >= 0)
        {
            SelectSwitch(i);
        }
    }

    private void SelectSwitch(int index)
    {
        _switchIndex = index;
        for (int i = 0; i < _sw.Length; i++)
        {
            bool on = i == index;
            _sw[i].BaseColor = on ? UiTheme.Chocolate : UiTheme.Surface2;
            _sw[i].TextColor = Color.White;
            _sw[i].Invalidate();
        }

        SetEffectsEnabled(TryCurrentGroup(out _));
        lblFxHint.Text = index == 0
            ? "A · curto só manda efeitos diferentes do patch. A · longo escolhe o patch."
            : $"Switch {(char)('A' + index)} · curto liga/desliga os pedais.";
        LoadFromConfig();
    }

    private void SetEffectsEnabled(bool on)
    {
        foreach (var pedal in _pedals)
        {
            if (pedal is not null)
            {
                pedal.Enabled = on && _usb.IsConnected;
            }
        }
    }

    private void SetWorkspaceEnabled(bool on)
    {
        panelChoco.Enabled = on;
        panelMatribox.Enabled = on;
        btnRead.Enabled = on;
        btnWrite.Enabled = on;
        numChannel.Enabled = on;
        cmbChocolateMidi.Enabled = !on;
        if (!on)
        {
            SetEffectsEnabled(false);
            lblChocoHint.Text = "Liga o Chocolate para editar o mapeamento.";
        }
    }

    private bool TryPatchOfGroup(Bank group, out int bank, out int slot)
        => MatriboxMidi.TryParseSelectPreset(
            group.A.LongPress.Concat(group.A.ShortPress), out bank, out slot);

    private static IEnumerable<MidiCommand> SwitchCommands(FootswitchConfig sw)
        => sw.ShortPress.Concat(sw.LongPress);

    private void LoadFromConfig()
    {
        _loading = true;
        if (!TryCurrentGroup(out var group))
        {
            lstPatches.SelectedIndex = -1;
            chkStomp.Checked = false;
            SetEffectsEnabled(false);
            RefreshPedalStrip();
            UpdateHint();
            _loading = false;
            return;
        }

        SetEffectsEnabled(true);

        if (TryPatchOfGroup(group, out int bank, out int slot))
        {
            int listIndex = (bank - 1) * 4 + slot + 1;
            if (listIndex >= 1 && listIndex < lstPatches.Items.Count)
            {
                lstPatches.SelectedIndex = listIndex;
            }
            else
            {
                lstPatches.SelectedIndex = 0;
            }
        }
        else
        {
            lstPatches.SelectedIndex = 0;
        }

        chkStomp.Checked = MatriboxMidi.HasStomp(group.A.ShortPress);
        RefreshPedalStrip();
        UpdateHint();
        _loading = false;
    }

    private IReadOnlyList<MatriboxChainSlot>? CurrentPatchChain()
    {
        int index = SelectedPresetIndex;
        return index >= 0 && index < _presets.Count ? _presets[index].Chain : null;
    }

    private void UpdateHint()
    {
        int index = lstPatches.SelectedIndex;
        string patch = index >= 0 && index < lstPatches.Items.Count
            ? lstPatches.Items[index]?.ToString() ?? ""
            : "";
        if (!_usb.IsConnected)
        {
            lblChocoHint.Text = "Liga o Chocolate para editar o mapeamento.";
            return;
        }

        if (!TryCurrentGroup(out _))
        {
            lblChocoHint.Text = "Escolhe um banco do Chocolate para filtrar o patch da Matribox.";
            return;
        }

        lblChocoHint.Text = _switchIndex == 0
            ? $"A · curto → pedais  ·  A · longo → {patch}."
            : $"A · longo → {patch}  ·  {(char)('A' + _switchIndex)} · curto liga/desliga os pedais.";
    }

    private void CommitCurrentSwitch()
    {
        if (_loading || !TryCurrentGroup(out var group))
        {
            return;
        }

        int ch = (int)numChannel.Value;
        var effects = BuildEffectCommands(ch);
        WriteAMapping(group, ch, _switchIndex == 0
            ? effects
            : group.A.ShortPress.Where(IsEffectCc).Select(c => c.Clone()));

        if (_switchIndex > 0)
        {
            var sw = CurrentSwitch();
            sw.Behavior = SwitchBehavior.ShortTap;
            sw.LongPress = [];
            sw.ShortPress = effects;
        }

        RefreshBankComboLabel(group);
        UpdateHint();
        StyleSave(true);
    }

    private void WriteAMapping(Bank group, int channel, IEnumerable<MidiCommand> effectCommands)
    {
        int index = SelectedPresetIndex;
        group.A.Behavior = SwitchBehavior.ShortAndLong;
        group.A.ShortPress = BuildAShortPress(channel, effectCommands);
        if (index < 0)
        {
            group.A.LongPress = [];
            group.Name = UnconfiguredLabel;
        }
        else
        {
            int bank = index / 4 + 1;
            int slot = index % 4;
            group.A.LongPress = MatriboxMidi.SelectPreset(bank, slot, channel);
            group.Name = $"Matribox {bank:00}";
        }
    }

    private List<MidiCommand> BuildAShortPress(int channel, IEnumerable<MidiCommand> effectCommands)
    {
        var list = new List<MidiCommand>();
        if (chkStomp.Checked)
        {
            list.Add(MatriboxMidi.StompMode(channel));
        }

        list.AddRange(effectCommands);
        return list;
    }

    private static bool IsEffectCc(MidiCommand cmd)
        => cmd.Type == MidiCommandType.ControlChange && cmd.Data1 is >= 43 and <= 54;

    private void RefreshBankComboLabel(Bank group)
    {
        int keep = cmbChocoBank.SelectedIndex;
        if (keep < 0 || keep >= cmbChocoBank.Items.Count)
        {
            return;
        }

        bool prev = _loading;
        _loading = true;
        cmbChocoBank.Items[keep] = group.ListLabel;
        if (cmbChocoBank.SelectedIndex != keep)
        {
            cmbChocoBank.SelectedIndex = keep;
        }

        _loading = prev;
    }

    private void ApplyMatriboxChainStatesToCombos()
        => RefreshPedalStrip(fromChocolateMapping: false);

    private void ApplyNewMatriboxPatch(Bank group)
    {
        int ch = (int)numChannel.Value;
        int presetIndex = SelectedPresetIndex;
        ApplyMatriboxChainStatesToCombos();
        var effects = BuildEffectCommands(ch);
        WriteAMapping(group, ch, effects);
        foreach (var sw in group.Switches.Skip(1))
        {
            sw.Behavior = SwitchBehavior.ShortTap;
            sw.LongPress = [];
            sw.ShortPress = presetIndex < 0
                ? []
                : effects.Select(c => c.Clone()).ToList();
        }

        RefreshBankComboLabel(group);
        UpdateHint();
        StyleSave(true);
    }

    private List<MidiCommand> BuildEffectCommands(int channel)
    {
        var chain = CurrentPatchChain();
        var list = new List<MidiCommand>();
        for (int i = 0; i < _pedals.Length; i++)
        {
            var pedal = _pedals[i];
            if (pedal is null)
            {
                continue;
            }

            int want = pedal.Engaged ? 2 : 1;
            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            if (preset != 0 && want == preset)
            {
                continue;
            }

            list.Add(MatriboxMidi.ModuleOnOff(i, pedal.Engaged, channel));
        }

        return list;
    }

    private bool SlimRedundantEffects()
    {
        bool changed = false;
        _config.EnsureBankCount();
        foreach (var group in _config.Banks.Take(_config.BankCount))
        {
            if (!TryPatchOfGroup(group, out int bank, out int slot))
            {
                continue;
            }

            int index = (bank - 1) * 4 + slot;
            var chain = index >= 0 && index < _presets.Count ? _presets[index].Chain : null;
            if (chain is null)
            {
                continue;
            }

            foreach (var sw in group.Switches)
            {
                var shortPress = StripMatchingModuleCcs(sw.ShortPress, chain);
                var longPress = StripMatchingModuleCcs(sw.LongPress, chain);
                if (!SameCommands(sw.ShortPress, shortPress))
                {
                    sw.ShortPress = shortPress;
                    changed = true;
                }

                if (!SameCommands(sw.LongPress, longPress))
                {
                    sw.LongPress = longPress;
                    changed = true;
                }
            }
        }

        return changed;
    }

    private static List<MidiCommand> StripMatchingModuleCcs(
        List<MidiCommand> commands, IReadOnlyList<MatriboxChainSlot> chain)
    {
        return commands.Where(cmd =>
        {
            if (!IsEffectCc(cmd))
            {
                return true;
            }

            int i = cmd.Data1 - 43;
            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            int sent = cmd.Data2 >= 64 ? 2 : 1;
            return preset == 0 || sent != preset;
        }).ToList();
    }

    private static bool SameCommands(List<MidiCommand> a, List<MidiCommand> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].Type != b[i].Type || a[i].Data1 != b[i].Data1 || a[i].Data2 != b[i].Data2)
            {
                return false;
            }
        }

        return true;
    }

    private void lstPatches_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshPedalStrip();
        if (_loading || !TryCurrentGroup(out var group))
        {
            return;
        }

        bool hadPatch = TryPatchOfGroup(group, out int oldBank, out int oldSlot);
        int oldPreset = hadPatch ? (oldBank - 1) * 4 + oldSlot : -1;
        int newPreset = SelectedPresetIndex;
        if (oldPreset == newPreset)
        {
            return;
        }

        if (hadPatch)
        {
            string from = lstPatches.Items[oldPreset + 1]?.ToString() ?? UnconfiguredLabel;
            string to = lstPatches.SelectedItem?.ToString() ?? UnconfiguredLabel;
            var answer = MessageBox.Show(
                this,
                $"Este banco do Chocolate já está ligado a:\n{from}\n\n" +
                $"Ao mudar para:\n{to}\n\n" +
                "o patch e as configurações de efeitos (A/B/C/D) serão substituídas ao gravar no Chocolate.\n\nContinuar?",
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes)
            {
                _loading = true;
                lstPatches.SelectedIndex = oldPreset + 1;
                RefreshPedalStrip();
                _loading = false;
                return;
            }
        }

        ApplyNewMatriboxPatch(group);
    }

    private void cmbChocoBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loading)
        {
            return;
        }

        if (!TryCurrentGroup(out _))
        {
            LoadFromConfig();
            return;
        }

        SelectSwitch(0);
    }

    private void chkStomp_CheckedChanged(object sender) => CommitCurrentSwitch();

    private void numChannel_ValueChanged(object sender, EventArgs e) => CommitCurrentSwitch();

    private void btnInicio_Click(object sender, EventArgs e) => Close();

    private async void btnConnect_Click(object sender, EventArgs e)
    {
        if (_usb.IsConnected)
        {
            _usb.Disconnect();
            _loading = true;
            FillChocolateBanks();
            _loading = false;
            LoadFromConfig();
            StyleSave(false);
            SetWorkspaceEnabled(false);
            StyleConnect();
            return;
        }

        if (cmbChocolateMidi.SelectedItem is not string name)
        {
            MessageBox.Show(this, "Escolhe o MIDI do Chocolate (FootCtrlPlus).", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            UseWaitCursor = true;
            _usb.Connect(name);
            await _usb.HandshakeAsync();
            _config = await _usb.ReadConfigurationAsync();
            _loading = true;
            FillChocolateBanks();
            _loading = false;
            SetWorkspaceEnabled(true);
            bool slimmed = SlimRedundantEffects();
            LoadFromConfig();
            StyleSave(slimmed);
            StyleConnect();
        }
        catch (Exception ex)
        {
            _usb.Disconnect();
            SetWorkspaceEnabled(false);
            StyleConnect();
            MessageBox.Show(this, ex.Message, "Chocolate USB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private async void btnRead_Click(object sender, EventArgs e)
    {
        if (!_usb.IsConnected)
        {
            MessageBox.Show(this, "Liga o Chocolate primeiro.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            UseWaitCursor = true;
            _config = await _usb.ReadConfigurationAsync();
            _loading = true;
            FillChocolateBanks();
            _loading = false;
            bool slimmed = SlimRedundantEffects();
            LoadFromConfig();
            StyleSave(slimmed);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ler Chocolate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private async void btnWrite_Click(object sender, EventArgs e)
    {
        if (!_usb.IsConnected)
        {
            MessageBox.Show(this, "Liga o Chocolate primeiro.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        CommitCurrentSwitch();
        try
        {
            UseWaitCursor = true;
            await _usb.WriteConfigurationAsync(_config);
            StyleSave(false);
            MessageBox.Show(this, "Alterações gravadas no Chocolate.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
}
