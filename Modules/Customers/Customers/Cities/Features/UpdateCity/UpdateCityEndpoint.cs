using FastEndpoints;

namespace Customers.Cities.Features.UpdateCity;

public record UpdateCityRequest(long Id, CityDto City);
public record UpdateCityResponse(long Id);

public class UpdateCityEndpoint : Endpoint<UpdateCityRequest, UpdateCityResponse>
{
  private readonly ISender _sender;

  public UpdateCityEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/cities/{id}");
    Description(x => x
        .WithName("UpdateCity")
        .WithTags("Cities")
        .Produces<UpdateCityResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateCityRequest request, CancellationToken ct)
  {
    var command = new UpdateCityCommand(request.Id, request.City);
    var result = await _sender.Send(command, ct);
    await SendAsync(new UpdateCityResponse(result.Id), cancellation: ct);
  }
}