using Customers.Currencies.Dtos;
using FastEndpoints;

namespace Customers.Currencies.Features.GetCurrencyById;

public record GetCurrencyByIdRequest(long Id);
public record GetCurrencyByIdResponse(CurrencyDto Currency);

/// <summary>
/// Endpoint for getting a currency by id
/// </summary>
public class GetCurrencyByIdEndpoint : Endpoint<GetCurrencyByIdRequest, GetCurrencyByIdResponse>
{
  private readonly ISender _sender;

  public GetCurrencyByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/currencies/{Id}");
    Description(x => x
        .WithName("GetCurrencyById")
        .WithTags("Currencies")
        .Produces<GetCurrencyByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCurrencyByIdRequest request, CancellationToken ct)
  {
    var query = new GetCurrencyByIdQuery(request.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetCurrencyByIdResponse(result.Currency), cancellation: ct);
  }
}