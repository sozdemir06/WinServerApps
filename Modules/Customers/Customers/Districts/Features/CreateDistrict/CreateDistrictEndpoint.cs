using FastEndpoints;

namespace Customers.Districts.Features.CreateDistrict;

public record CreateDistrictRequest(DistrictDto District);
public record CreateDistrictResponse(long Id);

public class CreateDistrictEndpoint(ISender sender) : Endpoint<CreateDistrictRequest, CreateDistrictResponse>
{
  public override void Configure()
  {
    Post("/admin/districts");
    Description(x => x
        .WithName("CreateDistrict")
        .WithTags("Districts")
        .Produces<CreateDistrictResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateDistrictRequest request, CancellationToken ct)
  {
    var command = new CreateDistrictCommand(request.District);
    var result = await sender.Send(command, ct);
    await SendAsync(new CreateDistrictResponse(result.Id), statusCode: StatusCodes.Status201Created, cancellation: ct);
  }
}