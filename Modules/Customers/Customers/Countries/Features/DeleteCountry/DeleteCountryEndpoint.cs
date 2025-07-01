using FastEndpoints;


namespace Customers.Countries.Features.DeleteCountry;

public class DeleteCountryEndpoint : Endpoint<DeleteCountryRequest, DeleteCountryResponse>
{
  private readonly ISender _sender;

  public DeleteCountryEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/countries/{id}");
    Description(x => x
        .WithName("DeleteCountry")
        .WithTags("Countries")
        .Produces<DeleteCountryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteCountryRequest request, CancellationToken ct)
  {
    var command = new DeleteCountryCommand(request.Id);
    await _sender.Send(command, ct);
    await SendAsync(new DeleteCountryResponse(), cancellation: ct);
  }
}

public record DeleteCountryRequest
{
  public long Id { get; init; }
}

public record DeleteCountryResponse;