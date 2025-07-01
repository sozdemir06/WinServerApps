using System.Text.Json;

namespace Shared.Services.Caching;

/// <summary>
/// Provides methods for generating consistent cache keys
/// </summary>
public static class CacheKeyGenerator
{
  private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = false };

  /// <summary>
  /// Generates a cache key for a request with parameters
  /// </summary>
  /// <param name="prefix">The prefix for the cache key (e.g. entity name)</param>
  /// <param name="parameters">The parameters to include in the cache key</param>
  /// <returns>A consistent cache key string</returns>
  public static string GenerateKey(string prefix, params object[] parameters)
  {
    if (string.IsNullOrWhiteSpace(prefix))
      throw new ArgumentException("Cache key prefix cannot be empty", nameof(prefix));

    var keyParts = new List<string> { prefix };

    if (parameters != null && parameters.Length > 0)
    {
      foreach (var param in parameters)
      {
        if (param == null)
          continue;

        // For simple types, use ToString()
        if (param.GetType().IsPrimitive || param is string || param is DateTime || param is DateTimeOffset)
        {
          keyParts.Add(param.ToString()!);
        }
        else
        {
          // For complex types, serialize to JSON
          var json = JsonSerializer.Serialize(param, _jsonOptions);
          keyParts.Add(json);
        }
      }
    }
    if (keyParts.Count == 1)
      keyParts.Add(prefix);

    return string.Join(":", keyParts);
  }
}

