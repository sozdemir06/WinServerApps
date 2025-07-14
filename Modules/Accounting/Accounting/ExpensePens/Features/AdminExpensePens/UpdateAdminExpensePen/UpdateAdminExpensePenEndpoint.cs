using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.AdminExpensePens.UpdateAdminExpensePen;

public record UpdateAdminExpensePenRequest(Guid Id, ExpensePenDto ExpensePen);
public record UpdateAdminExpensePenResponse(Guid Id);

public class UpdateAdminExpensePenEndpoint : Endpoint<UpdateAdminExpensePenRequest, UpdateAdminExpensePenResponse>
{
  private readonly ISender _sender;

  public UpdateAdminExpensePenEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/expense-pens/{Id}");
    Description(x => x
        .WithName("UpdateAdminExpensePen")
        .WithTags("ExpensePens")
        .Produces<UpdateAdminExpensePenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAdminExpensePenRequest req, CancellationToken ct)
  {
    var command = new UpdateAdminExpensePenCommand(req.Id, req.ExpensePen);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateAdminExpensePenResponse(result.Id), cancellation: ct);
  }
}