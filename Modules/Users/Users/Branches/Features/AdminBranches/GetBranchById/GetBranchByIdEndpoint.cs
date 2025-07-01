using FastEndpoints;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Features.GetBranchById;

namespace WinApps.Modules.Users.Users.Branches.Features.GetBranchById;

public record GetBranchByIdRequest(Guid Id);
public record GetBranchByIdResponse(BranchDto Branch);

/// <summary>
/// Endpoint for retrieving a specific branch by ID
/// </summary>
public class GetBranchByIdEndpoint : Endpoint<GetBranchByIdRequest, GetBranchByIdResponse>
{
  private readonly ISender _sender;

  public GetBranchByIdEndpoint(ISender sender)
  {
    _sender = sender ?? throw new ArgumentNullException(nameof(sender));
  }

  public override void Configure()
  {
    Get("/admin/branches/{id}");
    Description(x => x
        .WithName("GetBranchById")
        .WithTags("Branches")
        .Produces<GetBranchByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetBranchByIdRequest request, CancellationToken ct)
  {
    var query = new GetBranchByIdQuery(request.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetBranchByIdResponse(result.Branch), cancellation: ct);
  }
}