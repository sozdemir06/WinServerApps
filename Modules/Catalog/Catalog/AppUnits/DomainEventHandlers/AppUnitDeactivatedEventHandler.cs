using Catalog.AppUnits.DomainEvents;

namespace Catalog.AppUnits.DomainEventHandlers;

public class AppUnitDeactivatedEventHandler(ILogger<AppUnitDeactivatedEventHandler> logger) : INotificationHandler<AppUnitDeactivatedEvent>
{
  public async Task Handle(AppUnitDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppUnitDeactivatedEvent handled => AppUnitId: {AppUnitId}, MeasureUnitType: {MeasureUnitType}",
        notification.AppUnit.Id,
        notification.AppUnit.MeasureUnitType
    );

    // TODO: Implement any additional business logic for app unit deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}