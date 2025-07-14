using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.AdminExpensePens.CreateAdminExpensePen;

public record CreateAdminExpensePenRequest(ExpensePenDto ExpensePen);
public record CreateAdminExpensePenResponse(Guid Id);

public class CreateAdminExpensePenEndpoint : Endpoint<CreateAdminExpensePenRequest, CreateAdminExpensePenResponse>
{
  private readonly ISender _sender;

  public CreateAdminExpensePenEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/expense-pens");
    Description(x => x
        .WithName("CreateAdminExpensePen")
        .WithTags("ExpensePens")
        .Produces<CreateAdminExpensePenResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAdminExpensePenRequest req, CancellationToken ct)
  {
    var command = new CreateAdminExpensePenCommand(req.ExpensePen);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateAdminExpensePenResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}