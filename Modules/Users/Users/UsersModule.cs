using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Data.Interceptors;
using Shared.Data.Seed;
using Shared.Services.Http;
using Users.Data.Processors;
using Users.Data.Services.Securities;


namespace Users;

public static class UsersModule
{
  public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    var connectionString = configuration.GetConnectionString("Database");
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    services.AddDbContext<UserDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseNpgsql(connectionString);
    });

    services.AddScoped<IDataSeeder, UserDataSeeder>();
    services.AddHostedService<OutboxProcessor>();

    services.AddScoped<IManagerTokenService, ManagerTokenService>();
    services.AddScoped<IGetManagerRoleService, GetManagerRoleService>();
    return services;
  }

  public static IApplicationBuilder UseUsersModule(this IApplicationBuilder app)
  {
    // Configure Users module middleware here

    // Apply migrations to the database
    app.UseMigration<UserDbContext>();


    return app;
  }
}