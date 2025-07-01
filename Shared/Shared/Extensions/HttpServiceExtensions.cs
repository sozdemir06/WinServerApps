using Shared.Services.Http;

namespace Shared.Extensions;

public static class HttpServiceExtensions
{
  public static IServiceCollection AddHttpServices(this IServiceCollection services)
  {
    // Register HTTP services
    services.AddScoped<IExchangeRateService, ExchangeRateService>();
    services.AddScoped<IGetManagerRoleService, GetManagerRoleService>();

    return services;
  }
}