using System.Text.Json;

namespace ChocolateEditor.Music;

public sealed record MusicArtist(long Id, string Name, string Genre);

public sealed record MusicAlbum(long Id, string Name, string Year);

public sealed record MusicSong(long Id, string Name, long AlbumId, string AlbumName, int DiscNumber, int TrackNumber);

public sealed record ArtistCatalog(IReadOnlyList<MusicAlbum> Albums, IReadOnlyList<MusicSong> Songs);

public static class MusicCatalog
{
    private static readonly HttpClient Http = new()
    {
        Timeout = TimeSpan.FromSeconds(20)
    };

    static MusicCatalog()
    {
        Http.DefaultRequestHeaders.UserAgent.ParseAdd("ChocolatePlusEditor/1.3.0");
        Http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }

    public static async Task<IReadOnlyList<MusicArtist>> SearchArtistsAsync(string term, CancellationToken cancellationToken)
    {
        using JsonDocument doc = await GetAsync(SearchUrl(term, "musicArtist"), cancellationToken);
        var list = new List<MusicArtist>();
        var seen = new HashSet<long>();
        foreach (JsonElement row in Results(doc))
        {
            if (!TryLong(row, "artistId", out long id) || !seen.Add(id))
            {
                continue;
            }

            string name = Str(row, "artistName");
            if (name.Length == 0)
            {
                continue;
            }

            list.Add(new MusicArtist(id, name, Str(row, "primaryGenreName")));
        }

        return list;
    }

    public static async Task<ArtistCatalog> LoadArtistAsync(long artistId, CancellationToken cancellationToken)
    {
        Task<JsonDocument> albumsTask = GetAsync(LookupUrl(artistId, "album"), cancellationToken);
        Task<JsonDocument> songsTask = GetAsync(LookupUrl(artistId, "song"), cancellationToken);
        await Task.WhenAll(albumsTask, songsTask);
        using JsonDocument albumsDoc = await albumsTask;
        using JsonDocument songsDoc = await songsTask;

        List<MusicAlbum> albums = ReadAlbums(albumsDoc);
        List<MusicSong> songs = ReadSongs(songsDoc);
        var known = albums.Select(a => a.Id).ToHashSet();
        foreach (MusicSong song in songs)
        {
            if (song.AlbumId != 0 && known.Add(song.AlbumId))
            {
                albums.Add(new MusicAlbum(song.AlbumId, song.AlbumName, ""));
            }
        }

        albums.Sort(static (a, b) =>
        {
            int year = string.Compare(YearKey(a.Year), YearKey(b.Year), StringComparison.Ordinal);
            return year != 0
                ? year
                : string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase);
        });
        songs.Sort(static (a, b) =>
        {
            int name = string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase);
            if (name != 0)
            {
                return name;
            }

            int album = string.Compare(a.AlbumName, b.AlbumName, StringComparison.CurrentCultureIgnoreCase);
            return album != 0 ? album : a.TrackNumber.CompareTo(b.TrackNumber);
        });
        return new ArtistCatalog(albums, songs);
    }

    public static async Task<IReadOnlyList<MusicSong>> LoadAlbumSongsAsync(long albumId, CancellationToken cancellationToken)
    {
        using JsonDocument doc = await GetAsync(LookupUrl(albumId, "song"), cancellationToken);
        List<MusicSong> songs = ReadSongs(doc);
        songs.Sort(static (a, b) =>
        {
            int disc = a.DiscNumber.CompareTo(b.DiscNumber);
            if (disc != 0)
            {
                return disc;
            }

            int track = a.TrackNumber.CompareTo(b.TrackNumber);
            return track != 0
                ? track
                : string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase);
        });
        return songs;
    }

    private static List<MusicAlbum> ReadAlbums(JsonDocument doc)
    {
        var list = new List<MusicAlbum>();
        var seen = new HashSet<long>();
        foreach (JsonElement row in Results(doc))
        {
            if (!string.Equals(Str(row, "wrapperType"), "collection", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!TryLong(row, "collectionId", out long id) || !seen.Add(id))
            {
                continue;
            }

            string name = Str(row, "collectionName");
            if (name.Length == 0)
            {
                continue;
            }

            list.Add(new MusicAlbum(id, name, YearOf(Str(row, "releaseDate"))));
        }

        return list;
    }

    private static List<MusicSong> ReadSongs(JsonDocument doc)
    {
        var list = new List<MusicSong>();
        var seen = new HashSet<long>();
        foreach (JsonElement row in Results(doc))
        {
            if (!string.Equals(Str(row, "kind"), "song", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!TryLong(row, "trackId", out long id) || !seen.Add(id))
            {
                continue;
            }

            string name = Str(row, "trackName");
            if (name.Length == 0)
            {
                continue;
            }

            TryLong(row, "collectionId", out long albumId);
            list.Add(new MusicSong(
                id,
                name,
                albumId,
                Str(row, "collectionName"),
                IntOf(row, "discNumber"),
                IntOf(row, "trackNumber")));
        }

        return list;
    }

    private static async Task<JsonDocument> GetAsync(string url, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
    }

    private static string SearchUrl(string term, string entity)
        => "https://itunes.apple.com/search?country=br&limit=20&entity="
           + entity
           + "&term="
           + Uri.EscapeDataString(term);

    private static string LookupUrl(long id, string entity)
        => "https://itunes.apple.com/lookup?country=br&limit=200&entity="
           + entity
           + "&id="
           + id.ToString();

    private static IEnumerable<JsonElement> Results(JsonDocument doc)
    {
        if (!doc.RootElement.TryGetProperty("results", out JsonElement results)
            || results.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        foreach (JsonElement row in results.EnumerateArray())
        {
            yield return row;
        }
    }

    private static string Str(JsonElement row, string name)
        => row.TryGetProperty(name, out JsonElement value) && value.ValueKind == JsonValueKind.String
            ? value.GetString() ?? ""
            : "";

    private static bool TryLong(JsonElement row, string name, out long value)
    {
        value = 0;
        if (!row.TryGetProperty(name, out JsonElement el))
        {
            return false;
        }

        if (el.ValueKind == JsonValueKind.Number)
        {
            return el.TryGetInt64(out value);
        }

        return el.ValueKind == JsonValueKind.String && long.TryParse(el.GetString(), out value);
    }

    private static int IntOf(JsonElement row, string name)
    {
        if (!row.TryGetProperty(name, out JsonElement el))
        {
            return 0;
        }

        if (el.ValueKind == JsonValueKind.Number && el.TryGetInt32(out int value))
        {
            return value;
        }

        return el.ValueKind == JsonValueKind.String && int.TryParse(el.GetString(), out int parsed) ? parsed : 0;
    }

    private static string YearOf(string releaseDate)
        => releaseDate.Length >= 4 && releaseDate.Take(4).All(char.IsDigit) ? releaseDate[..4] : "";

    private static string YearKey(string year) => year.Length == 0 ? "9999" : year;
}
