namespace Shared.Behaviors;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheRemovingRequest
{
  private readonly IDatabase _redis;
  private readonly ILogger<CacheRemovingBehavior<TRequest, TResponse>> _logger;

  public CacheRemovingBehavior(IDatabase redis, ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger)
  {
    _redis = redis;
    _logger = logger;
  }

  public async Task<TResponse> Handle(
      TRequest request,
      RequestHandlerDelegate<TResponse> next,
      CancellationToken cancellationToken)
  {
    // Execute the request first
    var response = await next();

    try
    {
      // Remove cache items if enabled
      if (request.CacheKeysToRemove?.Count > 0)
      {
       
        foreach (var groupKey in request.CacheKeysToRemove)
        {
          // Delete the cache keys
          _redis.ListRange(groupKey).ToList().ForEach(k => _redis.KeyDelete(k.ToString()));

          await _redis.KeyDeleteAsync(groupKey);

          _logger.LogDebug(
          "Removed {Count} cache keys for {RequestType}: {Keys}",
          groupKey,
          typeof(TRequest).Name,
          string.Join(", ", request.CacheKeysToRemove));
        }

      
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(
          ex,
          "Error occurred while removing cache items for {RequestType}",
          typeof(TRequest).Name);
    }

    return response;
  }
}

