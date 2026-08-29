using System.Text.Json;

namespace ChocolateEditor.Matribox;

/// <summary>
/// Nomes de algoritmos do alg_data.json do editor Sonicake (fxid → "COMP 1").
/// </summary>
public static class MatriboxAlgCatalog
{
    private static readonly string[] JsonPaths =
    [
        @"C:\Program Files\Sonicake\Matribox II PRO\data\flutter_assets\assets\alg\alg_data.json",
        Path.Combine(AppContext.BaseDirectory, "alg_data.json")
    ];

    private static Dictionary<int, string>? _byFxid;

    public static string Name(int fxid)
    {
        Ensure();
        return _byFxid is not null && _byFxid.TryGetValue(fxid, out string? name) ? name : "";
    }

    private static void Ensure()
    {
        if (_byFxid is not null)
        {
            return;
        }

        _byFxid = [];
        string? path = JsonPaths.FirstOrDefault(File.Exists);
        if (path is null)
        {
            return;
        }

        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("Modules", out var modules))
            {
                return;
            }

            foreach (var module in modules.EnumerateArray())
            {
                if (!module.TryGetProperty("alg", out var algs))
                {
                    continue;
                }

                foreach (var alg in algs.EnumerateArray())
                {
                    if (!alg.TryGetProperty("fxid", out var fx))
                    {
                        continue;
                    }

                    string name = alg.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = alg.TryGetProperty("fxtitle", out var t) ? t.GetString() ?? "" : "";
                    }

                    if (fx.TryGetInt32(out int id) && !string.IsNullOrWhiteSpace(name))
                    {
                        _byFxid[id] = name;
                    }
                }
            }
        }
        catch
        {
            // catálogo em falta — a UI fica só com DYN/AMP/…
        }
    }
}
