using FastEndpoints;
using Users.UserRoles.Dtos;
using Users.UserRoles.Features.GetUserRolesByUserId;

namespace Modules.Users.Users.UserRoles.Features.GetUserRolesByUserId;

public record GetUserRolesByUserIdRequest(Guid ManagerId);
public record GetUserRolesByUserIdResponse(IEnumerable<UserRoleWithRoleNameDto> UserRoles);

public class GetUserRolesByUserIdEndpoint(ISender sender) : Endpoint<GetUserRolesByUserIdRequest, GetUserRolesByUserIdResponse>
{
  public override void Configure()
  {
    Get("/user-roles/manager/{managerId}");
    Description(x => x
        .WithName("GetUserRolesByUserId")
        .WithTags("UserRoles")
        .Produces<GetUserRolesByUserIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetUserRolesByUserIdRequest request, CancellationToken ct)
  {
    var query = new GetUserRolesByUserIdQuery(request.ManagerId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetUserRolesByUserIdResponse(result.UserRoles), cancellation: ct);
  }
}