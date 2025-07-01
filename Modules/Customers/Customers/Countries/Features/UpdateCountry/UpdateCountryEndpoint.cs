using FastEndpoints;


namespace Customers.Countries.Features.UpdateCountry;

public record UpdateCountryRequest(long Id, CountryDto Country);

public record UpdateCountryResponse(long Id);

public class UpdateCountryEndpoint(ISender sender) : Endpoint<UpdateCountryRequest, UpdateCountryResponse>
{


  public override void Configure()
  {
    Put("/admin/countries/{id}");
    Description(x => x
        .WithName("UpdateCountry")
        .WithTags("Countries")
        .Produces<UpdateCountryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateCountryRequest request, CancellationToken ct)
  {
    var command = new UpdateCountryCommand(request.Id, request.Country);
    var result = await sender.Send(command, ct);
    await SendAsync(new UpdateCountryResponse(result.Id), cancellation: ct);
  }
}

