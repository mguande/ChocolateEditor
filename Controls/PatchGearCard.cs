namespace ChocolateEditor.Controls;

public sealed class PatchGearCard : Control
{
    public enum Kind
    {
        Pedal,
        Amp,
        Cab
    }

    public Kind Gear { get; }
    public string GearTitle { get; }
    public string Algorithm { get; }
    public bool EnabledOn { get; }
    public IReadOnlyList<Matribox.MatriboxParamValue> Parameters { get; }

    public PatchGearCard(
        Kind gear,
        string title,
        string algorithm,
        bool on,
        IReadOnlyList<Matribox.MatriboxParamValue> parameters)
    {
        Gear = gear;
        GearTitle = title;
        Algorithm = algorithm;
        EnabledOn = on;
        Parameters = parameters;
        Size = new Size(168, 268);
        Margin = new Padding(8, 8, 8, 8);
        DoubleBuffered = true;
    }

    public static Kind KindOf(int moduleType) => moduleType switch
    {
        4 => Kind.Amp,
        5 or 6 => Kind.Cab,
        _ => Kind.Pedal
    };

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        var g = e.Graphics;
        Rectangle body = new(8, 8, Width - 16, Height - 16);
        Color shell = Gear switch
        {
            Kind.Amp => Color.FromArgb(48, 42, 38),
            Kind.Cab => Color.FromArgb(92, 62, 36),
            _ => Color.FromArgb(28, 92, 58)
        };
        if (GearTitle is "DRV")
        {
            shell = Color.FromArgb(160, 90, 28);
        }
        else if (GearTitle is "DLY" or "RVB")
        {
            shell = Color.FromArgb(32, 72, 140);
        }
        else if (GearTitle is "MOD" or "WAH" or "FREQ")
        {
            shell = Color.FromArgb(88, 48, 120);
        }
        else if (GearTitle is "EQ" or "DYN")
        {
            shell = Color.FromArgb(36, 110, 72);
        }

        using var fill = new SolidBrush(EnabledOn ? shell : Color.FromArgb(70, 70, 74));
        using var pen = new Pen(Color.FromArgb(20, 20, 22), 2);
        g.FillRounded(body, 10, fill);
        g.DrawRounded(body, 10, pen);

        if (Gear == Kind.Amp)
        {
            DrawAmp(g, body);
        }
        else if (Gear == Kind.Cab)
        {
            DrawCab(g, body);
        }
        else
        {
            DrawPedal(g, body);
        }

        using var fontTitle = new Font("Segoe UI", 8F, FontStyle.Bold);
        using var fontAlg = new Font("Segoe UI", 9F, FontStyle.Bold);
        using var fontKnob = new Font("Segoe UI", 6.5F);
        using var white = new SolidBrush(Color.White);
        using var muted = new SolidBrush(Color.FromArgb(220, 220, 220));
        var sf = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
        g.DrawString(GearTitle, fontTitle, muted, new RectangleF(body.X, body.Y + 6, body.Width, 16), sf);
        g.DrawString(Algorithm, fontAlg, white, new RectangleF(body.X + 6, body.Y + 22, body.Width - 12, 36), sf);

        var knobs = Parameters.Take(6).ToList();
        int cols = Math.Min(3, Math.Max(1, knobs.Count));
        int startY = body.Y + 62;
        for (int i = 0; i < knobs.Count; i++)
        {
            int col = i % cols;
            int row = i / cols;
            int x = body.X + 10 + col * (body.Width - 20) / cols;
            int y = startY + row * 52;
            DrawKnob(g, x + 8, y, knobs[i], fontKnob, white, muted);
        }

        using var led = new SolidBrush(EnabledOn ? Color.FromArgb(80, 255, 120) : Color.FromArgb(60, 40, 40));
        g.FillEllipse(led, body.Right - 22, body.Y + 8, 10, 10);
    }

    private static void DrawPedal(Graphics g, Rectangle body)
    {
        using var foot = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
        g.FillEllipse(foot, body.X + body.Width / 2 - 16, body.Bottom - 36, 32, 18);
        using var silver = new Pen(Color.FromArgb(180, 180, 180), 1.5f);
        g.DrawEllipse(silver, body.X + body.Width / 2 - 16, body.Bottom - 36, 32, 18);
    }

    private static void DrawAmp(Graphics g, Rectangle body)
    {
        var grille = new Rectangle(body.X + 14, body.Bottom - 78, body.Width - 28, 52);
        using var dark = new SolidBrush(Color.FromArgb(24, 22, 20));
        g.FillRectangle(dark, grille);
        using var line = new Pen(Color.FromArgb(90, 80, 70), 1);
        for (int y = grille.Y + 4; y < grille.Bottom; y += 4)
        {
            g.DrawLine(line, grille.X + 2, y, grille.Right - 2, y);
        }
    }

    private static void DrawCab(Graphics g, Rectangle body)
    {
        int cx = body.X + body.Width / 2;
        int cy = body.Bottom - 52;
        using var black = new SolidBrush(Color.FromArgb(28, 24, 20));
        using var ring = new Pen(Color.FromArgb(200, 180, 140), 3);
        g.FillEllipse(black, cx - 28, cy - 28, 56, 56);
        g.DrawEllipse(ring, cx - 28, cy - 28, 56, 56);
        g.DrawEllipse(ring, cx - 10, cy - 10, 20, 20);
    }

    private static void DrawKnob(
        Graphics g, int x, int y, Matribox.MatriboxParamValue p, Font font, Brush white, Brush muted)
    {
        float t = p.Max == p.Min ? 0 : (p.Value - p.Min) / (float)(p.Max - p.Min);
        float ang = -140 + t * 280;
        using var cap = new SolidBrush(Color.FromArgb(40, 40, 44));
        using var rim = new Pen(Color.FromArgb(200, 200, 200), 1.5f);
        g.FillEllipse(cap, x, y, 22, 22);
        g.DrawEllipse(rim, x, y, 22, 22);
        double rad = (ang - 90) * Math.PI / 180;
        int x2 = x + 11 + (int)(Math.Cos(rad) * 8);
        int y2 = y + 11 + (int)(Math.Sin(rad) * 8);
        using var needle = new Pen(Color.FromArgb(255, 200, 80), 2);
        g.DrawLine(needle, x + 11, y + 11, x2, y2);
        var sf = new StringFormat { Alignment = StringAlignment.Center };
        g.DrawString(p.Name, font, muted, new RectangleF(x - 14, y + 22, 50, 12), sf);
        g.DrawString(p.Value.ToString(), font, white, new RectangleF(x - 14, y + 32, 50, 12), sf);
    }
}

internal static class GraphicsRound
{
    public static void FillRounded(this Graphics g, Rectangle r, int radius, Brush brush)
    {
        using var path = RoundPath(r, radius);
        g.FillPath(brush, path);
    }

    public static void DrawRounded(this Graphics g, Rectangle r, int radius, Pen pen)
    {
        using var path = RoundPath(r, radius);
        g.DrawPath(pen, path);
    }

    private static System.Drawing.Drawing2D.GraphicsPath RoundPath(Rectangle r, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
