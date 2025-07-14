using Catalog.AppUnits.Dtos;
using FastEndpoints;

namespace Catalog.AppUnits.Features.AdminAppUnits.GetAdminAppUnitById;

public record GetAdminAppUnitByIdRequest(Guid Id);
public record GetAdminAppUnitByIdResponse(AppUnitDto AppUnit);

public class GetAdminAppUnitByIdEndpoint : Endpoint<GetAdminAppUnitByIdRequest, GetAdminAppUnitByIdResponse>
{
  private readonly ISender _sender;

  public GetAdminAppUnitByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/app-units/{Id}");
    Description(x => x
        .WithName("GetAdminAppUnitById")
        .WithTags("AppUnits")
        .Produces<GetAdminAppUnitByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminAppUnitByIdRequest req, CancellationToken ct)
  {
    var query = new GetAdminAppUnitByIdQuery(req.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminAppUnitByIdResponse(result.AppUnit), cancellation: ct);
  }
}