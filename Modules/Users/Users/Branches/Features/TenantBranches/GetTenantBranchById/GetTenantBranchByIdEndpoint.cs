using FastEndpoints;
using Shared.Services.Claims;
using WinApps.Modules.Users.Users.Branches.Dtos;

namespace WinApps.Modules.Users.Users.Branches.Features.GetTenantBranchById;

public record GetTenantBranchByIdRequest(Guid Id);
public record GetTenantBranchByIdResponse(BranchDto Branch);

public class GetTenantBranchByIdEndpoint : Endpoint<GetTenantBranchByIdRequest, GetTenantBranchByIdResponse>
{
  private readonly ISender _sender;
  private readonly IClaimsPrincipalService _claimsPrincipalService;

  public GetTenantBranchByIdEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService)
  {
    _sender = sender;
    _claimsPrincipalService = claimsPrincipalService;
  }

  public override void Configure()
  {
    Get("/tenant/branches/{id}");
    Description(x => x
        .WithName("GetTenantBranchById")
        .WithTags("TenantBranches")
        .Produces<GetTenantBranchByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantBranchByIdRequest request, CancellationToken ct)
  {
    var tenantId = _claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantBranchByIdQuery(request.Id, tenantId);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetTenantBranchByIdResponse(result.Branch), cancellation: ct);
  }
}