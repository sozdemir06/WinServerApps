using Accounting.TaxGroups.Dtos;
using FastEndpoints;
using Shared.Services.Claims;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.CreateTenantTaxGroup;

public record CreateTenantTaxGroupRequest(TenantTaxGroupDto TenantTaxGroup);
public record CreateTenantTaxGroupResponse(Guid Id);

public class CreateTenantTaxGroupEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<CreateTenantTaxGroupRequest, CreateTenantTaxGroupResponse>
{

    public override void Configure()
  {
    Post("/tenant-tax-groups");
    Description(x => x
        .WithName("CreateTenantTaxGroup")
        .WithTags("TaxGroups")
        .Produces<CreateTenantTaxGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantTaxGroupRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    

    var command = new CreateTenantTaxGroupCommand(req.TenantTaxGroup,tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantTaxGroupResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}