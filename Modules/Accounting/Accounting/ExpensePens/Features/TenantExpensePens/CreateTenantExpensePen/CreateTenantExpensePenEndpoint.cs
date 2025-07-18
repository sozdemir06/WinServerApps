using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.TenantExpensePens.CreateTenantExpensePen;

public record CreateTenantExpensePenRequest(ExpensePenDto ExpensePen);
public record CreateTenantExpensePenResponse(Guid Id);

public class CreateTenantExpensePenEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<CreateTenantExpensePenRequest, CreateTenantExpensePenResponse>
{

    public override void Configure()
  {
    Post("/tenant/expense-pens");
    Description(x => x
        .WithName("CreateTenantExpensePen")
        .WithTags("TenantExpensePens")
        .Produces<CreateTenantExpensePenResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantExpensePenRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new CreateTenantExpensePenCommand(req.ExpensePen, tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantExpensePenResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}