using FastEndpoints;
using Shared.Services.Claims;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.QueryParams;

namespace Users.RoleGroups.Features.TenantRoleGroups.GetTenantRoleGroups;

public record GetTenantRoleGroupsRequest(RoleGroupParams? Parameters = null);
public record GetTenantRoleGroupsResponse(IEnumerable<RoleGroupDto> RoleGroups, PaginationMetaData MetaData);

public class GetTenantRoleGroupsEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantRoleGroupsRequest, GetTenantRoleGroupsResponse>
{
  public override void Configure()
  {
    Get("/tenant/role-groups");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantRoleGroups")
        .WithTags("TenantRoleGroups")
        .Produces<GetTenantRoleGroupsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantRoleGroupsRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantRoleGroupsQuery(request.Parameters, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantRoleGroupsResponse(result.RoleGroups, result.MetaData), StatusCodes.Status200OK, ct);
  }
}