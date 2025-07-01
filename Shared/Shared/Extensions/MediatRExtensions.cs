using System.Reflection;
using Shared.Behaviors;

namespace Shared.Extensions;

public static class MediatRExtensions
{
  public static IServiceCollection AddMediatRWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
  {
    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(assemblies);
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthoırizeBehavior<,>));
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
    });

    return services;
  }
}