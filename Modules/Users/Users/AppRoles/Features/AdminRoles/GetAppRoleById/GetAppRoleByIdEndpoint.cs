using FastEndpoints;
using Users.AppRoles.Dtos;
using Users.AppRoles.Features.GetAppRoleById;

namespace Modules.Users.Users.AppRoles.Features.GetAppRoleById;

public record GetAppRoleByIdRequest(Guid Id);
public record GetAppRoleByIdResponse(AppRoleDto AppRole);

/// <summary>
/// Endpoint for retrieving a single application role by ID
/// </summary>
public class GetAppRoleByIdEndpoint(ISender sender) : Endpoint<GetAppRoleByIdRequest, GetAppRoleByIdResponse>
{
  public override void Configure()
  {
    Get("/admin/app-roles/{id}");
    Description(x => x
        .WithName("GetAppRoleById")
        .WithTags("AppRoles")
        .Produces<GetAppRoleByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAppRoleByIdRequest request, CancellationToken ct)
  {
    var query = new GetAppRoleByIdQuery(request.Id);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetAppRoleByIdResponse(result.AppRole), cancellation: ct);
  }
}