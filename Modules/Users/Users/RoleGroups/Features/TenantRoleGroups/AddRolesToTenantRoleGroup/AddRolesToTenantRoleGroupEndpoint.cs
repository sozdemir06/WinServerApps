using FastEndpoints;

namespace Users.RoleGroups.Features.TenantRoleGroups.AddRolesToTenantRoleGroup;

public record AddRolesToTenantRoleGroupRequest(Guid RoleGroupId, List<Guid> RoleIds);
public record AddRolesToTenantRoleGroupResponse(bool Success);

public class AddRolesToTenantRoleGroupEndpoint(ISender sender) : Endpoint<AddRolesToTenantRoleGroupRequest, AddRolesToTenantRoleGroupResponse>
{
  public override void Configure()
  {
    Post("/tenant/role-groups/{RoleGroupId}/roles");
    AllowAnonymous();
    Description(x => x
        .WithName("AddRolesToTenantRoleGroup")
        .WithTags("TenantRoleGroups")
        .Produces<AddRolesToTenantRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(AddRolesToTenantRoleGroupRequest request, CancellationToken ct)
  {
    var command = new AddRolesToTenantRoleGroupCommand(request.RoleGroupId, request.RoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new AddRolesToTenantRoleGroupResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}