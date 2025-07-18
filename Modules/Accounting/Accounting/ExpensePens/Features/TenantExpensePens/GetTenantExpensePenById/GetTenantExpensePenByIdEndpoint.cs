using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.TenantExpensePens.GetTenantExpensePenById;

public record GetTenantExpensePenByIdRequest(Guid Id, Guid TenantId);
public record GetTenantExpensePenByIdResponse(TenantExpensePenDto TenantExpensePen);

public class GetTenantExpensePenByIdEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantExpensePenByIdRequest, GetTenantExpensePenByIdResponse>
{

    public override void Configure()
  {
    Get("/tenant/expense-pens/{Id}");
    Description(x => x
        .WithName("GetTenantExpensePenById")
        .WithTags("TenantExpensePens")
        .Produces<GetTenantExpensePenByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantExpensePenByIdRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantExpensePenByIdQuery(req.Id, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantExpensePenByIdResponse(result.TenantExpensePen), cancellation: ct);
  }
}