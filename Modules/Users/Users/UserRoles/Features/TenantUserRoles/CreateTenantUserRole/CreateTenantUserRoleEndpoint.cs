using FastEndpoints;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.TenantUserRoles.CreateTenantUserRole;

public record CreateTenantUserRoleRequest(UserRoleDto UserRole);
public record CreateTenantUserRoleResponse(Guid Id);

public class CreateTenantUserRoleEndpoint(ISender sender) : Endpoint<CreateTenantUserRoleRequest, CreateTenantUserRoleResponse>
{
  public override void Configure()
  {
    Post("/tenant/user-roles");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateTenantUserRole")
        .WithTags("TenantUserRoles")
        .Produces<CreateTenantUserRoleResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantUserRoleRequest request, CancellationToken ct)
  {
    var command = new CreateTenantUserRoleCommand(request.UserRole);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantUserRoleResponse(result.Id), StatusCodes.Status201Created, ct);
  }
}