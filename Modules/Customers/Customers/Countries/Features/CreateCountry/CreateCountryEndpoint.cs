using FastEndpoints;


namespace Customers.Countries.Features.CreateCountry;

public record CreateCountryRequest(CountryDto Country);
public record CreateCountryResponse(long Id);

/// <summary>
/// Endpoint for creating a new country
/// </summary>
public class CreateCountryEndpoint : Endpoint<CreateCountryRequest, CreateCountryResponse>
{
  private readonly ISender _sender;

  public CreateCountryEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/countries");
    Description(x => x
        .WithName("CreateCountry")
        .WithTags("Countries")
        .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateCountryRequest request, CancellationToken ct)
  {
    var command = new CreateCountryCommand(request.Country);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateCountryResponse(result.Id), cancellation: ct);
  }
}