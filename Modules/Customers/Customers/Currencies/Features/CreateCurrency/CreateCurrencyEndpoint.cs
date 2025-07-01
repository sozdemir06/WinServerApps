using Customers.Currencies.Dtos;
using FastEndpoints;

namespace Customers.Currencies.Features.CreateCurrency;

public record CreateCurrencyRequest(CurrencyDto Currency);
public record CreateCurrencyResponse(long Id);

/// <summary>
/// Endpoint for creating a new currency
/// </summary>
public class CreateCurrencyEndpoint : Endpoint<CreateCurrencyRequest, CreateCurrencyResponse>
{
  private readonly ISender _sender;

  public CreateCurrencyEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/currencies");
    Description(x => x
        .WithName("CreateCurrency")
        .WithTags("Currencies")
        .Produces<CreateCurrencyResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateCurrencyRequest request, CancellationToken ct)
  {
    var command = new CreateCurrencyCommand(request.Currency);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateCurrencyResponse(result.Id), cancellation: ct);
  }
}