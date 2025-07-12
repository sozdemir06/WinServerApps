using FastEndpoints;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.TenantUserRoles.UpdateTenantUserRole;

public record UpdateTenantUserRoleRequest(Guid Id, UserRoleDto UserRole);
public record UpdateTenantUserRoleResponse(bool Success);

public class UpdateTenantUserRoleEndpoint(ISender sender) : Endpoint<UpdateTenantUserRoleRequest, UpdateTenantUserRoleResponse>
{
  public override void Configure()
  {
    Put("/tenant/user-roles/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("UpdateTenantUserRole")
        .WithTags("TenantUserRoles")
        .Produces<UpdateTenantUserRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantUserRoleRequest request, CancellationToken ct)
  {
    var command = new UpdateTenantUserRoleCommand(request.Id, request.UserRole);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateTenantUserRoleResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}