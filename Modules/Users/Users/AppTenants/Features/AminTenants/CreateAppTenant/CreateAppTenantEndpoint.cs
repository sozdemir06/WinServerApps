using FastEndpoints;
using Users.AppTenants.Features.CreateAppTenant;

namespace Modules.Users.Users.AppTenants.Features.CreateAppTenant;

public record CreateAppTenantRequest(AppTenantDto AppTenant);
public record CreateAppTenantResponse(Guid Id);

/// <summary>
/// Endpoint for creating a new application tenant
/// </summary>
public class CreateAppTenantEndpoint(ISender sender) : Endpoint<CreateAppTenantRequest, CreateAppTenantResponse>
{

  public override void Configure()
  {
    Post("/app-tenants");
    Description(x => x
        .WithName("CreateAppTenant")
        .WithTags("AppTenants")
        .Produces<CreateAppTenantResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAppTenantRequest request, CancellationToken ct)
  {

    var command = new CreateAppTennatCommand(request.AppTenant);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateAppTenantResponse(result.Id), cancellation: ct);
  }
}



