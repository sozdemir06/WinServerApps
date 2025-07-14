using FastEndpoints;

namespace Catalog.AppUnits.Features.AdminAppUnits.DeleteAdminAppUnit;

public record DeleteAdminAppUnitRequest(Guid Id);
public record DeleteAdminAppUnitResponse(Guid Id);

public class DeleteAdminAppUnitEndpoint : Endpoint<DeleteAdminAppUnitRequest, DeleteAdminAppUnitResponse>
{
  private readonly ISender _sender;

  public DeleteAdminAppUnitEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/app-units/{Id}");
    Description(x => x
        .WithName("DeleteAdminAppUnit")
        .WithTags("AppUnits")
        .Produces<DeleteAdminAppUnitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAdminAppUnitRequest req, CancellationToken ct)
  {
    var command = new DeleteAdminAppUnitCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteAdminAppUnitResponse(result.Id), cancellation: ct);
  }
}