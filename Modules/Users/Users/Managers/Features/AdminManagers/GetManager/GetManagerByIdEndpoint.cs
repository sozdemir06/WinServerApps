using FastEndpoints;
using Users.Managers.Dtos;

namespace Users.Managers.Features.AdminManagers.GetManager;

public record GetManagerByIdRequest(Guid Id);
public record GetManagerByIdResponse(ManagerDetailDto Manager);

public class GetManagerByIdEndpoint(ISender sender) : Endpoint<GetManagerByIdRequest, GetManagerByIdResponse>
{
  public override void Configure()
  {
    Get("/admin/managers/{id}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetManagerById")
        .WithTags("AdminManagers")
        .Produces<GetManagerByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetManagerByIdRequest request, CancellationToken ct)
  {
    var query = new GetManagerByIdQuery(request.Id);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetManagerByIdResponse(result.Manager), cancellation: ct);
  }
}