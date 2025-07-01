using FastEndpoints;
using Users.UserRoles.Features.RemoveUserRoles;

namespace Modules.Users.Users.UserRoles.Features.RemoveUserRoles;

public record RemoveUserRolesRequest(List<Guid> UserRoleIds);
public record RemoveUserRolesResponse(int RemovedCount, int NotFoundCount, List<Guid> RemovedIds);

public class RemoveUserRolesEndpoint(ISender sender) : Endpoint<RemoveUserRolesRequest, RemoveUserRolesResponse>
{
  public override void Configure()
  {
    Delete("/user-roles/bulk");
    Description(x => x
        .WithName("RemoveUserRoles")
        .WithTags("UserRoles")
        .Produces<RemoveUserRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveUserRolesRequest req, CancellationToken ct)
  {
    var command = new RemoveUserRolesCommand(req.UserRoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new RemoveUserRolesResponse(result.RemovedCount, result.NotFoundCount, result.RemovedIds), cancellation: ct);
  }
}