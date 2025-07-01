using FastEndpoints;

namespace Customers.Currencies.Features.DeleteCurrency;

public record DeleteCurrencyRequest(long Id);
public record DeleteCurrencyResponse;

/// <summary>
/// Endpoint for deleting a currency
/// </summary>
public class DeleteCurrencyEndpoint : Endpoint<DeleteCurrencyRequest, DeleteCurrencyResponse>
{
  private readonly ISender _sender;

  public DeleteCurrencyEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/currencies/{Id}");
    Description(x => x
        .WithName("DeleteCurrency")
        .WithTags("Currencies")
        .Produces<DeleteCurrencyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteCurrencyRequest request, CancellationToken ct)
  {
    var command = new DeleteCurrencyCommand(request.Id);
    await _sender.Send(command, ct);

    await SendAsync(new DeleteCurrencyResponse(), cancellation: ct);
  }
}