using System.Reflection;
using Accounting.Data.Processors;
using Accounting.Data.Seed;
using Shared.Data.Extensions;

namespace Accounting;

public static class AccountingModule
{
  public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    var connectionString = configuration.GetConnectionString("Database");
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    services.AddDbContext<AccountingDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseNpgsql(connectionString);
    });

    services.AddHostedService<OutboxProcessor>();
    services.AddScoped<IDataSeeder, AccountingDataSeeder>();

    return services;
  }

  public static IApplicationBuilder UseAccountingModule(this IApplicationBuilder app)
  {
    // Configure Accounting module middleware here

    // Apply migrations to the database
    app.UseMigration<AccountingDbContext>();

    return app;
  }
}