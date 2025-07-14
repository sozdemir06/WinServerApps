using FastEndpoints;

namespace Accounting.ExpensePens.Features.AdminExpensePens.DeleteAdminExpensePen;

public record DeleteAdminExpensePenRequest(Guid Id);
public record DeleteAdminExpensePenResponse(Guid Id);

public class DeleteAdminExpensePenEndpoint : Endpoint<DeleteAdminExpensePenRequest, DeleteAdminExpensePenResponse>
{
  private readonly ISender _sender;

  public DeleteAdminExpensePenEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/expense-pens/{Id}");
    Description(x => x
        .WithName("DeleteAdminExpensePen")
        .WithTags("ExpensePens")
        .Produces<DeleteAdminExpensePenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAdminExpensePenRequest req, CancellationToken ct)
  {
    var command = new DeleteAdminExpensePenCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteAdminExpensePenResponse(result.Id), cancellation: ct);
  }
}