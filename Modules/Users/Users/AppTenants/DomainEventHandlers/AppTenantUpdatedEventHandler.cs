using MassTransit;


namespace Users.AppTenants.DomainEventHandlers;

public class AppTenantUpdatedEventHandler(ILogger<AppTenantUpdatedEventHandler> logger,IBus bus) : INotificationHandler<AppTenantUpdatedEvent>
{
  public async Task Handle(AppTenantUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation("[AppTenantUpdatedEventHandler] handled event: {EventType}", notification.GetType().Name);

    try
    {
      var message=notification.AppTenant.Adapt<AppTenantUpdatedIntegrationEvent>();
      logger.LogInformation("AppTenantUpdatedEvent handled =>>"+message.TenantCode);
      if(message!=null){
        await bus.Publish(message, cancellationToken);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error handling AppTenantUpdatedEvent");
    }

      await Task.CompletedTask;
  }
}