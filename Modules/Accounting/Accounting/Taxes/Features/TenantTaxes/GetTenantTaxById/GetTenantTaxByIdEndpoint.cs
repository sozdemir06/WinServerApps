using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.GetTenantTaxById;

public record GetTenantTaxByIdRequest(Guid Id);
public record GetTenantTaxByIdResponse(TenantTaxDto TenantTax);

public class GetTenantTaxByIdEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantTaxByIdRequest, GetTenantTaxByIdResponse>
{

    public override void Configure()
  {
    Get("/tenant-taxes/{Id}");
    Description(x => x
        .WithName("GetTenantTaxById")
        .WithTags("Taxes")
        .Produces<GetTenantTaxByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantTaxByIdRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantTaxByIdQuery(req.Id,tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantTaxByIdResponse(result.TenantTax), cancellation: ct);
  }
}