using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class TenantCategoryDeactivatedEventHandler(ILogger<TenantCategoryDeactivatedEventHandler> logger) : INotificationHandler<TenantCategoryDeactivatedEvent>
{
  public async Task Handle(TenantCategoryDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "TenantCategoryDeactivatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, TenantId: {TenantId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.TenantId
    );

    // TODO: Implement any additional business logic for tenant category deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}