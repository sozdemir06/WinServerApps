using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class AdminCategoryDeactivatedEventHandler(ILogger<AdminCategoryDeactivatedEventHandler> logger) : INotificationHandler<AdminCategoryDeactivatedEvent>
{
  public async Task Handle(AdminCategoryDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AdminCategoryDeactivatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}",
        notification.Category.Id,
        notification.Category.IsActive
    );

    // TODO: Implement any additional business logic for category deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}