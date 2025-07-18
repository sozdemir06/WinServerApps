using FastEndpoints;

namespace Accounting.ExpensePens.Features.TenantExpensePens.DeleteTenantExpensePen;

public record DeleteTenantExpensePenRequest(Guid Id, Guid TenantId);
public record DeleteTenantExpensePenResponse(Guid Id);

public class DeleteTenantExpensePenEndpoint : Endpoint<DeleteTenantExpensePenRequest, DeleteTenantExpensePenResponse>
{
  private readonly ISender _sender;

  public DeleteTenantExpensePenEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/tenant/expense-pens/{Id}");
    Description(x => x
        .WithName("DeleteTenantExpensePen")
        .WithTags("TenantExpensePens")
        .Produces<DeleteTenantExpensePenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantExpensePenRequest req, CancellationToken ct)
  {
    var command = new DeleteTenantExpensePenCommand(req.Id, req.TenantId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteTenantExpensePenResponse(result.Id), cancellation: ct);
  }
}