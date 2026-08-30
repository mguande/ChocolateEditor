using ChocolateEditor.Matribox;

namespace ChocolateEditor.Controls;

/// <summary>
/// Pedal da cadeia: clique liga/desliga o efeito que o Chocolate vai mandar no toque curto.
/// </summary>
public sealed class ChainStompPedal : Control
{
    public int SlotIndex { get; }
    public int ModuleType { get; }
    public string ModuleName { get; }
    public string Algorithm { get; }

    private bool _engaged;

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool Engaged
    {
        get => _engaged;
        set
        {
            if (_engaged == value)
            {
                return;
            }

            _engaged = value;
            Invalidate();
            EngagedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? EngagedChanged;

    public ChainStompPedal(int slotIndex, int moduleType, string algorithm, bool engaged)
    {
        SlotIndex = slotIndex;
        ModuleType = moduleType;
        ModuleName = HotoneSysex.ModuleName(moduleType);
        Algorithm = string.IsNullOrWhiteSpace(algorithm) ? ModuleName : algorithm;
        _engaged = engaged;
        Size = new Size(112, 178);
        Margin = new Padding(4, 8, 4, 8);
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    public void SetEngagedSilent(bool engaged)
    {
        _engaged = engaged;
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        if (Enabled)
        {
            Engaged = !_engaged;
        }

        base.OnClick(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
        Invalidate();
        base.OnEnabledChanged(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        var kind = PatchGearCard.KindOf(ModuleType);
        Rectangle body = new(10, 6, Width - 20, 122);
        Color shell = ShellColor(ModuleName, kind);
        if (!Enabled)
        {
            shell = Color.FromArgb(48, 48, 52);
        }
        else if (!_engaged)
        {
            shell = Color.FromArgb(56, 56, 62);
        }

        using var fill = new SolidBrush(shell);
        using var edge = new Pen(_engaged && Enabled ? Color.FromArgb(255, 210, 90) : Color.FromArgb(28, 28, 32), 2);
        g.FillRounded(body, 12, fill);
        g.DrawRounded(body, 12, edge);

        using var jack = new SolidBrush(Color.FromArgb(20, 20, 22));
        g.FillEllipse(jack, 2, body.Y + body.Height / 2 - 6, 12, 12);
        g.FillEllipse(jack, Width - 14, body.Y + body.Height / 2 - 6, 12, 12);

        if (kind == PatchGearCard.Kind.Amp)
        {
            DrawAmp(g, body);
        }
        else if (kind == PatchGearCard.Kind.Cab)
        {
            DrawCab(g, body);
        }
        else
        {
            DrawPedalFoot(g, body);
        }

        using var led = new SolidBrush(_engaged && Enabled
            ? Color.FromArgb(90, 255, 120)
            : Color.FromArgb(50, 28, 28));
        g.FillEllipse(led, body.Right - 20, body.Y + 8, 10, 10);

        using var fontMod = new Font("Segoe UI", 8F, FontStyle.Bold);
        using var fontAlg = new Font("Segoe UI", 8F, FontStyle.Bold);
        using var fontState = new Font("Segoe UI", 8F, FontStyle.Bold);
        using var white = new SolidBrush(Enabled ? Color.White : Color.FromArgb(140, 140, 144));
        using var muted = new SolidBrush(Color.FromArgb(210, 210, 216));
        var sf = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
        g.DrawString(ModuleName, fontMod, muted, new RectangleF(body.X, body.Y + 8, body.Width, 16), sf);
        g.DrawString(Algorithm, fontAlg, white, new RectangleF(body.X + 6, body.Y + 26, body.Width - 12, 36), sf);

        string state = _engaged ? "ON" : "OFF";
        using var stateBrush = new SolidBrush(_engaged && Enabled
            ? Color.FromArgb(90, 255, 140)
            : Color.FromArgb(160, 160, 168));
        g.DrawString(state, fontState, stateBrush, new RectangleF(body.X, body.Bottom - 22, body.Width, 16), sf);

        using var nameFont = new Font("Segoe UI", 8F);
        using var nameBrush = new SolidBrush(Enabled ? Color.FromArgb(220, 220, 226) : Color.FromArgb(120, 120, 126));
        g.DrawString(
            Algorithm,
            nameFont,
            nameBrush,
            new RectangleF(2, body.Bottom + 8, Width - 4, 36),
            sf);
    }

    private static Color ShellColor(string module, PatchGearCard.Kind kind)
    {
        if (module is "DRV")
        {
            return Color.FromArgb(168, 88, 28);
        }

        if (module is "DLY" or "RVB")
        {
            return Color.FromArgb(36, 78, 148);
        }

        if (module is "MOD" or "WAH" or "FREQ")
        {
            return Color.FromArgb(96, 52, 128);
        }

        if (module is "EQ" or "DYN")
        {
            return Color.FromArgb(36, 118, 76);
        }

        return kind switch
        {
            PatchGearCard.Kind.Amp => Color.FromArgb(72, 58, 48),
            PatchGearCard.Kind.Cab => Color.FromArgb(108, 72, 40),
            _ => Color.FromArgb(32, 102, 64)
        };
    }

    private static void DrawPedalFoot(Graphics g, Rectangle body)
    {
        using var foot = new SolidBrush(Color.FromArgb(40, 0, 0, 0));
        g.FillEllipse(foot, body.X + body.Width / 2 - 18, body.Bottom - 40, 36, 20);
        using var silver = new Pen(Color.FromArgb(190, 190, 190), 1.5f);
        g.DrawEllipse(silver, body.X + body.Width / 2 - 18, body.Bottom - 40, 36, 20);
    }

    private static void DrawAmp(Graphics g, Rectangle body)
    {
        var grille = new Rectangle(body.X + 12, body.Bottom - 48, body.Width - 24, 28);
        using var dark = new SolidBrush(Color.FromArgb(24, 22, 20));
        g.FillRectangle(dark, grille);
        using var line = new Pen(Color.FromArgb(90, 80, 70), 1);
        for (int y = grille.Y + 3; y < grille.Bottom; y += 3)
        {
            g.DrawLine(line, grille.X + 2, y, grille.Right - 2, y);
        }
    }

    private static void DrawCab(Graphics g, Rectangle body)
    {
        int cx = body.X + body.Width / 2;
        int cy = body.Bottom - 36;
        using var black = new SolidBrush(Color.FromArgb(28, 24, 20));
        using var ring = new Pen(Color.FromArgb(200, 180, 140), 2.5f);
        g.FillEllipse(black, cx - 22, cy - 22, 44, 44);
        g.DrawEllipse(ring, cx - 22, cy - 22, 44, 44);
        g.DrawEllipse(ring, cx - 8, cy - 8, 16, 16);
    }
}

public sealed class ChainCable : Control
{
    public ChainCable()
    {
        Size = new Size(22, 178);
        Margin = new Padding(0, 8, 0, 8);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        int y = 66;
        using var cable = new Pen(Color.FromArgb(90, 90, 96), 3);
        g.DrawLine(cable, 0, y, Width, y);
        using var jack = new SolidBrush(Color.FromArgb(40, 40, 44));
        g.FillEllipse(jack, Width / 2 - 4, y - 4, 8, 8);
    }
}
