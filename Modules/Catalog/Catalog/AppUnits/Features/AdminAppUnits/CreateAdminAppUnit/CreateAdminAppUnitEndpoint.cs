using Catalog.AppUnits.Dtos;
using FastEndpoints;

namespace Catalog.AppUnits.Features.AdminAppUnits.CreateAdminAppUnit;

public record CreateAdminAppUnitRequest(AppUnitDto AppUnit);
public record CreateAdminAppUnitResponse(Guid Id);

public class CreateAdminAppUnitEndpoint : Endpoint<CreateAdminAppUnitRequest, CreateAdminAppUnitResponse>
{
  private readonly ISender _sender;

  public CreateAdminAppUnitEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/app-units");
    Description(x => x
        .WithName("CreateAdminAppUnit")
        .WithTags("AppUnits")
        .Produces<CreateAdminAppUnitResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAdminAppUnitRequest req, CancellationToken ct)
  {
    var command = new CreateAdminAppUnitCommand(req.AppUnit);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateAdminAppUnitResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}