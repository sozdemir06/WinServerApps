using FastEndpoints;
using Users.UserRoles.Dtos;
using Users.UserRoles.Features.CreateUserRoles;

namespace Modules.Users.Users.UserRoles.Features.CreateUserRoles;

public record CreateUserRolesRequest(Guid ManagerId, List<Guid> RoleIds);
public record CreateUserRolesResponse(bool IsSuccess);

public class CreateUserRolesEndpoint(ISender sender) : Endpoint<CreateUserRolesRequest, CreateUserRolesResponse>
{
  public override void Configure()
  {
    Post("/create/user-roles/{managerId}");
    Description(x => x
        .WithName("CreateUserRoles")
        .WithTags("UserRoles")
        .Produces<CreateUserRolesResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateUserRolesRequest req, CancellationToken ct)
  {
    var command = new CreateUserRolesCommand(req.ManagerId, req.RoleIds);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateUserRolesResponse(result.IsSuccess),
        statusCode: StatusCodes.Status201Created, cancellation: ct);
  }
}