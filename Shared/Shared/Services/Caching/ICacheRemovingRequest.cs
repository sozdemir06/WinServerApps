namespace Shared.Services.Caching;

/// <summary>
/// Interface for requests that need to remove items from the cache
/// </summary>
public interface ICacheRemovingRequest
{
  /// <summary>
  /// Gets the cache keys to remove
  /// </summary>
  List<string> CacheKeysToRemove { get; }



}

