using FastEndpoints;
using Shared.Services.Claims;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.TenantRoleGroups.GetTenantRoleGroupById;

public record GetTenantRoleGroupByIdRequest(Guid Id);
public record GetTenantRoleGroupByIdResponse(RoleGroupDto RoleGroup);

public class GetTenantRoleGroupByIdEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantRoleGroupByIdRequest, GetTenantRoleGroupByIdResponse>
{
  public override void Configure()
  {
    Get("/tenant/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantRoleGroupById")
        .WithTags("TenantRoleGroups")
        .Produces<GetTenantRoleGroupByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantRoleGroupByIdRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantRoleGroupByIdQuery(request.Id, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantRoleGroupByIdResponse(result.RoleGroup), StatusCodes.Status200OK, ct);
  }
}