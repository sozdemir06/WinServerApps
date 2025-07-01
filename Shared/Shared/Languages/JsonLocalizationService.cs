using System.Text.Json;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace Shared.Languages;

public class JsonLocalizationService : ILocalizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDatabase _redis;
    private readonly Dictionary<string, Dictionary<string, string>> _localizations;
    private const string CacheKeyPrefix = "localization:";
    private const int CacheExpiryMinutes = 60;

    public JsonLocalizationService(
        IHttpContextAccessor httpContextAccessor,
        IDatabase redis)
    {
        _httpContextAccessor = httpContextAccessor;
        _redis = redis;
        _localizations = LoadAllLocalizations();
    }

    private Dictionary<string, Dictionary<string, string>> LoadAllLocalizations()
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        var assemblyLocation = typeof(JsonLocalizationService).Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
        var path = Path.Combine(assemblyDirectory, "Languages/Localization"); 

        foreach (var file in Directory.GetFiles(path, "*.json"))
        {
            var culture = Path.GetFileNameWithoutExtension(file);
            var json = File.ReadAllText(file);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            result[culture] = dict!;

            // Cache the translations in Redis
            CacheTranslations(culture, dict!);
        }

        return result;
    }

    private void CacheTranslations(string culture, Dictionary<string, string> translations)
    {
        var cacheKey = $"{CacheKeyPrefix}{culture}";
        var json = JsonSerializer.Serialize(translations);
        _redis.StringSet(cacheKey, json, TimeSpan.FromMinutes(CacheExpiryMinutes));
    }

    private async Task<Dictionary<string, string>?> GetCachedTranslationsAsync(string culture)
    {
        try
        {
            var cacheKey = $"{CacheKeyPrefix}{culture}";
            var cachedJson = await _redis.StringGetAsync(cacheKey);

            if (!cachedJson.HasValue)
                return null;

            return JsonSerializer.Deserialize<Dictionary<string, string>>(cachedJson!);
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> Translate(string key)
    {
        var culture = GetRequestCulture();

        // Try to get from Redis cache first
        var cachedTranslations = GetCachedTranslationsAsync(culture).GetAwaiter().GetResult();
        if (cachedTranslations != null && cachedTranslations.TryGetValue(key, out var cachedValue))
        {
            return await Task.FromResult(cachedValue);
        }

        // Fallback to in-memory dictionary
        if (_localizations.TryGetValue(culture, out var dict) && dict.TryGetValue(key, out var value))
        {
            return await Task.FromResult(value);
        }

        return await Task.FromResult(key); // fallback: return the key itself
    }

    private string GetRequestCulture()
    {
        var lang = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();
        return string.IsNullOrWhiteSpace(lang) ? "en" : lang.Split(',')[0].Split('-')[0]; // "en-US" → "en"
    }
}
