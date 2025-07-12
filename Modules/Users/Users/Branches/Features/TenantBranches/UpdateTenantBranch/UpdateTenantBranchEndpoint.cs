using FastEndpoints;
using Shared.Services.Claims;
using WinApps.Modules.Users.Users.Branches.Dtos;

namespace WinApps.Modules.Users.Users.Branches.Features.UpdateTenantBranch;

public record UpdateTenantBranchRequest(Guid Id, BranchDto Branch);
public record UpdateTenantBranchResponse(bool Success);

public class UpdateTenantBranchEndpoint : Endpoint<UpdateTenantBranchRequest, UpdateTenantBranchResponse>
{
  private readonly ISender _sender;
  private readonly IClaimsPrincipalService _claimsPrincipalService;

  public UpdateTenantBranchEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService)
  {
    _sender = sender;
    _claimsPrincipalService = claimsPrincipalService;
  }

  public override void Configure()
  {
    Put("/tenant/branches/{id}");
    Description(x => x
        .WithName("UpdateTenantBranch")
        .WithTags("TenantBranches")
        .Produces<UpdateTenantBranchResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantBranchRequest request, CancellationToken ct)
  {
    var tenantId = _claimsPrincipalService.GetCurrentTenantId();
    var command = new UpdateTenantBranchCommand(request.Id, request.Branch, tenantId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateTenantBranchResponse(result.Success), cancellation: ct);
  }
}