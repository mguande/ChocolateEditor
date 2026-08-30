namespace ChocolateEditor.Ai;

public enum AiProvider
{
    ChatGpt,
    Claude,
    Gemini
}

public static class AiProviderNames
{
    public static string Display(AiProvider provider) => provider switch
    {
        AiProvider.Claude => "Claude",
        AiProvider.Gemini => "Gemini",
        _ => "ChatGPT"
    };
}
