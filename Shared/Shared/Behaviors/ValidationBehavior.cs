using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;

/// <summary>
/// Pipeline behavior for handling validation of requests using FluentValidation
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!_validators.Any()) return await next();

    var context = new ValidationContext<TRequest>(request);
    var validationResults = await Task.WhenAll(
        _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

    var failures = validationResults
        .SelectMany(r => r.Errors)
        .Where(f => f != null)
        .ToList();

    if (failures.Count != 0)
      throw new ValidationException(failures);

    return await next();
  }
}