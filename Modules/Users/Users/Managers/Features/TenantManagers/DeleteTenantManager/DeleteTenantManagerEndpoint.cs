using FastEndpoints;

namespace Users.Managers.Features.TenantManagers.DeleteTenantManager;

public record DeleteTenantManagerRequest(Guid Id);
public record DeleteTenantManagerResponse(bool Success);

public class DeleteTenantManagerEndpoint(ISender sender) : Endpoint<DeleteTenantManagerRequest, DeleteTenantManagerResponse>
{
  public override void Configure()
  {
    Delete("/tenant/managers/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteTenantManager")
        .WithTags("TenantManagers")
        .Produces<DeleteTenantManagerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantManagerRequest request, CancellationToken ct)
  {
    var command = new DeleteTenantManagerCommand(request.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteTenantManagerResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}