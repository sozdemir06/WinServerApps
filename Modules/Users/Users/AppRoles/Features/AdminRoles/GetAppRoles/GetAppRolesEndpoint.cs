using FastEndpoints;
using Users.AppRoles.Dtos;
using Users.AppRoles.Features.GetAppRoles;
using Users.AppRoles.QueryParams;

namespace Modules.Users.Users.AppRoles.Features.GetAppRoles;

public record GetAppRolesRequest() : AppRoleQueryParams;
public record GetAppRolesResponse(IEnumerable<AppRoleDto> AppRoles, PaginationMetaData MetaData);

/// <summary>
/// Endpoint for retrieving a list of application roles
/// </summary>
public class GetAppRolesEndpoint(ISender sender) : Endpoint<GetAppRolesRequest, GetAppRolesResponse>
{
  public override void Configure()
  {
    Get("/admin/app-roles");
    Description(x => x
        .WithName("GetAppRoles")
        .WithTags("AppRoles")
        .Produces<GetAppRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAppRolesRequest request, CancellationToken ct)
  {
    var query = new GetAppRolesQuery(request);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetAppRolesResponse(result.AppRoles, result.MetaData), cancellation: ct);
  }
}