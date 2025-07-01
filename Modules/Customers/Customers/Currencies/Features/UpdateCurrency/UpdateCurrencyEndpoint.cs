using Customers.Currencies.Dtos;
using FastEndpoints;

namespace Customers.Currencies.Features.UpdateCurrency;

public record UpdateCurrencyRequest(long Id, CurrencyDto Currency);
public record UpdateCurrencyResponse(long Id);

/// <summary>
/// Endpoint for updating a currency
/// </summary>
public class UpdateCurrencyEndpoint : Endpoint<UpdateCurrencyRequest, UpdateCurrencyResponse>
{
  private readonly ISender _sender;

  public UpdateCurrencyEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/currencies/{Id}");
    Description(x => x
        .WithName("UpdateCurrency")
        .WithTags("Currencies")
        .Produces<UpdateCurrencyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateCurrencyRequest request, CancellationToken ct)
  {
    var command = new UpdateCurrencyCommand(request.Id, request.Currency);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateCurrencyResponse(result.Id), cancellation: ct);
  }
}