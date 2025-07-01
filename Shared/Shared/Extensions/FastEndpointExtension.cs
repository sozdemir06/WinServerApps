using System.Reflection;
using FastEndpoints;

namespace Shared.Extensions;

public static class FastEndpointExtension
{
  public static IServiceCollection AddFastEndpointsWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
  {
    services.AddFastEndpoints(options =>
    {
      options.Assemblies = assemblies;
    });

    return services;
  }
}

public class GlobalPreProcessor : IGlobalPreProcessor
{
  public Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
  {
    // Add global pre-processing logic here
    return Task.CompletedTask;
  }
}

public class GlobalPostProcessor : IGlobalPostProcessor
{
  public Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
  {
    // Add global post-processing logic here
    return Task.CompletedTask;
  }
}
