using FastEndpoints;
using Users.UserRoles.Dtos;
using Users.UserRoles.Features.UpdateUserRole;

namespace Modules.Users.Users.UserRoles.Features.UpdateUserRole;

public record UpdateUserRoleRequest(UserRoleDto UserRole);
public record UpdateUserRoleResponse(bool Success);

public class UpdateUserRoleEndpoint(ISender sender) : Endpoint<UpdateUserRoleRequest, UpdateUserRoleResponse>
{
  public override void Configure()
  {
    Put("/user-roles");
    Description(x => x
        .WithName("UpdateUserRole")
        .WithTags("UserRoles")
        .Produces<UpdateUserRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateUserRoleRequest request, CancellationToken ct)
  {
    var command = new UpdateUserRoleCommand(request.UserRole);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateUserRoleResponse(result.Success), cancellation: ct);
  }
}