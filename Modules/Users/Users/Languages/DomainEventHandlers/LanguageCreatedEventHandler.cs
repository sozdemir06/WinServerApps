using Microsoft.Extensions.Logging;
using Users.Languages.DomainEvents;

namespace Users.Languages.DomainEventHandlers;

public class LanguageCreatedEventHandler(ILogger<LanguageCreatedEventHandler> logger) : INotificationHandler<LanguageCreatedEvent>
{
  public async Task Handle(LanguageCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "LanguageCreatedEvent handled => LanguageId: {LanguageId}, Name: {LanguageName}, Code: {LanguageCode}",
        notification.Language.Id,
        notification.Language.Name,
        notification.Language.Code);

    // TODO: Implement any additional business logic for language creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}