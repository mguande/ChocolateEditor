namespace ChocolateEditor;

/// <summary>
/// Ficheiros locais com os nomes de sempre, em %LocalAppData%\ChocolateEditor\.
/// Os <c>config/*.modelo</c> no repositório são só exemplos para o Git.
/// </summary>
public static class AppConfig
{
    public static string DirectoryPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ChocolateEditor");

    public static string File(string name)
    {
        Directory.CreateDirectory(DirectoryPath);
        return Path.Combine(DirectoryPath, name);
    }
}
