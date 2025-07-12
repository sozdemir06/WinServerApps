using Catalog.Languages.Dtos;
using Catalog.Languages.Features.CreateLanguage;
using Shared.Messages.Events.Users.Languages;

namespace Catalog.Languages.ConsumerIntegrationEventHandler;

public class LanguageCreatedIntegrationEventHandler(ISender sender, ILogger<LanguageCreatedIntegrationEventHandler> logger) : IConsumer<LanguageCreatedIntegrationEvent>
{
  public async Task Consume(ConsumeContext<LanguageCreatedIntegrationEvent> context)
  {
    try
    {
      var languageDto = context.Message.Adapt<LanguageDto>();
      var result = await sender.Send(new CreateLanguageCommand(languageDto));
      if (result.Success)
      {
        logger.LogInformation("[LanguageCreatedIntegrationEvent] consumed: {LanguageId}", context.Message.Id);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "[LanguageCreatedIntegrationEvent] error: {LanguageId}", context.Message.Id);
    }

    await Task.CompletedTask;
  }
}