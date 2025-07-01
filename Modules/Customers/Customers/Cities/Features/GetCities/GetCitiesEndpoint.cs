using Customers.Cities.QueryParams;
using FastEndpoints;
using Shared.Pagination;

namespace Customers.Cities.Features.GetCities;

public record GetCitiesRequest : CityParams;
public record GetCitiesResponse(IEnumerable<CityDto> Cities, PaginationMetaData MetaData);
public class GetCitiesEndpoint : Endpoint<GetCitiesRequest, GetCitiesResponse>
{
  private readonly ISender _sender;

  public GetCitiesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/cities");
    Description(x => x
        .WithName("GetCities")
        .WithTags("Cities")
        .Produces<GetCitiesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCitiesRequest request, CancellationToken ct)
  {
    var query = new GetCitiesQuery(request);
    var result = await _sender.Send(query, ct);
    await SendAsync(new GetCitiesResponse(result.Cities, result.MetaData), cancellation: ct);
  }
}

