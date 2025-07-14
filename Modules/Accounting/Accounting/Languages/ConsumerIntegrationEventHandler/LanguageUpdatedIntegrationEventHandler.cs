using Accounting.Languages.Dtos;
using Accounting.Languages.Features.UpdateLanguage;
using Shared.Messages.Events.Users.Languages;

namespace Accounting.Languages.ConsumerIntegrationEventHandler;

public class LanguageUpdatedIntegrationEventHandler(ISender sender, ILogger<LanguageUpdatedIntegrationEventHandler> logger) : IConsumer<LanguageUpdatedIntegrationEvent>
{
  public async Task Consume(ConsumeContext<LanguageUpdatedIntegrationEvent> context)
  {
    try
    {
      var languageDto = context.Message.Adapt<LanguageDto>();
      var result = await sender.Send(new UpdateLanguageCommand(languageDto));
      if (result.Success)
      {
        logger.LogInformation("[LanguageUpdatedIntegrationEvent] consumed: {LanguageId}", context.Message.Id);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "[LanguageUpdatedIntegrationEvent] error: {LanguageId}", context.Message.Id);
    }

    await Task.CompletedTask;
  }
}