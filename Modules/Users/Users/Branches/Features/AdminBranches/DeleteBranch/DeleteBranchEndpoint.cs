using FastEndpoints;

namespace WinApps.Modules.Users.Users.Branches.Features.DeleteBranch;

public record DeleteBranchRequest(Guid Id);
public record DeleteBranchResponse(bool Success);

public class DeleteBranchEndpoint : Endpoint<DeleteBranchRequest, DeleteBranchResponse>
{
  private readonly ISender _sender;

  public DeleteBranchEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/branches/{id}");
    Description(x => x
        .WithName("DeleteBranch")
        .WithTags("Branches")
        .Produces<DeleteBranchResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteBranchRequest request, CancellationToken ct)
  {
    var command = new DeleteBranchCommand(request.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteBranchResponse(result.Success), cancellation: ct);
  }
}