using ChocolateEditor.Matribox;
using ChocolateEditor.Models;
using ChocolateEditor.Protocol;

namespace ChocolateEditor.Forms;

public partial class FormChocolateMatribox : Form
{
    private static readonly Color Bg = Color.FromArgb(18, 18, 20);
    private static readonly Color Card = Color.FromArgb(36, 36, 40);
    private static readonly Color Border = Color.FromArgb(90, 90, 96);
    private static readonly Color Blue = Color.FromArgb(77, 163, 255);
    private static readonly Color Fg = Color.White;
    private static readonly Color Muted = Color.FromArgb(176, 176, 184);

    private readonly ChocolateUsbClient _usb = new();
    private readonly List<MatriboxPresetInfo> _presets = MatriboxPresetStore.Load();
    private DeviceConfiguration _config = DeviceConfiguration.CreateStarter();
    private ComboBox[] _fx = [];
    private Label[] _fxLabels = [];
    private Button[] _sw = [];
    private int _switchIndex;
    private bool _loading = true;
    private bool _dirty;

    public FormChocolateMatribox()
    {
        InitializeComponent();
        ApplyTheme();
        _usb.Log += (_, _) => { };
        _sw = [btnSwA, btnSwB, btnSwC, btnSwD];
        BuildEffectRows();
        FillChocolateBanks();
        FillPatchList();
        RefreshPorts();
        SelectSwitch(0);
        _loading = false;
        SetWorkspaceEnabled(false);
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _usb.Dispose();
        base.OnFormClosed(e);
    }

    private void ApplyTheme()
    {
        BackColor = Bg;
        ForeColor = Fg;
        foreach (Control c in new Control[]
                 {
                     panelTop, panelChoco, panelMatribox, panelBancos, panelFx, panelFxScroll
                 })
        {
            c.BackColor = Card;
            c.ForeColor = Fg;
        }

        panelTop.BackColor = Bg;
        foreach (var tab in new[] { tabChoco, tabMatribox, tabBancos, tabFx })
        {
            tab.BackColor = Blue;
            tab.ForeColor = Color.FromArgb(12, 20, 32);
        }

        lstPatches.BackColor = Color.FromArgb(24, 24, 28);
        lstPatches.ForeColor = Fg;
        foreach (ComboBox cmb in new[] { cmbChocolateMidi, cmbChocoBank })
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Color.FromArgb(24, 24, 28);
            cmb.ForeColor = Fg;
        }

        numChannel.BackColor = Color.FromArgb(24, 24, 28);
        numChannel.ForeColor = Fg;
        chkStomp.ForeColor = Fg;
        lblChocoHint.ForeColor = Muted;
        lblFxHint.ForeColor = Muted;
        lblBanco.ForeColor = Fg;
        lblTitle.ForeColor = Fg;
        lblChannel.ForeColor = Muted;
        foreach (Button b in new[] { btnInicio, btnConnect, btnRead })
        {
            StyleGhost(b);
        }

        StyleSave(false);
        PaintCard(panelChoco);
        PaintCard(panelMatribox);
        PaintCard(panelBancos);
        PaintCard(panelFx);
    }

    private static void PaintCard(Panel panel)
    {
        panel.Paint += (_, e) =>
        {
            var r = panel.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            using var pen = new Pen(Border);
            e.Graphics.DrawRectangle(pen, r);
        };
    }

    private static void StyleGhost(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderColor = Border;
        b.BackColor = Color.FromArgb(28, 28, 32);
        b.ForeColor = Fg;
    }

    private void StyleSave(bool dirty)
    {
        _dirty = dirty;
        btnWrite.FlatStyle = FlatStyle.Flat;
        btnWrite.FlatAppearance.BorderColor = dirty ? Blue : Border;
        btnWrite.BackColor = dirty ? Blue : Color.FromArgb(28, 28, 32);
        btnWrite.ForeColor = dirty ? Color.FromArgb(12, 20, 32) : Fg;
        btnWrite.Text = dirty ? "Gravar no Chocolate •" : "Gravar no Chocolate";
    }

    private void BuildEffectRows()
    {
        _fx = new ComboBox[HotoneSysex.SlotCount];
        _fxLabels = new Label[HotoneSysex.SlotCount];
        tableFx.RowCount = HotoneSysex.SlotCount;
        tableFx.RowStyles.Clear();
        for (int i = 0; i < _fx.Length; i++)
        {
            tableFx.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            var lbl = new Label
            {
                Text = $"--  (CC {MatriboxMidi.ModuleCc(i)})",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Fg,
                AutoEllipsis = true
            };
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(24, 24, 28),
                ForeColor = Fg
            };
            cmb.Items.AddRange(["Não enviar", "Off", "On"]);
            cmb.SelectedIndex = 0;
            cmb.SelectedIndexChanged += (_, _) => CommitCurrentSwitch();
            _fx[i] = cmb;
            _fxLabels[i] = lbl;
            tableFx.Controls.Add(lbl, 0, i);
            tableFx.Controls.Add(cmb, 1, i);
        }

        tableFx.Height = _fx.Length * 32;
        tableFx.BackColor = Card;
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
            _sw[i].BackColor = on ? Blue : Color.FromArgb(28, 28, 32);
            _sw[i].ForeColor = on ? Color.FromArgb(12, 20, 32) : Fg;
            _sw[i].FlatAppearance.BorderColor = on ? Blue : Border;
        }

        SetEffectsEnabled(TryCurrentGroup(out _));
        lblFxHint.Text = index == 0
            ? "A · curto só manda efeitos diferentes do patch. A · longo escolhe o patch."
            : $"Switch {(char)('A' + index)} · curto só manda o que for diferente do patch da Matribox.";
        LoadFromConfig();
    }

    private void SetEffectsEnabled(bool on)
    {
        foreach (var cmb in _fx)
        {
            cmb.Enabled = on && _usb.IsConnected;
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
            foreach (var cmb in _fx)
            {
                cmb.SelectedIndex = 0;
            }

            SetEffectsEnabled(false);
            ApplyPatchChainToLabels();
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
        ApplyPatchChainToLabels();
        var chain = CurrentPatchChain();
        var cmds = SwitchCommands(CurrentSwitch());
        for (int i = 0; i < _fx.Length; i++)
        {
            int stored = MatriboxMidi.EffectChoice(cmds, i);
            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            _fx[i].SelectedIndex = stored != 0 && stored == preset ? 0 : stored;
        }

        UpdateHint();
        _loading = false;
    }

    private IReadOnlyList<MatriboxChainSlot>? CurrentPatchChain()
    {
        int index = SelectedPresetIndex;
        return index >= 0 && index < _presets.Count ? _presets[index].Chain : null;
    }

    private static string EffectCaption(IReadOnlyList<MatriboxChainSlot>? chain, int moduleIndex)
    {
        if (chain is { Count: > 0 } && moduleIndex < chain.Count && !chain[moduleIndex].Empty)
        {
            return chain[moduleIndex].EffectName;
        }

        return $"Slot {moduleIndex + 1}";
    }

    private void ApplyPatchChainToLabels()
    {
        var chain = CurrentPatchChain();
        bool prev = _loading;
        _loading = true;
        for (int i = 0; i < _fxLabels.Length; i++)
        {
            string name = EffectCaption(chain, i);
            if (chain is { Count: > 0 } && i < chain.Count && !chain[i].Empty)
            {
                _fxLabels[i].Text = chain[i].Label;
            }
            else
            {
                _fxLabels[i].Text = $"--  (CC {MatriboxMidi.ModuleCc(i)})";
            }

            int keep = _fx[i].SelectedIndex;
            _fx[i].Items.Clear();
            _fx[i].Items.AddRange([$"Não enviar · {name}", $"Off · {name}", $"On · {name}"]);
            _fx[i].SelectedIndex = keep is >= 0 and <= 2 ? keep : 0;
        }

        _loading = prev;
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
            ? $"A · curto → efeitos  ·  A · longo → {patch}."
            : $"A · longo → {patch}  ·  {(char)('A' + _switchIndex)} · curto manda os On/Off da lista.";
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
    {
        bool prev = _loading;
        _loading = true;
        for (int i = 0; i < _fx.Length; i++)
        {
            _fx[i].SelectedIndex = 0;
        }

        _loading = prev;
    }

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
        for (int i = 0; i < _fx.Length; i++)
        {
            int sel = _fx[i].SelectedIndex;
            if (sel is not (1 or 2))
            {
                continue;
            }

            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            if (preset != 0 && sel == preset)
            {
                continue;
            }

            list.Add(MatriboxMidi.ModuleOnOff(i, sel == 2, channel));
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
        ApplyPatchChainToLabels();
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
                ApplyPatchChainToLabels();
                _loading = false;
                return;
            }

            var keep = MessageBox.Show(
                this,
                "Manter as configurações de efeitos do Chocolate para o banco novo da Matribox?\n\n" +
                "Sim: A, B, C e D continuam com os mesmos On/Off. Só muda o banco da Matribox para onde este banco do Chocolate envia.\n\n" +
                "Não: as configurações de efeitos são substituídas.",
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (keep == DialogResult.Yes)
            {
                KeepEffectsAndRetarget(group, oldPreset);
                return;
            }
        }

        ApplyNewMatriboxPatch(group);
    }

    private void KeepEffectsAndRetarget(Bank group, int oldPresetIndex)
    {
        var oldChain = oldPresetIndex >= 0 && oldPresetIndex < _presets.Count
            ? _presets[oldPresetIndex].Chain
            : null;
        var newChain = CurrentPatchChain();
        int channel = (int)numChannel.Value;
        int[] visible = _fx.Select(c => c.SelectedIndex).ToArray();
        FootswitchConfig current = CurrentSwitch();

        foreach (var sw in group.Switches)
        {
            int[] choices = ReferenceEquals(sw, current)
                ? visible
                : ChoicesFromCommands(sw.ShortPress, oldChain);
            var kept = sw.ShortPress.Where(c => !IsEffectCc(c)).Select(c => c.Clone()).ToList();
            kept.AddRange(EffectsFromChoices(choices, newChain, channel));
            sw.ShortPress = kept;
        }

        int index = SelectedPresetIndex;
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

        bool prev = _loading;
        _loading = true;
        for (int i = 0; i < _fx.Length && i < visible.Length; i++)
        {
            _fx[i].SelectedIndex = visible[i];
        }

        _loading = prev;
        RefreshBankComboLabel(group);
        UpdateHint();
        StyleSave(true);
    }

    private static int[] ChoicesFromCommands(List<MidiCommand> commands, IReadOnlyList<MatriboxChainSlot>? chain)
    {
        var choices = new int[HotoneSysex.SlotCount];
        for (int i = 0; i < choices.Length; i++)
        {
            int stored = MatriboxMidi.EffectChoice(commands, i);
            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            choices[i] = stored != 0 && stored == preset ? 0 : stored;
        }

        return choices;
    }

    private static List<MidiCommand> EffectsFromChoices(
        int[] choices, IReadOnlyList<MatriboxChainSlot>? chain, int channel)
    {
        var list = new List<MidiCommand>();
        for (int i = 0; i < choices.Length; i++)
        {
            int sel = choices[i];
            if (sel is not (1 or 2))
            {
                continue;
            }

            int preset = MatriboxMidi.PresetEffectChoice(chain, i);
            if (preset != 0 && sel == preset)
            {
                continue;
            }

            list.Add(MatriboxMidi.ModuleOnOff(i, sel == 2, channel));
        }

        return list;
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

    private void chkStomp_CheckedChanged(object sender, EventArgs e) => CommitCurrentSwitch();

    private void numChannel_ValueChanged(object sender, EventArgs e) => CommitCurrentSwitch();

    private void btnInicio_Click(object sender, EventArgs e) => Close();

    private async void btnConnect_Click(object sender, EventArgs e)
    {
        if (_usb.IsConnected)
        {
            _usb.Disconnect();
            btnConnect.Text = "Conectar";
            _loading = true;
            FillChocolateBanks();
            _loading = false;
            LoadFromConfig();
            StyleSave(false);
            SetWorkspaceEnabled(false);
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
            btnConnect.Text = "Desligar";
            _config = await _usb.ReadConfigurationAsync();
            _loading = true;
            FillChocolateBanks();
            _loading = false;
            SetWorkspaceEnabled(true);
            bool slimmed = SlimRedundantEffects();
            LoadFromConfig();
            StyleSave(slimmed);
        }
        catch (Exception ex)
        {
            _usb.Disconnect();
            btnConnect.Text = "Conectar";
            SetWorkspaceEnabled(false);
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
