using Customers.Districts.QueryParams;
using FastEndpoints;
using Shared.Pagination;

namespace Customers.Districts.Features.GetDistricts;

public record GetDistrictsRequest() : DistrictParams;
public record GetDistrictsResponse(IEnumerable<DistrictDto> Districts, PaginationMetaData MetaData);

public class GetDistrictsEndpoint(ISender sender) : Endpoint<GetDistrictsRequest, GetDistrictsResponse>
{
  public override void Configure()
  {
    Get("/admin/districts");
    Description(x => x
        .WithName("GetDistricts")
        .WithTags("Districts")
        .Produces<GetDistrictsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetDistrictsRequest request, CancellationToken ct)
  {
    var query = new GetDistrictsQuery(request);
    var result = await sender.Send(query, ct);
    await SendAsync(new GetDistrictsResponse(result.Districts, result.MetaData), cancellation: ct);
  }
}