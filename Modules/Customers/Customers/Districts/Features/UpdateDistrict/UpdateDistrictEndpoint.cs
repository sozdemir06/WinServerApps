using FastEndpoints;

namespace Customers.Districts.Features.UpdateDistrict;

public record UpdateDistrictRequest(long Id, DistrictDto District);
public record UpdateDistrictResponse(long Id);

public class UpdateDistrictEndpoint : Endpoint<UpdateDistrictRequest, UpdateDistrictResponse>
{
  private readonly ISender _sender;

  public UpdateDistrictEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/districts/{id}");
    Description(x => x
        .WithName("UpdateDistrict")
        .WithTags("Districts")
        .Produces<UpdateDistrictResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateDistrictRequest request, CancellationToken ct)
  {
    var command = new UpdateDistrictCommand(request.Id, request.District);
    var result = await _sender.Send(command, ct);
    await SendAsync(new UpdateDistrictResponse(result.Id), cancellation: ct);
  }
}