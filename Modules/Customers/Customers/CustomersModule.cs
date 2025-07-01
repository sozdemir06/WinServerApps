using Customers.Data.Processors;

namespace Customers;

public static class CustomersModule
{
  public static IServiceCollection AddCustomersModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Customers module services here
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    var connectionString = configuration.GetConnectionString("Database");
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    services.AddDbContext<CustomerDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseNpgsql(connectionString);
    });

    services.AddScoped<IDataSeeder, CustomerSeedData>();

    // Register background services
    services.AddHostedService<CurrencyExchangeRateProcessor>();

    return services;
  }

  public static IApplicationBuilder UseCustomersModule(this IApplicationBuilder app)
  {
    // Configure Customers module middleware here

    app.UseMigration<CustomerDbContext>();

    return app;
  }
}