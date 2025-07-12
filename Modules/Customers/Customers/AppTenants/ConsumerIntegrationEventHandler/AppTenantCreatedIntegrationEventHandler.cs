using Customers.AppTenants.Features.CreateAppTenant;

namespace Customers.AppTenants.ConsumerIntegrationEventHandler;

public class AppTenantCreatedIntegrationEventHandler(ISender sender,ILogger<AppTenantCreatedIntegrationEventHandler> logger) : IConsumer<AppTenantCreatedIntegrationEvent>
{


  public async Task Consume(ConsumeContext<AppTenantCreatedIntegrationEvent> context)
  {

     try
     {
      var appTenantDto= context.Message.Adapt<AppTenantDto>();
      var result= await sender.Send(new CreateAppTenantCommand(appTenantDto));
      if(result.Success)
      {
        logger.LogInformation("[AppTenantCreatedIntegrationEvent] consumed: {AppTenantId}", context.Message.Id);
      }
     }
     catch (Exception ex)
     {
      logger.LogError(ex, "[AppTenantCreatedIntegrationEvent] error: {AppTenantId}", context.Message.Id);
     }
    
    await Task.CompletedTask; 
  }
}