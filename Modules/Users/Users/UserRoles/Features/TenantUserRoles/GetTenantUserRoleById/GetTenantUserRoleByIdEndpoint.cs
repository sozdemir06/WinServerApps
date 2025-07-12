using FastEndpoints;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRoleById;

public record GetTenantUserRoleByIdRequest(Guid Id);
public record GetTenantUserRoleByIdResponse(UserRoleDto UserRole);

public class GetTenantUserRoleByIdEndpoint(ISender sender) : Endpoint<GetTenantUserRoleByIdRequest, GetTenantUserRoleByIdResponse>
{
  public override void Configure()
  {
    Get("/tenant/user-roles/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantUserRoleById")
        .WithTags("TenantUserRoles")
        .Produces<GetTenantUserRoleByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantUserRoleByIdRequest request, CancellationToken ct)
  {
    var query = new GetTenantUserRoleByIdQuery(request.Id);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantUserRoleByIdResponse(result.UserRole), StatusCodes.Status200OK, ct);
  }
}