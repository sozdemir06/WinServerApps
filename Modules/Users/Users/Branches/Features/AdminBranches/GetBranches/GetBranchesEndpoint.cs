using FastEndpoints;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Features.GetBranches;
using WinApps.Modules.Users.Users.Branches.QueryParams;

namespace WinApps.Modules.Users.Users.Branches.Features.GetBranches;

public record GetBranchesRequest() : BranchParams;
public record GetBranchesResponse(IEnumerable<BranchDto> Branches, PaginationMetaData MetaData);

public class GetBranchesEndpoint : Endpoint<GetBranchesRequest, GetBranchesResponse>
{
  private readonly ISender _sender;

  public GetBranchesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/branches");
    Description(x => x
        .WithName("GetBranches")
        .WithTags("Branches")
        .Produces<GetBranchesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetBranchesRequest req, CancellationToken ct)
  {
    var query = new GetBranchesQuery(req);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetBranchesResponse(result.Branches, result.MetaData), cancellation: ct);
  }
}