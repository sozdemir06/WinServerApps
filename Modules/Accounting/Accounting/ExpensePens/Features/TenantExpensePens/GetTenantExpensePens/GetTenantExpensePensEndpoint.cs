using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.QueryParams;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.TenantExpensePens.GetTenantExpensePens;

public record GetTenantExpensePensRequest() : TenantExpensePenParams;
public record GetTenantExpensePensResponse(IEnumerable<TenantExpensePenDto> TenantExpensePens, PaginationMetaData MetaData);

public class GetTenantExpensePensEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantExpensePensRequest, GetTenantExpensePensResponse>
{

    public override void Configure()
  {
    Get("/tenant/expense-pens");
    Description(x => x
        .WithName("GetTenantExpensePens")
        .WithTags("TenantExpensePens")
        .Produces<GetTenantExpensePensResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantExpensePensRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantExpensePensQuery(req, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantExpensePensResponse(result.TenantExpensePens, result.MetaData), cancellation: ct);
  }
}