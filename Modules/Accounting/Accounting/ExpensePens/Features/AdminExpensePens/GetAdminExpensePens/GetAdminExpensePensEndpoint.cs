using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.QueryParams;
using FastEndpoints;

namespace Accounting.ExpensePens.Features.AdminExpensePens.GetAdminExpensePens;

public record GetAdminExpensePensRequest() : ExpensePenParams;
public record GetAdminExpensePensResponse(IEnumerable<ExpensePenDto> ExpensePens, PaginationMetaData MetaData);

public class GetAdminExpensePensEndpoint : Endpoint<GetAdminExpensePensRequest, GetAdminExpensePensResponse>
{
  private readonly ISender _sender;

  public GetAdminExpensePensEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/expense-pens");
    Description(x => x
        .WithName("GetAdminExpensePens")
        .WithTags("ExpensePens")
        .Produces<GetAdminExpensePensResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminExpensePensRequest req, CancellationToken ct)
  {
    var query = new GetAdminExpensePensQuery(req);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminExpensePensResponse(result.ExpensePens, result.MetaData), cancellation: ct);
  }
}