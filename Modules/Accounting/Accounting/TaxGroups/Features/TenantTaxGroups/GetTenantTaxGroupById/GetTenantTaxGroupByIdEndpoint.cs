using Accounting.TaxGroups.Dtos;
using FastEndpoints;
using Shared.Services.Claims;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.GetTenantTaxGroupById;

public record GetTenantTaxGroupByIdRequest(Guid Id);
public record GetTenantTaxGroupByIdResponse(TenantTaxGroupDto TenantTaxGroup);

public class GetTenantTaxGroupByIdEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantTaxGroupByIdRequest, GetTenantTaxGroupByIdResponse>
{


    public override void Configure()
  {
    Get("/tenant-tax-groups/{Id}");
    Description(x => x
        .WithName("GetTenantTaxGroupById")
        .WithTags("TaxGroups")
        .Produces<GetTenantTaxGroupByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantTaxGroupByIdRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantTaxGroupByIdQuery(req.Id,tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantTaxGroupByIdResponse(result.TenantTaxGroup), cancellation: ct);
  }
}