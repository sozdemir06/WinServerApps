using FastEndpoints;

namespace Users.UserRoles.Features.TenantUserRoles.DeleteTenantUserRole;

public record DeleteTenantUserRoleRequest(Guid Id);
public record DeleteTenantUserRoleResponse(bool Success);

public class DeleteTenantUserRoleEndpoint(ISender sender) : Endpoint<DeleteTenantUserRoleRequest, DeleteTenantUserRoleResponse>
{
  public override void Configure()
  {
    Delete("/tenant/user-roles/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteTenantUserRole")
        .WithTags("TenantUserRoles")
        .Produces<DeleteTenantUserRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantUserRoleRequest request, CancellationToken ct)
  {
    var command = new DeleteTenantUserRoleCommand(request.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteTenantUserRoleResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}