using FastEndpoints;

namespace Users.UserRoles.Features.TenantUserRoles.RemoveTenantUserRoles;

public record RemoveTenantUserRolesRequest(List<Guid> UserRoleIds);
public record RemoveTenantUserRolesResponse(int RemovedCount, int NotFoundCount, List<Guid> RemovedIds);

public class RemoveTenantUserRolesEndpoint(ISender sender) : Endpoint<RemoveTenantUserRolesRequest, RemoveTenantUserRolesResponse>
{
  public override void Configure()
  {
    Delete("/tenant/user-roles/bulk");
    AllowAnonymous();
    Description(x => x
        .WithName("RemoveTenantUserRoles")
        .WithTags("TenantUserRoles")
        .Produces<RemoveTenantUserRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveTenantUserRolesRequest req, CancellationToken ct)
  {
    var command = new RemoveTenantUserRolesCommand(req.UserRoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new RemoveTenantUserRolesResponse(result.RemovedCount, result.NotFoundCount, result.RemovedIds), cancellation: ct);
  }
}