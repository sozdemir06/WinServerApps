using FastEndpoints;

namespace Customers.Countries.Features.GetCountryById;

public record GetCountryByIdRequest(long Id);

public record GetCountryByIdResponse(CountryDto Country);

public class GetCountryByIdEndpoint(ISender sender) : Endpoint<GetCountryByIdRequest, GetCountryByIdResponse>
{

  public override void Configure()
  {
    Get("/admin/countries/{id}");
    Description(x => x
        .WithName("GetCountryById")
        .WithTags("Countries")
        .Produces<GetCountryByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetCountryByIdRequest request, CancellationToken ct)
  {
    var query = new GetCountryByIdQuery(request.Id);
    var result = await sender.Send(query, ct);
    await SendAsync(new GetCountryByIdResponse(result.Country), cancellation: ct);
  }
}

