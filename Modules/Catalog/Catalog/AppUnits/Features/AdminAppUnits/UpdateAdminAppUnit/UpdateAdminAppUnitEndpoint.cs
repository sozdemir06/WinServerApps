using Catalog.AppUnits.Dtos;
using FastEndpoints;

namespace Catalog.AppUnits.Features.AdminAppUnits.UpdateAdminAppUnit;

public record UpdateAdminAppUnitRequest(Guid Id, AppUnitDto AppUnit);
public record UpdateAdminAppUnitResponse(Guid Id);

public class UpdateAdminAppUnitEndpoint : Endpoint<UpdateAdminAppUnitRequest, UpdateAdminAppUnitResponse>
{
  private readonly ISender _sender;

  public UpdateAdminAppUnitEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/app-units/{Id}");
    Description(x => x
        .WithName("UpdateAdminAppUnit")
        .WithTags("AppUnits")
        .Produces<UpdateAdminAppUnitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAdminAppUnitRequest req, CancellationToken ct)
  {
    var command = new UpdateAdminAppUnitCommand(req.Id, req.AppUnit);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateAdminAppUnitResponse(result.Id), cancellation: ct);
  }
}