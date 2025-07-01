using FastEndpoints;
using WinApps.Modules.Users.Users.Branches.Dtos;

namespace WinApps.Modules.Users.Users.Branches.Features.CreateBranch;

public record CreateBranchRequest(BranchDto Branch);
public record CreateBranchResponse(Guid Id);

public class CreateBranchEndpoint : Endpoint<CreateBranchRequest, CreateBranchResponse>
{
  private readonly ISender _sender;

  public CreateBranchEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/branches");
    Description(x => x
        .WithName("CreateBranch")
        .WithTags("Branches")
        .Produces<CreateBranchResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateBranchRequest request, CancellationToken ct)
  {
    var command = new CreateBranchCommand(request.Branch);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateBranchResponse(result.Id), cancellation: ct);
  }
}