using FastEndpoints;

namespace Customers.Cities.Features.DeleteCity;

public record DeleteCityRequest(long Id);
public record DeleteCityResponse(bool IsSuccess);

public class DeleteCityEndpoint : Endpoint<DeleteCityRequest, DeleteCityResponse>
{
  private readonly ISender _sender;

  public DeleteCityEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/cities/{id}");
    Description(x => x
        .WithName("DeleteCity")
        .WithTags("Cities")
        .Produces<DeleteCityResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteCityRequest request, CancellationToken ct)
  {
    var command = new DeleteCityCommand(request.Id);
    var result = await _sender.Send(command, ct);
    await SendAsync(new DeleteCityResponse(result.IsSuccess), cancellation: ct);
  }
}