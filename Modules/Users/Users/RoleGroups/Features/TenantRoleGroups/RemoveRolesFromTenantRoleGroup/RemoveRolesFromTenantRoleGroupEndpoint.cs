using FastEndpoints;

namespace Users.RoleGroups.Features.TenantRoleGroups.RemoveRolesFromTenantRoleGroup;

public record RemoveRolesFromTenantRoleGroupRequest(Guid RoleGroupId, List<Guid> RoleIds);
public record RemoveRolesFromTenantRoleGroupResponse(bool Success);

public class RemoveRolesFromTenantRoleGroupEndpoint(ISender sender) : Endpoint<RemoveRolesFromTenantRoleGroupRequest, RemoveRolesFromTenantRoleGroupResponse>
{
  public override void Configure()
  {
    Delete("/tenant/role-groups/{RoleGroupId}/roles");
    AllowAnonymous();
    Description(x => x
        .WithName("RemoveRolesFromTenantRoleGroup")
        .WithTags("TenantRoleGroups")
        .Produces<RemoveRolesFromTenantRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveRolesFromTenantRoleGroupRequest request, CancellationToken ct)
  {
    var command = new RemoveRolesFromTenantRoleGroupCommand(request.RoleGroupId, request.RoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new RemoveRolesFromTenantRoleGroupResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}