using FastEndpoints;
using Shared.Services.Claims;
using Users.Managers.Dtos;
using Users.Managers.QueryParams;

namespace Users.Managers.Features.TenantManagers.GetTenantManager;

public record GetTenantManagersRequest():ManagerParams;
public record GetTenantManagersResponse(IEnumerable<ManagerDto> Managers, PaginationMetaData MetaData);

public class GetTenantManagersEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantManagersRequest, GetTenantManagersResponse>
{
  public override void Configure()
  {
    Get("/tenant/managers");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantManagers")
        .WithTags("TenantManagers")
        .Produces<GetTenantManagersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantManagersRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantManagersQuery(request, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantManagersResponse(result.Managers, result.MetaData), StatusCodes.Status200OK, ct);
  }
}