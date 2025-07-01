using Microsoft.Extensions.Configuration;

namespace Shared.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedisExtensions(this IServiceCollection services, IConfiguration configuration)
    {
    // Add Redis services
    var redisConnectionString = configuration.GetSection("Caching:RedisConnectionString").Value;
    if (string.IsNullOrEmpty(redisConnectionString))
    {
      throw new InvalidOperationException("Redis connection string is not configured.");
    }

    // Register Redis ConnectionMultiplexer as a singleton
    // Redis Configuration
    services.AddSingleton<IDatabase>(sp =>
    {
      var redis = ConnectionMultiplexer.Connect(redisConnectionString ?? "localhost:6379");
      return redis.GetDatabase();
    });
      return services;
    }
}