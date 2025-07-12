using FastEndpoints;
using Shared.Services.Claims;

namespace WinApps.Modules.Users.Users.Branches.Features.DeleteTenantBranch;

public record DeleteTenantBranchRequest(Guid Id);
public record DeleteTenantBranchResponse(bool Success);

public class DeleteTenantBranchEndpoint : Endpoint<DeleteTenantBranchRequest, DeleteTenantBranchResponse>
{
  private readonly ISender _sender;
  private readonly IClaimsPrincipalService _claimsPrincipalService;

  public DeleteTenantBranchEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService)
  {
    _sender = sender;
    _claimsPrincipalService = claimsPrincipalService;
  }

  public override void Configure()
  {
    Delete("/tenant/branches/{id}");
    Description(x => x
        .WithName("DeleteTenantBranch")
        .WithTags("TenantBranches")
        .Produces<DeleteTenantBranchResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantBranchRequest request, CancellationToken ct)
  {
    var tenantId = _claimsPrincipalService.GetCurrentTenantId();
    var command = new DeleteTenantBranchCommand(request.Id, tenantId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteTenantBranchResponse(result.Success), cancellation: ct);
  }
}