namespace Accounting;

public static class AccountingModule
{
  public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Accounting module services here

    return services;
  }

  public static IApplicationBuilder UseAccountingModule(this IApplicationBuilder app)
  {
    // Configure Accounting module middleware here

    return app;
  }
}