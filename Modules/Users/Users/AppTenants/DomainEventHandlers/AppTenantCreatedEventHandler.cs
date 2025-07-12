



using MassTransit;

namespace Users.AppTenants.DomainEventHandlers;

public class AppTenantCreatedEventHandler(ILogger<AppTenantCreatedEventHandler> logger,IBus bus) : INotificationHandler<AppTenantCreatedEvent>
{
  public async Task Handle(AppTenantCreatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      var message=notification.AppTenant.Adapt<AppTenantCreatedIntegrationEvent>();
      logger.LogInformation("AppTenantCreatedEvent handled =>>"+message.TenantCode);
      if(message!=null){
        await bus.Publish(message, cancellationToken);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error handling AppTenantCreatedEvent");
    }

    await Task.CompletedTask;
  }
}