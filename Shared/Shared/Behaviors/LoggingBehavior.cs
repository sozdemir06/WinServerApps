using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

/// <summary>
/// MediatR pipeline behavior for logging requests and responses
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

  public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);
    var response = await next();
    _logger.LogInformation("Handled {RequestType}", typeof(TRequest).Name);
    return response;
  }
}