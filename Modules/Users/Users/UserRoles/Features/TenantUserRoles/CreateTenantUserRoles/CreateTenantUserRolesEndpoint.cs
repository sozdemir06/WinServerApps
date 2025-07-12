using FastEndpoints;

namespace Users.UserRoles.Features.TenantUserRoles.CreateTenantUserRoles;

public record CreateTenantUserRolesRequest(Guid ManagerId, List<Guid> RoleIds);
public record CreateTenantUserRolesResponse(bool IsSuccess);

public class CreateTenantUserRolesEndpoint(ISender sender) : Endpoint<CreateTenantUserRolesRequest, CreateTenantUserRolesResponse>
{
  public override void Configure()
  {
    Post("/tenant/user-roles/{ManagerId}");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateTenantUserRoles")
        .WithTags("TenantUserRoles")
        .Produces<CreateTenantUserRolesResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantUserRolesRequest req, CancellationToken ct)
  {
    var command = new CreateTenantUserRolesCommand(req.ManagerId, req.RoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantUserRolesResponse(result.IsSuccess),
        statusCode: StatusCodes.Status201Created, cancellation: ct);
  }
}