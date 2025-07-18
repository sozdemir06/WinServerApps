using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.QueryParams;
using FastEndpoints;
using Shared.Services.Claims;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.GetTenantTaxGroups;

public record GetTenantTaxGroupsRequest() : TenantTaxGroupParams;
public record GetTenantTaxGroupsResponse(IEnumerable<TenantTaxGroupDto> TenantTaxGroups, PaginationMetaData MetaData);

public class GetTenantTaxGroupsEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantTaxGroupsRequest, GetTenantTaxGroupsResponse>
{


    public override void Configure()
  {
    Get("/tenant-tax-groups");
    Description(x => x
        .WithName("GetTenantTaxGroups")
        .WithTags("TaxGroups")
        .Produces<GetTenantTaxGroupsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantTaxGroupsRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantTaxGroupsQuery(req,tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantTaxGroupsResponse(result.TenantTaxGroups, result.MetaData), cancellation: ct);
  }
}