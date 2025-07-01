using Customers.Countries.QueryParams;
using FastEndpoints;
using Shared.Pagination;

namespace Customers.Countries.Features.GetCountries;

public record GetCountriesRequest : CountryParams;

public record GetCountriesResponse(IEnumerable<CountryDto> Countries, PaginationMetaData MetaData);
public class GetCountriesEndpoint : Endpoint<GetCountriesRequest, GetCountriesResponse>
{
  private readonly ISender _sender;

  public GetCountriesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/countries");
    Description(x => x
        .WithName("GetCountries")
        .WithTags("Countries")
        .Produces<GetCountriesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCountriesRequest request, CancellationToken ct)
  {
    var query = new GetCountriesQuery(request);
    var result = await _sender.Send(query, ct);
    await SendAsync(new GetCountriesResponse(result.Countries, result.MetaData), cancellation: ct);
  }
}

