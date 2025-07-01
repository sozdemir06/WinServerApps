using FastEndpoints;
using Users.AppRoles.Dtos;
using Users.AppRoles.Features.CreateAppRole;

namespace Modules.Users.Users.AppRoles.Features.CreateAppRole;

public record CreateAppRoleRequest(AppRoleDto AppRole);
public record CreateAppRoleResponse(Guid Id);

/// <summary>
/// Endpoint for creating a new application role
/// </summary>
public class CreateAppRoleEndpoint(ISender sender) : Endpoint<CreateAppRoleRequest, CreateAppRoleResponse>
{
  public override void Configure()
  {
    Post("/admin/app-roles");
    Description(x => x
        .WithName("CreateAppRole")
        .WithTags("AppRoles")
        .Produces<CreateAppRoleResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAppRoleRequest request, CancellationToken ct)
  {
    var command = new CreateAppRoleCommand(request.AppRole);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateAppRoleResponse(result.Id), cancellation: ct);
  }
}