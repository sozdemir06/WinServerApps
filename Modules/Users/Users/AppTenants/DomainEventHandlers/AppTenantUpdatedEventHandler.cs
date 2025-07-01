using Microsoft.Extensions.Logging;

namespace Users.AppTenants.DomainEventHandlers;

public class AppTenantUpdatedEventHandler(ILogger<AppTenantUpdatedEventHandler> logger) : INotificationHandler<AppTenantUpdatedEvent>
{
  public async Task Handle(AppTenantUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation("[AppTenantUpdatedEventHandler] handled event: {EventType}", notification.GetType().Name);

    // var message = new AppTenantUpdatedIntegrationEvent
    // {
    //   AppTenantId = notification.AppTenant.Id,
    //   Name = notification.AppTenant.Name,
    //   Host = notification.AppTenant.Host,
    //   Phone = notification.AppTenant.Phone,
    //   TenantCode = notification.AppTenant.TenantCode,
    //   IsEnabledWebUi = notification.AppTenant.IsEnabledWebUi,
    //   Description = notification.AppTenant.Description,
    //   AllowedBranchNumber = notification.AppTenant.AllowedBranchNumber,
    //   IsActive = notification.AppTenant.IsActive,
    //   AdminEmail = notification.AppTenant.AdminEmail,
    //   SubscriptionEndDate = notification.AppTenant.SubscriptionEndDate,
    //   SubscriptionStartDate = notification.AppTenant.SubscriptionStartDate,
    //   TenantType = notification.AppTenant.TenantType,
    //   MaxUserCount = notification.AppTenant.MaxUserCount,
    //   CreatedBy = notification.AppTenant.CreatedBy?.ToString() ?? string.Empty,
    //   CreatedAt = notification.AppTenant.CreatedAt
    // };
   
   // await bus.Publish(message, cancellationToken);
     await Task.CompletedTask;
  }
}