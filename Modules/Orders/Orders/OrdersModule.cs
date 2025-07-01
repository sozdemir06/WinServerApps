namespace Orders;

public static class OrdersModule
{
  public static IServiceCollection AddOrdersModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Orders module services here

    return services;
  }

  public static IApplicationBuilder UseOrdersModule(this IApplicationBuilder app)
  {
    // Configure Orders module middleware here

    return app;
  }
}