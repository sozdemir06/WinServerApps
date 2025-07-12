using FastEndpoints;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.TenantRoleGroups.UpdateTenantRoleGroup;

public record UpdateTenantRoleGroupRequest(Guid Id, RoleGroupDto RoleGroup);
public record UpdateTenantRoleGroupResponse(bool Success);

public class UpdateTenantRoleGroupEndpoint(ISender sender) : Endpoint<UpdateTenantRoleGroupRequest, UpdateTenantRoleGroupResponse>
{
  public override void Configure()
  {
    Put("/tenant/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("UpdateTenantRoleGroup")
        .WithTags("TenantRoleGroups")
        .Produces<UpdateTenantRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantRoleGroupRequest request, CancellationToken ct)
  {
    var command = new UpdateTenantRoleGroupCommand(request.Id, request.RoleGroup);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateTenantRoleGroupResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}