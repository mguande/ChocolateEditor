using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace ChocolateEditor.Controls;

public sealed class SuggestItem
{
    public required string Key { get; init; }
    public required string Label { get; init; }
    public string FilterText { get; init; } = "";
    public object? Tag { get; init; }

    public override string ToString() => Label;
}

public sealed class SuggestBox : UserControl
{
    private readonly Panel _frame;
    private readonly TextBox _text;
    private readonly Button _button;
    private readonly ListBox _list;
    private readonly ToolStripDropDown _drop;
    private readonly ToolStripControlHost _host;
    private readonly System.Windows.Forms.Timer _debounce;
    private readonly List<SuggestItem> _items = [];
    private CancellationTokenSource? _searchCts;
    private bool _suppress;
    private bool _ready;

    public SuggestBox()
    {
        Height = 30;
        MinimumSize = new Size(80, 30);
        TabStop = false;

        _text = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F),
            TabStop = true
        };
        _button = new Button
        {
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 8F),
            TabStop = false,
            Text = "▾",
            Width = 22
        };
        _button.FlatAppearance.BorderSize = 0;
        _frame = new Panel
        {
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            Padding = new Padding(6, 5, 0, 4)
        };
        _frame.Controls.Add(_text);
        _frame.Controls.Add(_button);

        _list = new ListBox
        {
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 9.5F),
            IntegralHeight = false
        };
        _host = new ToolStripControlHost(_list)
        {
            AutoSize = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        _drop = new ToolStripDropDown
        {
            AutoClose = false,
            AutoSize = false,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        _drop.Items.Add(_host);

        _debounce = new System.Windows.Forms.Timer { Interval = 280 };
        _debounce.Tick += async (_, _) =>
        {
            _debounce.Stop();
            await RunSearchAsync();
        };

        _text.TextChanged += OnTextChanged;
        _text.KeyDown += OnTextKeyDown;
        _text.Leave += (_, _) => BeginInvoke(CloseIfAway);
        _button.Click += (_, _) => ToggleDrop();
        _button.Leave += (_, _) => BeginInvoke(CloseIfAway);
        _list.MouseUp += (_, e) =>
        {
            int index = _list.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                _list.SelectedIndex = index;
            }

            Accept();
        };

        Controls.Add(_frame);
        _ready = true;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Placeholder
    {
        get => _text.PlaceholderText;
        set => _text.PlaceholderText = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<string, CancellationToken, Task<IReadOnlyList<SuggestItem>>>? RemoteSearch { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int MinRemoteLength { get; set; } = 2;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SuggestItem? SelectedItem { get; private set; }

    public event EventHandler? SelectionChanged;

    public void FocusEditor() => _text.Focus();

    public void SetItems(IReadOnlyList<SuggestItem> items)
    {
        _items.Clear();
        _items.AddRange(items);
        if (SelectedItem is { } current)
        {
            SuggestItem? match = _items.FirstOrDefault(i => i.Key == current.Key);
            SelectedItem = match;
            SetText(match?.Label ?? "");
        }

        CloseDrop();
    }

    public void ClearContents()
    {
        _items.Clear();
        ClearInput();
    }

    public void ClearInput()
    {
        SelectedItem = null;
        SetText("");
        CloseDrop();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _ready = false;
            _debounce.Stop();
            _debounce.Dispose();
            _searchCts?.Cancel();
            _searchCts?.Dispose();
            _drop.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        _text.Focus();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_ready)
        {
            CloseDrop();
        }
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        if (_suppress)
        {
            return;
        }

        if (SelectedItem is not null && !string.Equals(SelectedItem.Label, _text.Text, StringComparison.Ordinal))
        {
            SelectedItem = null;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        _debounce.Stop();
        _debounce.Start();
    }

    private void OnTextKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Down)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            if (!_drop.Visible)
            {
                if (RemoteSearch is null)
                {
                    ShowList(Choices().ToList());
                }
                else if (_items.Count > 0)
                {
                    ShowList(_items.ToList());
                }
                else
                {
                    _debounce.Stop();
                    _ = RunSearchAsync();
                }
            }

            MoveSelection(1);
            return;
        }

        if (e.KeyCode == Keys.Up && _drop.Visible)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            MoveSelection(-1);
            return;
        }

        if (e.KeyCode == Keys.Enter && _drop.Visible)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            Accept();
            return;
        }

        if (e.KeyCode == Keys.Escape && _drop.Visible)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            CloseDrop();
        }
    }

    private void ToggleDrop()
    {
        if (_drop.Visible)
        {
            CloseDrop();
            _text.Focus();
            return;
        }

        if (RemoteSearch is not null && _items.Count == 0 && _text.Text.Trim().Length >= MinRemoteLength)
        {
            _debounce.Stop();
            _ = RunSearchAsync();
            return;
        }

        ShowList(Choices().ToList());
        _text.Focus();
    }

    private IEnumerable<SuggestItem> Choices()
    {
        if (RemoteSearch is not null)
        {
            return _items;
        }

        return Filter(_text.Text.Trim());
    }

    private async Task RunSearchAsync()
    {
        if (_suppress || IsDisposed)
        {
            return;
        }

        string term = _text.Text.Trim();
        if (RemoteSearch is null)
        {
            if (IsDisposed || (!_text.Focused && !_button.Focused))
            {
                return;
            }

            if (term.Length == 0 || IsSelectedText(term))
            {
                CloseDrop();
                return;
            }

            ShowList(Filter(term).ToList());
            return;
        }

        _searchCts?.Cancel();
        _searchCts?.Dispose();
        if (term.Length < MinRemoteLength)
        {
            CloseDrop();
            return;
        }

        _searchCts = new CancellationTokenSource();
        CancellationToken token = _searchCts.Token;
        try
        {
            IReadOnlyList<SuggestItem> found = await RemoteSearch(term, token);
            if (token.IsCancellationRequested || IsDisposed || (!_text.Focused && !_button.Focused))
            {
                return;
            }

            _items.Clear();
            _items.AddRange(found);
            ShowList(found.ToList());
        }
        catch (OperationCanceledException)
        {
        }
        catch
        {
            if (!IsDisposed && !token.IsCancellationRequested && _text.Focused)
            {
                ShowList([Notice("Sem ligação à base")]);
            }
        }
    }

    private IEnumerable<SuggestItem> Filter(string term)
    {
        if (term.Length == 0 || IsSelectedText(term))
        {
            return _items;
        }

        string folded = Fold(term);
        return _items.Where(item =>
            Fold(item.Label).Contains(folded, StringComparison.Ordinal)
            || Fold(item.FilterText).Contains(folded, StringComparison.Ordinal));
    }

    private bool IsSelectedText(string term)
        => SelectedItem is not null && string.Equals(SelectedItem.Label, term, StringComparison.OrdinalIgnoreCase);

    private void ShowList(IReadOnlyList<SuggestItem> items)
    {
        if (IsDisposed || Width < 40)
        {
            return;
        }

        _list.BeginUpdate();
        _list.Items.Clear();
        if (items.Count == 0)
        {
            _list.Items.Add(Notice("Sem resultados"));
        }
        else
        {
            foreach (SuggestItem item in items)
            {
                _list.Items.Add(item);
            }
        }

        _list.EndUpdate();
        _list.SelectedIndex = -1;

        int rows = Math.Max(items.Count, 1);
        int height = Math.Min(rows * Math.Max(_list.ItemHeight, 18) + 4, 260);
        int width = Math.Max(Width, 180);
        _list.Size = new Size(width, height);
        _host.Size = new Size(width, height);
        _drop.Size = new Size(width, height);
        if (!_drop.Visible)
        {
            _drop.Show(this, new Point(0, Height));
        }
    }

    private void MoveSelection(int delta)
    {
        if (_list.Items.Count == 0)
        {
            return;
        }

        int index = _list.SelectedIndex < 0 ? 0 : Math.Clamp(_list.SelectedIndex + delta, 0, _list.Items.Count - 1);
        _list.SelectedIndex = index;
    }

    private void Accept()
    {
        if (_list.SelectedItem is not SuggestItem item || item.Key.Length == 0)
        {
            CloseDrop();
            return;
        }

        SelectedItem = item;
        SetText(item.Label);
        CloseDrop();
        SelectionChanged?.Invoke(this, EventArgs.Empty);
        _text.Focus();
    }

    private void CloseIfAway()
    {
        if (IsDisposed || !_ready || _drop.IsDisposed)
        {
            return;
        }

        bool open;
        try
        {
            open = _drop.Visible;
        }
        catch (NullReferenceException)
        {
            return;
        }
        catch (ObjectDisposedException)
        {
            return;
        }

        if (!open)
        {
            return;
        }

        if (_drop.Bounds.Contains(Cursor.Position) || _text.Focused || _button.Focused)
        {
            return;
        }

        CloseDrop();
    }

    private void CloseDrop()
    {
        if (!_ready || _drop.IsDisposed)
        {
            return;
        }

        try
        {
            if (_drop.Visible)
            {
                _drop.Close();
            }
        }
        catch (NullReferenceException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
    }

    private void SetText(string value)
    {
        _suppress = true;
        _debounce.Stop();
        _searchCts?.Cancel();
        _text.Text = value;
        _text.SelectionStart = _text.Text.Length;
        _suppress = false;
    }

    private static SuggestItem Notice(string label) => new() { Key = "", Label = label };

    private static string Fold(string text)
    {
        string form = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(form.Length);
        foreach (char c in form)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(char.ToLowerInvariant(c));
            }
        }

        return sb.ToString();
    }
}
