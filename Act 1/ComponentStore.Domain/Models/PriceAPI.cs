using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ComponentStore.Domain;

public static class PriceAPI
{
    private const int RequestTimeoutSeconds = 180;
    private static readonly Dictionary<string, string> DotEnvValues = LoadDotEnvValues();
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds)
    };

    public static async Task<PriceLookupResult> SearchProductPricesAsync(string query, string country = "us", int limit = 3)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new PriceLookupResult([], "No product name was provided for price lookup.");
        }

        string? apiKey = GetApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return new PriceLookupResult([], "No API key configured. Define PRICESAPI_KEY in environment variables or in the project root .env file.");
        }

        string url = $"https://api.pricesapi.io/api/v1/products/search?q={Uri.EscapeDataString(query)}&country={country}&limit={limit}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            using HttpResponseMessage response = await Client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                double retryAfter = response.Headers.RetryAfter?.Delta?.TotalSeconds ?? 5;
                return new PriceLookupResult([], $"API is busy. Retry in {retryAfter:0} seconds.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new PriceLookupResult([], $"Price lookup failed (HTTP {(int)response.StatusCode}).");
            }

            string body = await response.Content.ReadAsStringAsync();

            ApiResponse? parsed = JsonSerializer.Deserialize<ApiResponse>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            List<PriceSuggestion> suggestions = parsed?.Data?.Products?
                .Where(p => p.Price > 0)
                .Select(p => new PriceSuggestion(
                    p.Title ?? "Untitled",
                    p.Price,
                    string.IsNullOrWhiteSpace(p.Currency) ? "USD" : p.Currency,
                    string.IsNullOrWhiteSpace(p.Source) ? "Unknown" : p.Source))
                .ToList() ?? [];

            if (suggestions.Count == 0)
            {
                return new PriceLookupResult([], "No reference prices were found for that product name.");
            }

            return new PriceLookupResult(suggestions, null);
        }
        catch (TaskCanceledException)
        {
            return new PriceLookupResult([], $"Price lookup timed out and was canceled (timeout: {RequestTimeoutSeconds}s).");
        }
        catch (HttpRequestException ex)
        {
            return new PriceLookupResult([], $"Network error while looking up prices: {ex.Message}");
        }
    }

    private sealed class ApiResponse
    {
        public DataResult? Data { get; set; }
    }

    private sealed class DataResult
    {
        public List<Product>? Products { get; set; }
    }

    private sealed class Product
    {
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public string? Source { get; set; }
    }

    private static string? GetApiKey()
    {
        string? envValue = Environment.GetEnvironmentVariable("PRICESAPI_KEY")?.Trim();
        if (!string.IsNullOrWhiteSpace(envValue)) return envValue;

        return DotEnvValues.TryGetValue("PRICESAPI_KEY", out string? fileValue)
            ? fileValue.Trim()
            : null;
    }

    private static Dictionary<string, string> LoadDotEnvValues()
    {
        string? dotEnvPath = FindDotEnvPath();
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(dotEnvPath) || !File.Exists(dotEnvPath)) return values;

        foreach (string rawLine in File.ReadLines(dotEnvPath))
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith("#")) continue;

            int separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0) continue;

            string currentKey = line[..separatorIndex].Trim();
            string value = line[(separatorIndex + 1)..].Trim();
            values[currentKey] = Unquote(value);
        }

        return values;
    }

    private static string? FindDotEnvPath()
    {
        string[] rootsToProbe = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };

        foreach (string root in rootsToProbe)
        {
            for (DirectoryInfo? dir = new DirectoryInfo(root); dir is not null; dir = dir.Parent)
            {
                string dotEnvPath = Path.Combine(dir.FullName, ".env");
                if (File.Exists(dotEnvPath)) return dotEnvPath;
            }
        }

        return null;
    }

    private static string Unquote(string value)
        => value.Length >= 2 && value.StartsWith('"') && value.EndsWith('"')
            ? value[1..^1]
            : value;
}

public sealed record PriceSuggestion(string Title, decimal Price, string Currency, string Source);

public sealed record PriceLookupResult(IReadOnlyList<PriceSuggestion> Suggestions, string? Message);