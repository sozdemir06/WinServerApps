using FastEndpoints;
using WinApps.Modules.Users.Users.Branches.Dtos;

namespace WinApps.Modules.Users.Users.Branches.Features.UpdateBranch;

public record UpdateBranchRequest(Guid Id, BranchDto Branch);
public record UpdateBranchResponse(bool Success);

public class UpdateBranchEndpoint(ISender sender) : Endpoint<UpdateBranchRequest, UpdateBranchResponse>
{

  public override void Configure()
  {
    Put("/admin/branches/{id}");
    Description(x => x
        .WithName("UpdateBranch")
        .WithTags("Branches")
        .Produces<UpdateBranchResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateBranchRequest request, CancellationToken ct)
  {
    var command = new UpdateBranchCommand(request.Id, request.Branch);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateBranchResponse(result.Success), cancellation: ct);
  }
}