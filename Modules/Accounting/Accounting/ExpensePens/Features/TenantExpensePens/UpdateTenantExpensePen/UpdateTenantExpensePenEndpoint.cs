using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.TenantExpensePens.UpdateTenantExpensePen;

public record UpdateTenantExpensePenRequest(Guid Id, Guid TenantId, ExpensePenDto ExpensePen);
public record UpdateTenantExpensePenResponse(Guid Id);

public class UpdateTenantExpensePenEndpoint : Endpoint<UpdateTenantExpensePenRequest, UpdateTenantExpensePenResponse>
{
  private readonly ISender _sender;

  public UpdateTenantExpensePenEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/tenant/expense-pens/{Id}");
    Description(x => x
        .WithName("UpdateTenantExpensePen")
        .WithTags("TenantExpensePens")
        .Produces<UpdateTenantExpensePenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantExpensePenRequest req, CancellationToken ct)
  {
    var command = new UpdateTenantExpensePenCommand(req.Id, req.TenantId, req.ExpensePen);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateTenantExpensePenResponse(result.Id), cancellation: ct);
  }
}