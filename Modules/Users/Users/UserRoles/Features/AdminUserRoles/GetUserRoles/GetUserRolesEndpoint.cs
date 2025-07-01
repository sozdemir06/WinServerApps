using FastEndpoints;
using Users.UserRoles.Dtos;
using Users.UserRoles.Features.GetUserRoles;
using Users.UserRoles.QueryParams;

namespace Modules.Users.Users.UserRoles.Features.GetUserRoles;

public record GetUserRolesRequest() : UserRoleQueryParams;
public record GetUserRolesResponse(IEnumerable<UserRoleDto> UserRoles, PaginationMetaData MetaData);

public class GetUserRolesEndpoint(ISender sender) : Endpoint<GetUserRolesRequest, GetUserRolesResponse>
{
  public override void Configure()
  {
    Get("/user-roles");
    Description(x => x
        .WithName("GetUserRoles")
        .WithTags("UserRoles")
        .Produces<GetUserRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetUserRolesRequest req, CancellationToken ct)
  {
    var query = new GetUserRolesQuery(req);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetUserRolesResponse(result.UserRoles, result.MetaData), cancellation: ct);
  }
}