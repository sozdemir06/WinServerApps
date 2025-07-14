using Catalog.AppUnits.DomainEvents;

namespace Catalog.AppUnits.DomainEventHandlers;

public class AppUnitUpdatedEventHandler(ILogger<AppUnitUpdatedEventHandler> logger) : INotificationHandler<AppUnitUpdatedEvent>
{
  public async Task Handle(AppUnitUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppUnitUpdatedEvent handled => AppUnitId: {AppUnitId}, MeasureUnitType: {MeasureUnitType}, IsActive: {IsActive}",
        notification.AppUnit.Id,
        notification.AppUnit.MeasureUnitType,
        notification.AppUnit.IsActive
    );

    // TODO: Implement any additional business logic for app unit update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}