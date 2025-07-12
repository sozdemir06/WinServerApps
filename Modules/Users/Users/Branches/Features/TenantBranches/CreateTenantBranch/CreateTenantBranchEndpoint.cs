using FastEndpoints;
using Shared.Services.Claims;
using WinApps.Modules.Users.Users.Branches.Dtos;

namespace WinApps.Modules.Users.Users.Branches.Features.CreateTenantBranch;

public record CreateTenantBranchRequest(BranchDto Branch);
public record CreateTenantBranchResponse(Guid Id);

public class CreateTenantBranchEndpoint : Endpoint<CreateTenantBranchRequest, CreateTenantBranchResponse>
{
  private readonly ISender _sender;
  private readonly IClaimsPrincipalService _claimsPrincipalService;

  public CreateTenantBranchEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService)
  {
    _sender = sender;
    _claimsPrincipalService = claimsPrincipalService;
  }

  public override void Configure()
  {
    Post("/tenant/branches");
    Description(x => x
        .WithName("CreateTenantBranch")
        .WithTags("TenantBranches")
        .Produces<CreateTenantBranchResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }
  public override async Task HandleAsync(CreateTenantBranchRequest request, CancellationToken ct)
  {
    var tenantId = _claimsPrincipalService.GetCurrentTenantId();
    var command = new CreateTenantBranchCommand(request.Branch, tenantId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateTenantBranchResponse(result.Id), cancellation: ct);
  }
}