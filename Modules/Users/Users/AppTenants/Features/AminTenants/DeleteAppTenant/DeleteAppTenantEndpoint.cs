using FastEndpoints;

namespace Modules.Users.Users.AppTenants.Features.DeleteAppTenant;

public record DeleteAppTenantRequest(Guid Id);
public record DeleteAppTenantResponse(bool Success);

/// <summary>
/// Endpoint for deleting an application tenant
/// </summary>
public class DeleteAppTenantEndpoint(ISender sender) : Endpoint<DeleteAppTenantRequest, DeleteAppTenantResponse>
{
  public override void Configure()
  {
    Delete("/admin/app-tenants/{id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteAppTenant")
        .WithTags("AppTenants")
        .Produces<DeleteAppTenantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAppTenantRequest request, CancellationToken ct)
  {
    var command = new DeleteAppTenantCommand(request.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteAppTenantResponse(result.Success), cancellation: ct);
  }
}