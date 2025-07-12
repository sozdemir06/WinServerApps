using System.Reflection;
using Catalog.Data;
using Catalog.Data.Processors;
using Catalog.Data.Seed;
using Shared.Data.Extensions;

namespace Catalog;

public static class CatalogModule
{
  public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    var connectionString = configuration.GetConnectionString("Database");
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    services.AddDbContext<CatalogDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseNpgsql(connectionString);
    });

    services.AddHostedService<OutboxProcessor>();
    services.AddScoped<IDataSeeder, CatalogDataSeeder>();

    return services;
  }

  public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
  {
    // Configure Catalog module middleware here

    // Apply migrations to the database
    app.UseMigration<CatalogDbContext>();

    return app;
  }
}