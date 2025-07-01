using FastEndpoints;

namespace Modules.Users.Users.AppTenants.Features.UpdateAppTenant;

public record UpdateAppTenantRequest(Guid Id, AppTenantDto AppTenant);
public record UpdateAppTenantResponse(bool Success);

/// <summary>
/// Endpoint for updating an existing application tenant
/// </summary>
public class UpdateAppTenantEndpoint(ISender sender) : Endpoint<UpdateAppTenantRequest, UpdateAppTenantResponse>
{
  public override void Configure()
  {
    Put("/admin/app-tenants/{id}");
    Description(x => x
        .WithName("UpdateAppTenant")
        .WithTags("AppTenants")
        .Produces<UpdateAppTenantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAppTenantRequest request, CancellationToken ct)
  {
    var command = new UpdateAppTenantCommand(request.Id, request.AppTenant);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateAppTenantResponse(result.Success), cancellation: ct);
  }
}