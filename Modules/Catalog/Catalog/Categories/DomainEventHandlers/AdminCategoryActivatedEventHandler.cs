using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class AdminCategoryActivatedEventHandler(ILogger<AdminCategoryActivatedEventHandler> logger) : INotificationHandler<AdminCategoryActivatedEvent>
{
  public async Task Handle(AdminCategoryActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AdminCategoryActivatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}",
        notification.Category.Id,
        notification.Category.IsActive
    );

    // TODO: Implement any additional business logic for category activation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}