using Users.Languages.DomainEvents;

namespace Users.Languages.DomainEventHandlers;

public class LanguageSetAsDefaultEventHandler(ILogger<LanguageSetAsDefaultEventHandler> logger) : INotificationHandler<LanguageSetAsDefaultEvent>
{
  public async Task Handle(LanguageSetAsDefaultEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "LanguageSetAsDefaultEvent handled => LanguageId: {LanguageId}, Name: {LanguageName}, Code: {LanguageCode}",
        notification.Language.Id,
        notification.Language.Name,
        notification.Language.Code);

    // TODO: Implement any additional business logic for setting language as default
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}