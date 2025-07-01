using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Shared.Messages.Extensions;

public static class MassTransitExtensions
{
  public static IServiceCollection AddMassTransitWithAssemblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
  {
    services.AddMassTransit(config => 
    {
      config.SetKebabCaseEndpointNameFormatter();
      config.SetInMemorySagaRepositoryProvider();
      config.AddConsumers(assemblies);
      config.AddSagaStateMachines(assemblies);
      config.AddSagas(assemblies);
      config.AddActivities(assemblies);
      config.UsingRabbitMq((context, cfg) => 
      {
        cfg.Host(new Uri(configuration["MessageBroker:RabbitMQ:Host"]!), h =>
        {
          h.Username(configuration["MessageBroker:RabbitMQ:Username"]!);
          h.Password(configuration["MessageBroker:RabbitMQ:Password"]!);
        });
        cfg.ConfigureEndpoints(context);
      });
    });

    return services;
  }
}