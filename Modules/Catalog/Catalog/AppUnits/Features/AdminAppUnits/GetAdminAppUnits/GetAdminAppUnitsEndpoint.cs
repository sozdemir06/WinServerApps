using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.QueryParams;
using FastEndpoints;

namespace Catalog.AppUnits.Features.AdminAppUnits.GetAdminAppUnits;

public record GetAdminAppUnitsRequest() : AppUnitParams;
public record GetAdminAppUnitsResponse(IEnumerable<AppUnitDto> AppUnits, PaginationMetaData MetaData);

public class GetAdminAppUnitsEndpoint : Endpoint<GetAdminAppUnitsRequest, GetAdminAppUnitsResponse>
{
  private readonly ISender _sender;

  public GetAdminAppUnitsEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/app-units");
    Description(x => x
        .WithName("GetAdminAppUnits")
        .WithTags("AppUnits")
        .Produces<GetAdminAppUnitsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminAppUnitsRequest req, CancellationToken ct)
  {
    var query = new GetAdminAppUnitsQuery(req);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminAppUnitsResponse(result.AppUnits, result.MetaData), cancellation: ct);
  }
}