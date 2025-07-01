using FastEndpoints;
using Shared.Dtos;
using Users.UserRoles.Features.GetUserRolesForAuthorize;

namespace Modules.Users.Users.UserRoles.Features.GetUserRolesForAuthorize;

public record GetUserRolesForAuthorizeRequest(Guid ManagerId);
public record GetUserRolesForAuthorizeResponse(IEnumerable<ManagerRoleDto> UserRoles);

/// <summary>
/// Endpoint for retrieving user roles for authorization purposes
/// </summary>
public class GetUserRolesForAuthorizeEndpoint(ISender sender) : Endpoint<GetUserRolesForAuthorizeRequest, GetUserRolesForAuthorizeResponse>
{
  public override void Configure()
  {
    Get("/user-roles/authorize/{managerId}");
    Description(x => x
        .WithName("GetUserRolesForAuthorize")
        .WithTags("UserRoles")
        .Produces<GetUserRolesForAuthorizeResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetUserRolesForAuthorizeRequest request, CancellationToken ct)
  {
    var query = new GetUserRolesForAuthorizeQuery(request.ManagerId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetUserRolesForAuthorizeResponse(result.UserRoles), cancellation: ct);
  }
}