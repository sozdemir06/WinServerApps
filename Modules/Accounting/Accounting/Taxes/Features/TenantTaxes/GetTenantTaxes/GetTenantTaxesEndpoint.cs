using Accounting.Taxes.Dtos;
using Accounting.Taxes.QueryParams;
using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.GetTenantTaxes;

public record GetTenantTaxesRequest() : TenantTaxParams;
public record GetTenantTaxesResponse(IEnumerable<TenantTaxDto> TenantTaxes, PaginationMetaData MetaData);

public class GetTenantTaxesEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantTaxesRequest, GetTenantTaxesResponse>
{

    public override void Configure()
  {
    Get("/tenant-taxes");
    Description(x => x
        .WithName("GetTenantTaxes")
        .WithTags("Taxes")
        .Produces<GetTenantTaxesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantTaxesRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantTaxesQuery(req,tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantTaxesResponse(result.TenantTaxes, result.MetaData), cancellation: ct);
  }
}