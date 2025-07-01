using Microsoft.Extensions.Logging;


namespace Users.AppTenants.DomainEventHandlers;

public class AppTenantCreatedEventHandler(ILogger<AppTenantCreatedEventHandler> logger) : INotificationHandler<AppTenantCreatedEvent>
{
  public async Task Handle(AppTenantCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation("AppTenantCreatedEvent handled =>>"+notification.GetType().Name);
    await Task.CompletedTask;
  }
}