using FastEndpoints;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRolesByUserId;

public record GetTenantUserRolesByUserIdRequest(Guid UserId);
public record GetTenantUserRolesByUserIdResponse(IEnumerable<UserRoleWithRoleNameDto> UserRoles);

public class GetTenantUserRolesByUserIdEndpoint(ISender sender) : Endpoint<GetTenantUserRolesByUserIdRequest, GetTenantUserRolesByUserIdResponse>
{
  public override void Configure()
  {
    Get("/tenant/user-roles/user/{UserId}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantUserRolesByUserId")
        .WithTags("TenantUserRoles")
        .Produces<GetTenantUserRolesByUserIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantUserRolesByUserIdRequest request, CancellationToken ct)
  {
    var query = new GetTenantUserRolesByUserIdQuery(request.UserId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantUserRolesByUserIdResponse(result.UserRoles), StatusCodes.Status200OK, ct);
  }
}