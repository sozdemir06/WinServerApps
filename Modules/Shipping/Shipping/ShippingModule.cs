namespace Shipping;

public static class ShippingModule
{
  public static IServiceCollection AddShippingModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Shipping module services here

    return services;
  }

  public static IApplicationBuilder UseShippingModule(this IApplicationBuilder app)
  {
    // Configure Shipping module middleware here

    return app;
  }
}