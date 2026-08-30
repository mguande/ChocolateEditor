using System.Text.Json;

namespace ChocolateEditor.Ai;

public sealed class AiSettings
{
    public string OpenAiApiKey { get; set; } = "";
    public string OpenAiModel { get; set; } = "gpt-4o-mini";
    public string ClaudeApiKey { get; set; } = "";
    public string ClaudeModel { get; set; } = "claude-sonnet-4-5";
    public string GeminiApiKey { get; set; } = "";
    public string GeminiModel { get; set; } = "gemini-2.5-flash";
    public AiProvider LastProvider { get; set; } = AiProvider.ChatGpt;
    public bool IncludeMatriboxManual { get; set; }

    public bool HasKey(AiProvider provider) => provider switch
    {
        AiProvider.Claude => !string.IsNullOrWhiteSpace(ClaudeApiKey),
        AiProvider.Gemini => !string.IsNullOrWhiteSpace(GeminiApiKey),
        _ => !string.IsNullOrWhiteSpace(OpenAiApiKey)
    };

    public string ModelOf(AiProvider provider) => provider switch
    {
        AiProvider.Claude => string.IsNullOrWhiteSpace(ClaudeModel) ? "claude-sonnet-4-5" : ClaudeModel.Trim(),
        AiProvider.Gemini => string.IsNullOrWhiteSpace(GeminiModel) ? "gemini-2.5-flash" : GeminiModel.Trim(),
        _ => string.IsNullOrWhiteSpace(OpenAiModel) ? "gpt-4o-mini" : OpenAiModel.Trim()
    };
}

public static class AiSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string FilePath => AppConfig.File("ai-settings.json");

    public static AiSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return new AiSettings();
            }

            return JsonSerializer.Deserialize<AiSettings>(File.ReadAllText(FilePath), JsonOptions)
                   ?? new AiSettings();
        }
        catch
        {
            return new AiSettings();
        }
    }

    public static void Save(AiSettings settings)
    {
        string? dir = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(FilePath, JsonSerializer.Serialize(settings, JsonOptions));
    }
}
