using Accounting.ExpensePens.Dtos;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.AdminExpensePens.GetAdminExpensePenById;

public record GetAdminExpensePenByIdRequest(Guid Id);
public record GetAdminExpensePenByIdResponse(ExpensePenDto ExpensePen);

public class GetAdminExpensePenByIdEndpoint : Endpoint<GetAdminExpensePenByIdRequest, GetAdminExpensePenByIdResponse>
{
  private readonly ISender _sender;

  public GetAdminExpensePenByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/expense-pens/{Id}");
    Description(x => x
        .WithName("GetAdminExpensePenById")
        .WithTags("ExpensePens")
        .Produces<GetAdminExpensePenByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminExpensePenByIdRequest req, CancellationToken ct)
  {
    var query = new GetAdminExpensePenByIdQuery(req.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminExpensePenByIdResponse(result.ExpensePen), cancellation: ct);
  }
}