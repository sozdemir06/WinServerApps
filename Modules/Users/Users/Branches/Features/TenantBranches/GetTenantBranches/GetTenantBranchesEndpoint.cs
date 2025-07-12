using FastEndpoints;
using Shared.Services.Claims;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.QueryParams;

namespace WinApps.Modules.Users.Users.Branches.Features.GetTenantBranches;

public record GetTenantBranchesRequest() : BranchParams;
public record GetTenantBranchesResponse(IEnumerable<BranchDto> Branches, PaginationMetaData MetaData);

public class GetTenantBranchesEndpoint : Endpoint<GetTenantBranchesRequest, GetTenantBranchesResponse>
{
  private readonly ISender _sender;
  private readonly IClaimsPrincipalService _claimsPrincipalService;

  public GetTenantBranchesEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService)
  {
    _sender = sender;
    _claimsPrincipalService = claimsPrincipalService;
  }

  public override void Configure()
  {
    Get("/tenant/branches");
    Description(x => x
        .WithName("GetTenantBranches")
        .WithTags("TenantBranches")
        .Produces<GetTenantBranchesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantBranchesRequest req, CancellationToken ct)
  {
    var tenantId = _claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantBranchesQuery(req, tenantId);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetTenantBranchesResponse(result.Branches, result.MetaData), cancellation: ct);
  }
}