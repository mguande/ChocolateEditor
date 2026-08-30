using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ChocolateEditor.Ai;

public static class AiChatClient
{
    private static readonly HttpClient Http = new()
    {
        Timeout = TimeSpan.FromMinutes(2)
    };

    public static async Task<string> AskAsync(
        AiSettings settings,
        AiProvider provider,
        string prompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default)
    {
        prompt = prompt.Trim();
        if (prompt.Length == 0)
        {
            throw new InvalidOperationException("Escreve um prompt.");
        }

        return provider switch
        {
            AiProvider.Claude => await AskClaudeAsync(settings, prompt, systemPrompt, cancellationToken),
            AiProvider.Gemini => await AskGeminiAsync(settings, prompt, systemPrompt, cancellationToken),
            _ => await AskOpenAiAsync(settings, prompt, systemPrompt, cancellationToken)
        };
    }

    private static async Task<string> AskOpenAiAsync(
        AiSettings settings, string prompt, string? systemPrompt, CancellationToken cancellationToken)
    {
        if (!settings.HasKey(AiProvider.ChatGpt))
        {
            throw new InvalidOperationException("Falta a chave da API do ChatGPT. Abre Configurar.");
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.OpenAiApiKey.Trim());
        object[] messages = string.IsNullOrWhiteSpace(systemPrompt)
            ? [new { role = "user", content = prompt }]
            : [new { role = "system", content = systemPrompt }, new { role = "user", content = prompt }];
        req.Content = JsonContent(new
        {
            model = settings.ModelOf(AiProvider.ChatGpt),
            messages
        });

        using var res = await Http.SendAsync(req, cancellationToken);
        string body = await res.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        if (!res.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ReadError(doc, $"ChatGPT HTTP {(int)res.StatusCode}"));
        }

        if (doc.RootElement.TryGetProperty("choices", out var choices)
            && choices.GetArrayLength() > 0
            && choices[0].TryGetProperty("message", out var message)
            && message.TryGetProperty("content", out var content)
            && content.ValueKind == JsonValueKind.String)
        {
            return content.GetString() ?? "";
        }

        throw new InvalidOperationException("O ChatGPT não devolveu texto.");
    }

    private static async Task<string> AskClaudeAsync(
        AiSettings settings, string prompt, string? systemPrompt, CancellationToken cancellationToken)
    {
        if (!settings.HasKey(AiProvider.Claude))
        {
            throw new InvalidOperationException("Falta a chave da API do Claude. Abre Configurar.");
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
        req.Headers.TryAddWithoutValidation("x-api-key", settings.ClaudeApiKey.Trim());
        req.Headers.TryAddWithoutValidation("anthropic-version", "2023-06-01");
        req.Content = JsonContent(string.IsNullOrWhiteSpace(systemPrompt)
            ? new
            {
                model = settings.ModelOf(AiProvider.Claude),
                max_tokens = 8192,
                messages = new[] { new { role = "user", content = prompt } }
            }
            : new
            {
                model = settings.ModelOf(AiProvider.Claude),
                max_tokens = 8192,
                system = systemPrompt,
                messages = new[] { new { role = "user", content = prompt } }
            });

        using var res = await Http.SendAsync(req, cancellationToken);
        string body = await res.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        if (!res.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ReadError(doc, $"Claude HTTP {(int)res.StatusCode}"));
        }

        if (doc.RootElement.TryGetProperty("content", out var blocks) && blocks.ValueKind == JsonValueKind.Array)
        {
            var text = new StringBuilder();
            foreach (var block in blocks.EnumerateArray())
            {
                if (block.TryGetProperty("type", out var type)
                    && type.GetString() == "text"
                    && block.TryGetProperty("text", out var part))
                {
                    text.Append(part.GetString());
                }
            }

            if (text.Length > 0)
            {
                return text.ToString();
            }
        }

        throw new InvalidOperationException("O Claude não devolveu texto.");
    }

    private static async Task<string> AskGeminiAsync(
        AiSettings settings, string prompt, string? systemPrompt, CancellationToken cancellationToken)
    {
        if (!settings.HasKey(AiProvider.Gemini))
        {
            throw new InvalidOperationException("Falta a chave da API do Gemini. Abre Configurar.");
        }

        string model = Uri.EscapeDataString(settings.ModelOf(AiProvider.Gemini));
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";
        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.TryAddWithoutValidation("x-goog-api-key", settings.GeminiApiKey.Trim());
        req.Content = JsonContent(string.IsNullOrWhiteSpace(systemPrompt)
            ? new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            }
            : new
            {
                systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            });

        using var res = await Http.SendAsync(req, cancellationToken);
        string body = await res.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        if (!res.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ReadError(doc, $"Gemini HTTP {(int)res.StatusCode}"));
        }

        if (doc.RootElement.TryGetProperty("candidates", out var candidates)
            && candidates.GetArrayLength() > 0
            && candidates[0].TryGetProperty("content", out var content)
            && content.TryGetProperty("parts", out var parts))
        {
            var text = new StringBuilder();
            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var piece))
                {
                    text.Append(piece.GetString());
                }
            }

            if (text.Length > 0)
            {
                return text.ToString();
            }
        }

        if (doc.RootElement.TryGetProperty("promptFeedback", out var feedback)
            && feedback.TryGetProperty("blockReason", out var reason))
        {
            throw new InvalidOperationException($"O Gemini bloqueou o pedido ({reason.GetString()}).");
        }

        throw new InvalidOperationException("O Gemini não devolveu texto.");
    }

    private static StringContent JsonContent(object payload)
        => new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    private static string ReadError(JsonDocument doc, string fallback)
    {
        var root = doc.RootElement;
        if (root.TryGetProperty("error", out var error))
        {
            if (error.ValueKind == JsonValueKind.Object && error.TryGetProperty("message", out var message))
            {
                return message.GetString() ?? fallback;
            }

            if (error.ValueKind == JsonValueKind.String)
            {
                return error.GetString() ?? fallback;
            }
        }

        return fallback;
    }
}
