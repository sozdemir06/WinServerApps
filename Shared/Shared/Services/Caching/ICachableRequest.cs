namespace Shared.Services.Caching;

/// <summary>
/// Interface for requests that can be cached
/// </summary>
public interface ICachableRequest
{
  /// <summary>
  /// Gets the cache key for this request
  /// </summary>
  string CacheKey { get; }

  /// <summary>
  /// Gets the cache key prefix for this request
  /// </summary>
  string CacheGroupKey { get; }

  /// <summary>
  /// Gets the cache expiration time for this request
  /// </summary>
  TimeSpan? CacheExpiration { get; }

}

