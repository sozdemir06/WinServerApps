using FastEndpoints;

namespace Users.RoleGroups.Features.TenantRoleGroups.DeleteTenantRoleGroup;

public record DeleteTenantRoleGroupRequest(Guid Id);
public record DeleteTenantRoleGroupResponse(bool Success);

public class DeleteTenantRoleGroupEndpoint(ISender sender) : Endpoint<DeleteTenantRoleGroupRequest, DeleteTenantRoleGroupResponse>
{
  public override void Configure()
  {
    Delete("/tenant/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteTenantRoleGroup")
        .WithTags("TenantRoleGroups")
        .Produces<DeleteTenantRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantRoleGroupRequest request, CancellationToken ct)
  {
    var command = new DeleteTenantRoleGroupCommand(request.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteTenantRoleGroupResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}