using System.Runtime.InteropServices;
using ForeverButton = ReaLTaiizor.Controls.ForeverButton;

namespace ChocolateEditor.Ui;

public static class UiTheme
{
    public static readonly Color Bg = Color.FromArgb(18, 18, 20);
    public static readonly Color Surface = Color.FromArgb(28, 28, 32);
    public static readonly Color Surface2 = Color.FromArgb(40, 40, 46);
    public static readonly Color Text = Color.FromArgb(236, 236, 240);
    public static readonly Color Muted = Color.FromArgb(168, 168, 176);
    public static readonly Color Border = Color.FromArgb(72, 72, 80);
    public static readonly Color Chocolate = Color.FromArgb(168, 78, 36);
    public static readonly Color Blue = Color.FromArgb(36, 110, 176);
    public static readonly Color Teal = Color.FromArgb(24, 120, 118);
    public static readonly Color Accent = Color.FromArgb(196, 92, 42);

    public static void UseDarkTitleBar(Form form)
    {
        if (!form.IsHandleCreated)
        {
            return;
        }

        int dark = 1;
        _ = DwmSetWindowAttribute(form.Handle, 20, ref dark, sizeof(int));
        _ = DwmSetWindowAttribute(form.Handle, 19, ref dark, sizeof(int));
    }

    public static void ApplyTree(Control root)
    {
        ApplyOne(root);
        foreach (Control child in root.Controls)
        {
            ApplyTree(child);
        }
    }

    public static void StyleTile(ForeverButton button, Color fill)
    {
        button.Rounded = true;
        button.BaseColor = fill;
        button.TextColor = Color.White;
        button.BackColor = Color.Transparent;
        button.Font = new Font("Segoe UI Semibold", 11F);
        button.Cursor = Cursors.Hand;
        button.Invalidate();
    }

    public static void StyleAction(ForeverButton button, Color fill)
    {
        StyleTile(button, fill);
        button.Font = new Font("Segoe UI Semibold", button.Width < 100 ? 8.25F : 9.75F);
        button.Invalidate();
    }

    public static void StyleForeverButton(ForeverButton button)
    {
        StyleAction(button, FillForName(button.Name));
    }

    public static Color FillForName(string name)
    {
        if (name.Contains("Matribox", StringComparison.OrdinalIgnoreCase)
            && name.Contains("Connect", StringComparison.OrdinalIgnoreCase))
        {
            return Teal;
        }

        if (ContainsAny(name, "Connect", "Send", "Write", "Save", "Ok", "Guardar", "Export", "Apply", "Import"))
        {
            return name.Contains("Connect", StringComparison.OrdinalIgnoreCase) ? Blue : Accent;
        }

        if (ContainsAny(name, "Copiar", "Copy", "Map", "Scan", "Test"))
        {
            return Accent;
        }

        return Surface2;
    }

    private static bool ContainsAny(string name, params string[] parts)
        => parts.Any(p => name.Contains(p, StringComparison.OrdinalIgnoreCase));

    private static void ApplyOne(Control c)
    {
        if (c is ForeverButton forever)
        {
            StyleForeverButton(forever);
            return;
        }

        if (c.GetType().Namespace?.StartsWith("ReaLTaiizor", StringComparison.Ordinal) == true)
        {
            return;
        }

        switch (c)
        {
            case WebBrowser:
                return;
            case Form form:
                form.BackColor = Bg;
                form.ForeColor = Text;
                break;
            case MenuStrip menu:
                menu.Renderer = new DarkToolStripRenderer();
                menu.BackColor = Surface;
                menu.ForeColor = Text;
                break;
            case StatusStrip status:
                status.Renderer = new DarkToolStripRenderer();
                status.BackColor = Surface;
                status.ForeColor = Text;
                break;
            case ToolStrip strip:
                strip.Renderer = new DarkToolStripRenderer();
                strip.BackColor = Surface;
                strip.ForeColor = Text;
                break;
            case Button button:
                StyleStandardButton(button);
                break;
            case TextBox box:
                box.BackColor = Surface2;
                box.ForeColor = Text;
                box.BorderStyle = BorderStyle.FixedSingle;
                break;
            case ComboBox combo:
                combo.FlatStyle = FlatStyle.Flat;
                combo.BackColor = Surface2;
                combo.ForeColor = Text;
                break;
            case ListBox list:
                list.BackColor = Surface2;
                list.ForeColor = Text;
                list.BorderStyle = BorderStyle.FixedSingle;
                break;
            case NumericUpDown num:
                num.BackColor = Surface2;
                num.ForeColor = Text;
                break;
            case CheckBox or RadioButton:
                c.ForeColor = Text;
                break;
            case Label label:
                ThemeLabel(label);
                break;
            case GroupBox group:
                group.ForeColor = Text;
                group.BackColor = Surface;
                break;
            case TreeView tree:
                tree.BackColor = Surface2;
                tree.ForeColor = Text;
                tree.BorderStyle = BorderStyle.None;
                tree.LineColor = Border;
                break;
            case TabPage page:
                page.BackColor = Surface;
                page.ForeColor = Text;
                break;
            case SplitContainer split:
                split.BackColor = Bg;
                split.Panel1.BackColor = Bg;
                split.Panel2.BackColor = Bg;
                break;
            case Panel or FlowLayoutPanel or TableLayoutPanel or UserControl:
                if (IsLight(c.BackColor) || c.BackColor == SystemColors.Control)
                {
                    c.BackColor = c.Dock == DockStyle.Top || c.Dock == DockStyle.Bottom
                        ? Surface
                        : Bg;
                }

                c.ForeColor = Text;
                break;
        }
    }

    private static void StyleStandardButton(Button button)
    {
        bool primary = button.Name.Contains("Write", StringComparison.OrdinalIgnoreCase)
                       || button.Name.Contains("Send", StringComparison.OrdinalIgnoreCase)
                       || button.Name.Contains("Save", StringComparison.OrdinalIgnoreCase)
                       || button.Name.Contains("Ok", StringComparison.OrdinalIgnoreCase)
                       || button.Name.Contains("Connect", StringComparison.OrdinalIgnoreCase)
                       || button.Name.Contains("Export", StringComparison.OrdinalIgnoreCase);
        button.FlatStyle = FlatStyle.Flat;
        button.UseVisualStyleBackColor = false;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        if (primary)
        {
            button.BackColor = Accent;
            button.ForeColor = Color.White;
        }
        else
        {
            button.BackColor = Surface2;
            button.ForeColor = Text;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Border;
        }
    }

    private static void ThemeLabel(Label label)
    {
        if (label.ForeColor.GetBrightness() > 0.7f)
        {
            return;
        }

        bool gray = Math.Abs(label.ForeColor.R - label.ForeColor.G) < 18
                    && Math.Abs(label.ForeColor.G - label.ForeColor.B) < 18;
        label.ForeColor = gray || label.ForeColor.GetBrightness() is > 0.22f and < 0.55f
            ? Muted
            : Text;
    }

    private static bool IsLight(Color color)
        => color.A > 0 && color.GetBrightness() > 0.55f;

    private sealed class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new DarkColorTable())
        {
            RoundedEdges = false;
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Text;
            base.OnRenderItemText(e);
        }
    }

    private sealed class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => Surface;
        public override Color MenuStripGradientEnd => Surface;
        public override Color MenuBorder => Border;
        public override Color MenuItemBorder => Border;
        public override Color MenuItemSelected => Surface2;
        public override Color MenuItemSelectedGradientBegin => Surface2;
        public override Color MenuItemSelectedGradientEnd => Surface2;
        public override Color MenuItemPressedGradientBegin => Accent;
        public override Color MenuItemPressedGradientEnd => Accent;
        public override Color ToolStripDropDownBackground => Surface;
        public override Color ImageMarginGradientBegin => Surface;
        public override Color ImageMarginGradientMiddle => Surface;
        public override Color ImageMarginGradientEnd => Surface;
        public override Color SeparatorDark => Border;
        public override Color SeparatorLight => Border;
        public override Color StatusStripGradientBegin => Surface;
        public override Color StatusStripGradientEnd => Surface;
        public override Color ToolStripGradientBegin => Surface;
        public override Color ToolStripGradientMiddle => Surface;
        public override Color ToolStripGradientEnd => Surface;
        public override Color ToolStripBorder => Border;
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);
}
