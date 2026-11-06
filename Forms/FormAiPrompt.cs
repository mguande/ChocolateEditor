using System.Net;
using ChocolateEditor.Ai;
using ChocolateEditor.Controls;
using ChocolateEditor.Matribox;
using ChocolateEditor.Music;

namespace ChocolateEditor.Forms;

public partial class FormAiPrompt : Form
{
    private readonly CancellationTokenSource _alive = new();
    private MatriboxPatchDraft? _draft;
    private string _externalQuestion = "";
    private bool _fromExternal;
    private bool _busy;
    private bool _catalogLoading;
    private int _catalogGen;
    private int _albumGen;
    private MusicArtist? _artist;
    private MusicAlbum? _album;
    private MusicSong? _song;
    private List<MusicSong> _songs = [];

    public FormAiPrompt()
    {
        InitializeComponent();
        cmbProvider.Items.AddRange(["ChatGPT", "Claude", "Gemini"]);
        var settings = AiSettingsStore.Load();
        cmbProvider.SelectedIndex = settings.LastProvider switch
        {
            AiProvider.Claude => 1,
            AiProvider.Gemini => 2,
            _ => 0
        };
        chkSendManual.Checked = settings.IncludeMatriboxManual;
        SyncManualCheckbox();
        ShowPlain("A resposta aparece aqui, como numa página do browser.");
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        txtPrompt.KeyDown += txtPrompt_KeyDown;
        txtPastedReply.KeyDown += txtPastedReply_KeyDown;
        sugArtist.RemoteSearch = SearchArtistsAsync;
        sugArtist.SelectionChanged += async (_, _) => await ArtistChangedAsync();
        sugAlbum.SelectionChanged += async (_, _) => await AlbumChangedAsync();
        sugSong.SelectionChanged += (_, _) => _song = sugSong.SelectedItem?.Tag as MusicSong;
        SyncProcessButton();
        SyncMusicBoxes();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _alive.Cancel();
        base.OnFormClosed(e);
    }

    private void txtPrompt_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnSend.PerformClick();
        }
    }

    private void txtPastedReply_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnProcessReply.PerformClick();
        }
    }

    private AiProvider SelectedProvider() => cmbProvider.SelectedIndex switch
    {
        1 => AiProvider.Claude,
        2 => AiProvider.Gemini,
        _ => AiProvider.ChatGpt
    };

    private void btnInicio_Click(object sender, EventArgs e) => Close();

    private void btnSettings_Click(object sender, EventArgs e)
    {
        using var form = new FormAiSettings();
        form.ShowDialog(this);
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
        var settings = AiSettingsStore.Load();
        var provider = SelectedProvider();
        if (!settings.HasKey(provider))
        {
            MessageBox.Show(this,
                $"Falta a chave da API do {AiProviderNames.Display(provider)}.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            using var form = new FormAiSettings();
            form.ShowDialog(this);
            return;
        }

        if (!TryQuestion(out string question))
        {
            return;
        }

        settings.LastProvider = provider;
        settings.IncludeMatriboxManual = chkSendManual.Checked;
        AiSettingsStore.Save(settings);

        bool patch = chkPatchMatribox.Checked;
        bool manual = patch && chkSendManual.Checked;
        string prompt = patch ? MatriboxPatchPrompt.WrapUserPrompt(question, manual) : question;
        string? system = patch ? MatriboxPatchPrompt.SystemInstruction(settings.PatchInstruction) : null;

        _draft = null;
        _fromExternal = false;
        btnExportPrst.Enabled = false;
        SetBusy(true);
        ShowPlain("A pedir resposta…");
        try
        {
            string reply = await AiChatClient.AskAsync(settings, provider, prompt, system);
            if (patch && MatriboxPatchPrompt.TryParse(reply, out var draft))
            {
                draft.Question = question;
                draft.Answer = reply;
                _draft = draft;
                btnExportPrst.Enabled = true;
                ShowPatch(reply, draft);
            }
            else
            {
                ShowPlain(reply);
            }
        }
        catch (Exception ex)
        {
            ShowPlain("Erro:\n\n" + ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void btnExportPrst_Click(object sender, EventArgs e)
    {
        if (_draft is null)
        {
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Guardar patch Matribox",
            Filter = "Patch Matribox (*.prst)|*.prst",
            FileName = $"01-A {_draft.FileStem}.prst",
            InitialDirectory = MatriboxPrstImport.FirstExistingDefaultFolder()
                               ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            MatriboxPrstExport.Write(dlg.FileName, _draft);
            string txtPath = Path.ChangeExtension(dlg.FileName, ".txt");
            File.WriteAllText(txtPath, FormatSessionLog(_draft, SessionModelLabel()));
            MessageBox.Show(this,
                $"Gravado:\n{dlg.FileName}\n{txtPath}\n\nCopia o .prst para a pasta Patchs do editor Sonicake.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Guardar .prst", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        btnSend.Enabled = !busy;
        btnBuildPrompt.Enabled = !busy;
        txtPrompt.Enabled = !busy;
        cmbProvider.Enabled = !busy;
        btnSettings.Enabled = !busy;
        chkPatchMatribox.Enabled = !busy;
        SyncManualCheckbox();
        btnExportPrst.Enabled = !busy && _draft is not null;
        btnSend.Text = busy ? "A aguardar…" : "Enviar";
        SyncMusicBoxes();
    }

    private void chkPatchMatribox_CheckedChanged(object? sender, EventArgs e)
        => SyncManualCheckbox();

    private void chkSendManual_CheckedChanged(object? sender, EventArgs e)
    {
        var s = AiSettingsStore.Load();
        s.IncludeMatriboxManual = chkSendManual.Checked;
        AiSettingsStore.Save(s);
    }

    private void SyncManualCheckbox()
    {
        chkSendManual.Enabled = chkPatchMatribox.Enabled && chkPatchMatribox.Checked;
        SyncProcessButton();
    }

    private void SyncProcessButton()
        => btnProcessReply.Text = chkPatchMatribox.Checked ? "Montar patch" : "Mostrar";

    private void SyncMusicBoxes()
    {
        sugArtist.Enabled = !_busy;
        bool ready = !_busy && !_catalogLoading && _artist is not null;
        sugAlbum.Enabled = ready;
        sugSong.Enabled = ready;
    }

    private async Task<IReadOnlyList<SuggestItem>> SearchArtistsAsync(string term, CancellationToken cancellationToken)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(_alive.Token, cancellationToken);
        lblMusicStatus.Text = "A procurar artistas…";
        try
        {
            IReadOnlyList<MusicArtist> artists = await MusicCatalog.SearchArtistsAsync(term, linked.Token);
            if (!IsDisposed && !linked.IsCancellationRequested)
            {
                lblMusicStatus.Text = artists.Count == 0
                    ? "Nenhum artista encontrado."
                    : "Escolhe o artista na lista.";
            }

            return artists.Select(artist => new SuggestItem
            {
                Key = artist.Id.ToString(),
                Label = artist.Genre.Length == 0 ? artist.Name : artist.Name + " · " + artist.Genre,
                FilterText = artist.Name,
                Tag = artist
            }).ToList();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            if (!IsDisposed)
            {
                lblMusicStatus.Text = "Não foi possível consultar a base de músicas.";
            }

            throw;
        }
    }

    private async Task ArtistChangedAsync()
    {
        MusicArtist? next = sugArtist.SelectedItem?.Tag as MusicArtist;
        if (next?.Id == _artist?.Id)
        {
            return;
        }

        int gen = ++_catalogGen;
        _albumGen++;
        _artist = next;
        _album = null;
        _song = null;
        _songs = [];
        sugAlbum.ClearContents();
        sugSong.ClearContents();
        if (_artist is null)
        {
            _catalogLoading = false;
            lblMusicStatus.Text = "Base iTunes. Escreve a banda e a lista vai filtrando.";
            SyncMusicBoxes();
            return;
        }

        _catalogLoading = true;
        SyncMusicBoxes();
        lblMusicStatus.Text = "A carregar álbuns e músicas de " + _artist.Name + "…";
        using var load = CancellationTokenSource.CreateLinkedTokenSource(_alive.Token);
        try
        {
            ArtistCatalog catalog = await MusicCatalog.LoadArtistAsync(_artist.Id, load.Token);
            if (IsDisposed || gen != _catalogGen)
            {
                return;
            }

            _songs = catalog.Songs.ToList();
            sugAlbum.SetItems(catalog.Albums.Select(AlbumItem).ToList());
            ShowSongs(albumId: null);
            lblMusicStatus.Text = catalog.Albums.Count + " álbuns · " + _songs.Count + " músicas";
            if (_songs.Count >= 200)
            {
                lblMusicStatus.Text += ". A lista geral está limitada; escolhe o álbum para ver as faixas.";
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch
        {
            if (!IsDisposed && gen == _catalogGen)
            {
                lblMusicStatus.Text = "Não foi possível carregar os álbuns e as músicas.";
            }
        }
        finally
        {
            if (!IsDisposed && gen == _catalogGen)
            {
                _catalogLoading = false;
                SyncMusicBoxes();
            }
        }
    }

    private async Task AlbumChangedAsync()
    {
        MusicAlbum? next = sugAlbum.SelectedItem?.Tag as MusicAlbum;
        if (next?.Id == _album?.Id)
        {
            return;
        }

        int gen = ++_albumGen;
        int catalogGen = _catalogGen;
        _album = next;
        _song = null;
        sugSong.ClearInput();
        if (_album is null)
        {
            ShowSongs(albumId: null);
            return;
        }

        List<MusicSong> ofAlbum = _songs.Where(s => s.AlbumId == _album.Id).ToList();
        if (ofAlbum.Count == 0)
        {
            lblMusicStatus.Text = "A carregar músicas de " + _album.Name + "…";
            using var load = CancellationTokenSource.CreateLinkedTokenSource(_alive.Token);
            try
            {
                IReadOnlyList<MusicSong> fetched = await MusicCatalog.LoadAlbumSongsAsync(_album.Id, load.Token);
                if (IsDisposed || gen != _albumGen || catalogGen != _catalogGen)
                {
                    return;
                }

                var known = _songs.Select(s => s.Id).ToHashSet();
                foreach (MusicSong song in fetched)
                {
                    if (known.Add(song.Id))
                    {
                        _songs.Add(song);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch
            {
                if (!IsDisposed && gen == _albumGen)
                {
                    lblMusicStatus.Text = "Não foi possível carregar as músicas deste álbum.";
                }

                return;
            }
        }

        if (IsDisposed || gen != _albumGen)
        {
            return;
        }

        ShowSongs(_album.Id);
        int count = _songs.Count(s => s.AlbumId == _album.Id);
        lblMusicStatus.Text = _album.Name + " · " + count + (count == 1 ? " música" : " músicas");
    }

    private void ShowSongs(long? albumId)
    {
        IEnumerable<MusicSong> source = albumId is null
            ? _songs
            : _songs.Where(s => s.AlbumId == albumId).OrderBy(s => s.DiscNumber).ThenBy(s => s.TrackNumber);
        sugSong.SetItems(source.Select(song => new SuggestItem
        {
            Key = song.Id.ToString(),
            Label = albumId is null
                ? song.Name + (song.AlbumName.Length == 0 ? "" : " — " + song.AlbumName)
                : song.TrackNumber > 0 ? song.TrackNumber + ". " + song.Name : song.Name,
            FilterText = song.Name + " " + song.AlbumName,
            Tag = song
        }).ToList());
        _song = sugSong.SelectedItem?.Tag as MusicSong;
    }

    private static SuggestItem AlbumItem(MusicAlbum album) => new()
    {
        Key = album.Id.ToString(),
        Label = album.Year.Length == 0 ? album.Name : album.Name + " (" + album.Year + ")",
        FilterText = album.Name + " " + album.Year,
        Tag = album
    };

    private bool TryQuestion(out string question)
    {
        question = ComposeQuestion();
        if (question.Length > 0)
        {
            return true;
        }

        MessageBox.Show(this,
            "Escolhe uma banda ou música, ou escreve o que queres pedir à IA.",
            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        sugArtist.FocusEditor();
        return false;
    }

    private string ComposeQuestion()
    {
        var sb = new System.Text.StringBuilder();
        if (_artist is not null)
        {
            sb.AppendLine("Artista: " + _artist.Name);
        }

        if (_album is not null)
        {
            sb.AppendLine("Álbum: " + _album.Name);
        }
        else if (_song is { AlbumName.Length: > 0 })
        {
            sb.AppendLine("Álbum: " + _song.AlbumName);
        }

        if (_song is not null)
        {
            sb.AppendLine("Música: " + _song.Name);
        }

        string extra = txtPrompt.Text.Trim();
        if (extra.Length > 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            sb.Append(extra);
        }

        return sb.ToString().Trim();
    }

    private string SessionModelLabel()
        => _fromExternal ? "IA externa (resposta colada)" : AiProviderNames.Display(SelectedProvider());

    private void btnBuildPrompt_Click(object sender, EventArgs e)
    {
        if (!TryQuestion(out string question))
        {
            return;
        }

        bool patch = chkPatchMatribox.Checked;
        bool manual = patch && chkSendManual.Checked;
        _externalQuestion = question;
        string prompt = patch ? MatriboxPatchPrompt.WrapUserPrompt(_externalQuestion, manual) : _externalQuestion;
        string? system = patch
            ? MatriboxPatchPrompt.SystemInstruction(AiSettingsStore.Load().PatchInstruction)
            : null;
        txtGeneratedPrompt.Text = FormatForClipboard(system, prompt);
        btnCopyPrompt.Text = "Copiar";
        ShowExternal();
        tabsExternal.SelectedTab = tabPrompt;
        txtGeneratedPrompt.Focus();
        txtGeneratedPrompt.SelectAll();
    }

    private void btnCopyPrompt_Click(object sender, EventArgs e)
    {
        string text = txtGeneratedPrompt.Text;
        if (text.Length == 0)
        {
            return;
        }

        try
        {
            Clipboard.SetText(text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Copiar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnCopyPrompt.Text = "Copiado";
        txtGeneratedPrompt.Focus();
        txtGeneratedPrompt.SelectAll();
    }

    private void btnProcessReply_Click(object sender, EventArgs e)
    {
        string reply = txtPastedReply.Text;
        if (string.IsNullOrWhiteSpace(reply))
        {
            MessageBox.Show(this,
                "Cola a resposta da IA na aba Resposta.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            tabsExternal.SelectedTab = tabReply;
            txtPastedReply.Focus();
            return;
        }

        bool patch = chkPatchMatribox.Checked;
        if (patch && MatriboxPatchPrompt.TryParse(reply, out var draft))
        {
            draft.Question = string.IsNullOrWhiteSpace(_externalQuestion) ? txtPrompt.Text : _externalQuestion;
            draft.Answer = reply;
            _draft = draft;
            _fromExternal = true;
            ShowPatch(reply, draft);
            return;
        }

        if (patch)
        {
            _draft = null;
            btnExportPrst.Enabled = false;
            MessageBox.Show(this,
                "Não encontrei um patch válido nesta resposta.\n\nA IA tem de devolver o bloco JSON com name e slots.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            tabsExternal.SelectedTab = tabReply;
            txtPastedReply.Focus();
            return;
        }

        _draft = null;
        _fromExternal = true;
        ShowPlain(reply);
    }

    private void btnEditReply_Click(object sender, EventArgs e)
    {
        ShowExternal();
        tabsExternal.SelectedTab = tabReply;
        txtPastedReply.Focus();
    }

    private static string FormatForClipboard(string? system, string prompt)
    {
        if (string.IsNullOrWhiteSpace(system))
        {
            return prompt.TrimEnd() + "\n";
        }

        string instructions = system.Trim();
        if (!instructions.StartsWith("=== Instruções ===", StringComparison.Ordinal))
        {
            instructions = "=== Instruções ===\n" + instructions;
        }

        return instructions
               + "\n\n=== Pedido ===\n"
               + prompt.TrimEnd()
               + "\n";
    }

    private void ShowExternal()
    {
        webReply.Visible = false;
        grpChain.Visible = false;
        panelExternal.Visible = true;
        panelExternal.BringToFront();
    }

    private static string FormatSessionLog(MatriboxPatchDraft draft, string model)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Chocolate Plus Editor — patch Matribox gerado por IA");
        sb.AppendLine("Data: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        sb.AppendLine("Modelo: " + model);
        sb.AppendLine("Patch: " + draft.Name);
        if (!string.IsNullOrWhiteSpace(draft.Style))
        {
            sb.AppendLine("Estilo: " + draft.Style);
        }

        sb.AppendLine();
        sb.AppendLine("=== Pergunta ===");
        sb.AppendLine(draft.Question.Trim());
        sb.AppendLine();
        sb.AppendLine("=== Resposta ===");
        sb.AppendLine(draft.Answer.Trim());
        sb.AppendLine();
        sb.AppendLine("=== Cadeia ===");
        foreach (var slot in draft.Chain.Where(s => !s.Empty))
        {
            string alg = MatriboxAlgCatalog.Name(slot.Algorithm);
            sb.Append(slot.Position + 1).Append("  ")
                .Append(HotoneSysex.ModuleTypeNames[slot.ModuleType]).Append("  ")
                .Append(alg).Append("  ").Append(slot.On ? "On" : "Off");
            if (slot.Parameters.Count > 0)
            {
                sb.Append("  ").AppendJoin(", ", slot.Parameters.Select(p => $"{p.Name}={p.Value}"));
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private void ShowPatch(string reply, MatriboxPatchDraft draft)
    {
        panelExternal.Visible = false;
        webReply.Visible = false;
        grpChain.Visible = true;
        grpChain.BringToFront();
        lblPatchTitle.Text = draft.Name;
        lblPatchMeta.Text = string.Join(" · ", new[] { draft.Style, draft.Note }.Where(s => !string.IsNullOrWhiteSpace(s)));
        lblPatchNote.Text = MatriboxPatchPrompt.ReplyWithoutJson(reply);
        flowChain.SuspendLayout();
        flowChain.Controls.Clear();
        foreach (var slot in draft.Chain.Where(s => !s.Empty))
        {
            string alg = MatriboxAlgCatalog.Name(slot.Algorithm);
            if (string.IsNullOrWhiteSpace(alg))
            {
                alg = slot.EffectName;
            }

            var card = new PatchGearCard(
                PatchGearCard.KindOf(slot.ModuleType),
                HotoneSysex.ModuleTypeNames[slot.ModuleType],
                alg,
                slot.On,
                slot.Parameters);
            flowChain.Controls.Add(card);
        }

        flowChain.ResumeLayout();
        btnExportPrst.Enabled = true;
        btnEditReply.Visible = _fromExternal;
    }

    private void ShowPlain(string text)
    {
        panelExternal.Visible = false;
        grpChain.Visible = false;
        webReply.Visible = true;
        webReply.BringToFront();
        ShowHtml("<pre>" + WebUtility.HtmlEncode(text) + "</pre>", preWrap: true);
    }

    private void ShowHtml(string inner, bool preWrap)
    {
        string html = $$"""
            <!DOCTYPE html>
            <html>
            <head>
            <meta charset="utf-8">
            <style>
              html, body { margin: 0; padding: 0; background: #fff; }
              body {
                font-family: "Segoe UI", Arial, sans-serif;
                font-size: 15px;
                line-height: 1.5;
                color: #111;
                padding: 20px 24px;
                {{(preWrap ? "white-space: pre-wrap;" : "")}}
                word-wrap: break-word;
              }
              h1 { font-size: 22px; margin: 0 0 6px; }
              .meta { color: #555; margin: 0 0 16px; }
              table { border-collapse: collapse; width: 100%; max-width: 720px; margin: 12px 0 20px; }
              th, td { border: 1px solid #ccc; padding: 6px 10px; text-align: left; }
              th { background: #f3f3f3; }
              .hint { color: #333; }
              pre { white-space: pre-wrap; font-family: "Segoe UI", Arial, sans-serif; }
            </style>
            </head>
            <body>
            {{inner}}
            </body></html>
            """;
        webReply.DocumentText = html;
    }
}
