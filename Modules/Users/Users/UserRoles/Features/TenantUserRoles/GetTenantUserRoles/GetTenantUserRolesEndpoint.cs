using FastEndpoints;
using Users.UserRoles.Dtos;
using Users.UserRoles.QueryParams;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRoles;

public record GetTenantUserRolesRequest(UserRoleQueryParams? UserRoleParams = null);
public record GetTenantUserRolesResponse(IEnumerable<UserRoleDto> UserRoles, PaginationMetaData MetaData);

public class GetTenantUserRolesEndpoint(ISender sender) : Endpoint<GetTenantUserRolesRequest, GetTenantUserRolesResponse>
{
  public override void Configure()
  {
    Get("/tenant/user-roles");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantUserRoles")
        .WithTags("TenantUserRoles")
        .Produces<GetTenantUserRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantUserRolesRequest request, CancellationToken ct)
  {
    var query = new GetTenantUserRolesQuery(request.UserRoleParams);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantUserRolesResponse(result.UserRoles, result.MetaData), StatusCodes.Status200OK, ct);
  }
}