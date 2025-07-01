using FastEndpoints;

namespace Customers.Cities.Features.GetCityById;

public record GetCityByIdRequest(long Id);
public record GetCityByIdResponse(CityDto City);

public class GetCityByIdEndpoint : Endpoint<GetCityByIdRequest, GetCityByIdResponse>
{
  private readonly ISender _sender;

  public GetCityByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/cities/{id}");
    Description(x => x
        .WithName("GetCityById")
        .WithTags("Cities")
        .Produces<GetCityByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCityByIdRequest request, CancellationToken ct)
  {
    var query = new GetCityByIdQuery(request.Id);
    var result = await _sender.Send(query, ct);
    await SendAsync(new GetCityByIdResponse(result), cancellation: ct);
  }
}