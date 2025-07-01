using FastEndpoints;
using Users.AppRoles.Dtos;
using Users.AppRoles.Features.UpdateAppRole;

namespace Modules.Users.Users.AppRoles.Features.UpdateAppRole;

public record UpdateAppRoleRequest(AppRoleDto AppRole);
public record UpdateAppRoleResponse(bool Success);

/// <summary>
/// Endpoint for updating an existing application role
/// </summary>
public class UpdateAppRoleEndpoint(ISender sender) : Endpoint<UpdateAppRoleRequest, UpdateAppRoleResponse>
{
  public override void Configure()
  {
    Put("/admin/app-roles");
    Description(x => x
        .WithName("UpdateAppRole")
        .WithTags("AppRoles")
        .Produces<UpdateAppRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAppRoleRequest request, CancellationToken ct)
  {
    var command = new UpdateAppRoleCommand(request.AppRole);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateAppRoleResponse(result.Success), cancellation: ct);
  }
}