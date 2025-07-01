using FastEndpoints;
using Users.Managers.Dtos;


namespace Users.Managers.Features.AdminManagers.UpdateManager;

public record UpdateManagerRequest(Guid Id, ManagerDto Manager);
public record UpdateManagerResponse(bool Success);

public class UpdateManagerEndpoint(ISender sender) : Endpoint<UpdateManagerRequest, UpdateManagerResponse>
{
  public override void Configure()
  {
    Put("/admin/managers/{id}");
    AllowAnonymous();
    Description(x => x
        .WithName("UpdateManager")
        .WithTags("AdminManagers")
        .Produces<UpdateManagerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateManagerRequest request, CancellationToken ct)
  {
    var command = new UpdateManagerCommand(request.Id, request.Manager);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateManagerResponse(result.Success), cancellation: ct);
  }
}