namespace Notifications;

public static class NotificationsModule
{
  public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
  {
    // Register Notifications module services here

    return services;
  }

  public static IApplicationBuilder UseNotificationsModule(this IApplicationBuilder app)
  {
    // Configure Notifications module middleware here

    return app;
  }
}