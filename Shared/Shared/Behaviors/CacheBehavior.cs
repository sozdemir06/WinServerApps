using System.Text.Json;
using Microsoft.Extensions.Configuration;
namespace Shared.Behaviors;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachableRequest
{
  private readonly IDatabase _redis;
  private readonly ILogger<CacheBehavior<TRequest, TResponse>> _logger;
  private readonly IConfiguration _configuration;
  private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = false };

  public CacheBehavior(
      IDatabase redis,
      ILogger<CacheBehavior<TRequest, TResponse>> logger,
      IConfiguration configuration)
  {
    _redis = redis;
    _logger = logger;
    _configuration = configuration;
  }

  private TimeSpan GetCacheExpiration(TimeSpan? requestExpiration)
  {
    if (requestExpiration.HasValue)
    {
      return TimeSpan.FromMinutes(int.Parse(requestExpiration.Value.TotalMinutes.ToString()));
    }

    var defaultExpirationStr = _configuration.GetValue<string>("Caching:DefaultExpiration");
    if (string.IsNullOrEmpty(defaultExpirationStr))
    {
      return TimeSpan.FromMinutes(30); // Fallback to 30 minutes if not configured
    }

    return TimeSpan.FromMinutes(int.Parse(defaultExpirationStr));
  }

  public async Task<TResponse> Handle(
      TRequest request,
      RequestHandlerDelegate<TResponse> next,
      CancellationToken cancellationToken)
  {
    try
    {
      // Try to get from cache
      var cachedValue = await _redis.StringGetAsync(request.CacheKey);
      if (!cachedValue.IsNull)
      {
        return JsonSerializer.Deserialize<TResponse>(cachedValue!, _jsonOptions)!;
      }

      // If not in cache, execute the request
      var response = await next(cancellationToken);

      // Cache the response if it's not null
      if (response != null)
      {
        var serialized = JsonSerializer.Serialize(response, _jsonOptions);
        var expiration = GetCacheExpiration(request.CacheExpiration);

        await _redis.StringSetAsync(
            request.CacheKey,
            serialized,
            expiration);

        // If CacheGroupKey is provided, add the key to the group list
        if (!string.IsNullOrEmpty(request.CacheGroupKey))
        {
          await _redis.ListRightPushAsync(request.CacheGroupKey, request.CacheKey);

          // Set expiration for the group list
          await _redis.KeyExpireAsync(request.CacheGroupKey, expiration);
        }
      }

      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError(
          ex,
          "Error occurred while caching {RequestType} with key {CacheKey}",
          typeof(TRequest).Name,
          request.CacheKey);

      // If caching fails, still execute the request
      return await next(cancellationToken);
    }
  }
}
