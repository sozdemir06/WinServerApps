namespace Catalog;

public static class CatalogModule
{
  public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Catalog module services here

    return services;
  }

  public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
  {
    // Configure Catalog module middleware here

    return app;
  }
}