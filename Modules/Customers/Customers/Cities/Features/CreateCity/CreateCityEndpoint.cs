using FastEndpoints;

namespace Customers.Cities.Features.CreateCity;

public record CreateCityRequest(CityDto City);
public record CreateCityResponse(long Id);

public class CreateCityEndpoint : Endpoint<CreateCityRequest, CreateCityResponse>
{
  private readonly ISender _sender;

  public CreateCityEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/cities");
    Description(x => x
        .WithName("CreateCity")
        .WithTags("Cities")
        .Produces<CreateCityResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateCityRequest request, CancellationToken ct)
  {
    var command = new CreateCityCommand(request.City);
    var result = await _sender.Send(command, ct);
    await SendAsync(new CreateCityResponse(result.Id), cancellation: ct);
  }
}