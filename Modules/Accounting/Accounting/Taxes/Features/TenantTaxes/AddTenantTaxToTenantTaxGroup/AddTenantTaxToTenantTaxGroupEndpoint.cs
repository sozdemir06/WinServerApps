using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.AddTenantTaxToTenantTaxGroup;

public record AddTenantTaxToTenantTaxGroupRequest(Guid TenantTaxGroupId, TenantTaxDto TenantTax);
public record AddTenantTaxToTenantTaxGroupResponse(Guid TenantTaxId, Guid TenantTaxGroupId);

public class AddTenantTaxToTenantTaxGroupEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<AddTenantTaxToTenantTaxGroupRequest, AddTenantTaxToTenantTaxGroupResponse>
{


    public override void Configure()
  {
    Post("/tenant-tax-groups/{TenantTaxGroupId}/taxes");
    Description(x => x
        .WithName("AddTenantTaxToTenantTaxGroup")
        .WithTags("Taxes")
        .Produces<AddTenantTaxToTenantTaxGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(AddTenantTaxToTenantTaxGroupRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new AddTenantTaxToTenantTaxGroupCommand(req.TenantTaxGroupId, req.TenantTax,tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new AddTenantTaxToTenantTaxGroupResponse(result.TenantTaxId, result.TenantTaxGroupId), StatusCodes.Status201Created, cancellation: ct);
  }
}