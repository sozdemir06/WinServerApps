using FastEndpoints;

namespace Customers.Currencies.Features.UpdateExchangeRates;

public record UpdateExchangeRatesRequest;
public record UpdateExchangeRatesResponse(int UpdatedCount, List<string> Errors);

/// <summary>
/// Endpoint for updating exchange rates from TCMB
/// </summary>
public class UpdateExchangeRatesEndpoint : Endpoint<UpdateExchangeRatesRequest, UpdateExchangeRatesResponse>
{
  private readonly ISender _sender;

  public UpdateExchangeRatesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/currencies/update-exchange-rates");
    Description(x => x
        .WithName("UpdateExchangeRates")
        .WithTags("Currencies")
        .Produces<UpdateExchangeRatesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateExchangeRatesRequest request, CancellationToken ct)
  {
    var command = new UpdateExchangeRatesCommand();
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateExchangeRatesResponse(result.UpdatedCount, result.Errors), cancellation: ct);
  }
}