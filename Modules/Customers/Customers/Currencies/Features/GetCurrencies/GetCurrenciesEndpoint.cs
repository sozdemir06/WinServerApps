using Customers.Currencies.Dtos;
using Customers.Currencies.QueryParams;
using FastEndpoints;
using Shared.Pagination;

namespace Customers.Currencies.Features.GetCurrencies;

public record GetCurrenciesRequest():CurrencyParams;
public record GetCurrenciesResponse(IEnumerable<CurrencyDto> Currencies, PaginationMetaData MetaData);

/// <summary>
/// Endpoint for getting currencies with pagination and filtering
/// </summary>
public class GetCurrenciesEndpoint : Endpoint<GetCurrenciesRequest, GetCurrenciesResponse>
{
  private readonly ISender _sender;

  public GetCurrenciesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/currencies");
    Description(x => x
        .WithName("GetCurrencies")
        .WithTags("Currencies")
        .Produces<GetCurrenciesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCurrenciesRequest request, CancellationToken ct)
  {
    var query = new GetCurrenciesQuery(request);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetCurrenciesResponse(result.Currencies, result.MetaData), cancellation: ct);
  }
}