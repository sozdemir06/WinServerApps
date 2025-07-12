using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class TenantCategoryActivatedEventHandler(ILogger<TenantCategoryActivatedEventHandler> logger) : INotificationHandler<TenantCategoryActivatedEvent>
{
  public async Task Handle(TenantCategoryActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "TenantCategoryActivatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, TenantId: {TenantId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.TenantId
    );

    // TODO: Implement any additional business logic for tenant category activation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}