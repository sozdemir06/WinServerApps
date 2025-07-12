using Catalog.AppTenants.Dtos;
using Catalog.AppTenants.Features.UpdateAppTenant;
using Shared.Messages.Events.Users.AppTennats;

namespace Catalog.AppTenants.ConsumerIntegrationEventHandler;

public class AppTenantUpdatedIntegrationEventHandler(ISender sender, ILogger<AppTenantUpdatedIntegrationEventHandler> logger) : IConsumer<AppTenantUpdatedIntegrationEvent>
{
  public async Task Consume(ConsumeContext<AppTenantUpdatedIntegrationEvent> context)
  {
    try
    {
      
      var appTenantDto = context.Message.Adapt<AppTenantDto>();
      var result = await sender.Send(new UpdateAppTenantCommand(appTenantDto));
      if (result.Success)
      {
        logger.LogInformation("[AppTenantUpdatedIntegrationEvent] consumed: {AppTenantId}", context.Message.Id);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "[AppTenantUpdatedIntegrationEvent] error: {AppTenantId}", context.Message.Id);
    }
    await Task.CompletedTask;
  }
}