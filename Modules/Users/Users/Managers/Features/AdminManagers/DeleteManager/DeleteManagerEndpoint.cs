using FastEndpoints;

namespace Users.Managers.Features.AdminManagers.DeleteManager;

public record DeleteManagerRequest(Guid Id);
public record DeleteManagerResponse(bool Success);

public class DeleteManagerEndpoint(ISender sender) : Endpoint<DeleteManagerRequest, DeleteManagerResponse>
{
  public override void Configure()
  {
    Delete("/admin/managers/{id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteManager")
        .WithTags("AdminManagers")
        .Produces<DeleteManagerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteManagerRequest request, CancellationToken ct)
  {
    var command = new DeleteManagerCommand(request.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteManagerResponse(result.Success), cancellation: ct);
  }
}