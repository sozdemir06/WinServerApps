using FastEndpoints;
using Users.Managers.Dtos;
using Users.Managers.QueryParams;

namespace Users.Managers.Features.AdminManagers.GetManager;

public record GetManagersRequest(ManagerParams? Params = null);
public record GetManagersResponse(IEnumerable<ManagerDto> Managers, PaginationMetaData MetaData);

public class GetManagersEndpoint(ISender sender) : Endpoint<GetManagersRequest, GetManagersResponse>
{
  public override void Configure()
  {
    Get("/admin/managers");
    AllowAnonymous();
    Description(x => x
        .WithName("GetManagers")
        .WithTags("AdminManagers")
        .Produces<GetManagersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetManagersRequest request, CancellationToken ct)
  {
    var query = new GetManagersQuery(request.Params);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetManagersResponse(result.Managers, result.MetaData), StatusCodes.Status200OK, ct);
  }
}