using FastEndpoints;

namespace Customers.Districts.Features.GetDistrictById;

public record GetDistrictByIdRequest(long Id);
public record GetDistrictByIdResponse(DistrictDto District);

public class GetDistrictByIdEndpoint(ISender sender) : Endpoint<GetDistrictByIdRequest, GetDistrictByIdResponse>
{
  public override void Configure()
  {
    Get("/admin/districts/{id}");
    Description(x => x
        .WithName("GetDistrictById")
        .WithTags("Districts")
        .Produces<GetDistrictByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetDistrictByIdRequest request, CancellationToken ct)
  {
    var query = new GetDistrictByIdQuery(request.Id);
    var result = await sender.Send(query, ct);
    await SendAsync(new GetDistrictByIdResponse(result), cancellation: ct);
  }
}