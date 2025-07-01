using FastEndpoints;
using Shared.Dtos;
using Users.UserRoles.Features.GetUserRoleById;

namespace Modules.Users.Users.UserRoles.Features.GetUserRoleById;

public record GetUserRoleByIdRequest(Guid ManagerId);
public record GetUserRoleByIdResponse(ManagerRoleDto ManagerRole);

public class GetUserRoleByIdEndpoint : Endpoint<GetUserRoleByIdRequest, GetUserRoleByIdResponse>
{
  private readonly ISender _sender;

  public GetUserRoleByIdEndpoint(ISender sender)
  {
    _sender = sender ?? throw new ArgumentNullException(nameof(sender));
  }

  public override void Configure()
  {
    Get("/manager-roles/{managerId}");
    Description(x => x
        .WithName("GetManagerRoleById")
        .WithTags("ManagerRoles")
        .Produces<GetUserRoleByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetUserRoleByIdRequest request, CancellationToken ct)
  {
    var query = new GetUserRoleByIdQuery(request.ManagerId);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetUserRoleByIdResponse(result.ManagerRole), cancellation: ct);
  }
}