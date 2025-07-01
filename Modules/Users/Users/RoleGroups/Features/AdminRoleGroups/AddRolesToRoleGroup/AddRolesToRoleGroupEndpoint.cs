using FastEndpoints;

namespace Users.RoleGroups.Features.AddRolesToRoleGroup;

public record AddRolesToRoleGroupRequest(Guid RoleGroupId, List<Guid> AppRoleIds);
public record AddRolesToRoleGroupResponse(Guid RoleGroupId, int AddedRolesCount);

public class AddRolesToRoleGroupEndpoint : Endpoint<AddRolesToRoleGroupRequest, AddRolesToRoleGroupResponse>
{
  private readonly ISender _sender;

  public AddRolesToRoleGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/role-groups/{RoleGroupId}");
    AllowAnonymous();
    Description(x => x
        .WithName("AddRolesToRoleGroup")
        .WithTags("RoleGroups")
        .Produces<AddRolesToRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(AddRolesToRoleGroupRequest request, CancellationToken ct)
  {
    var command = new AddRolesToRoleGroupCommand(request.RoleGroupId, request.AppRoleIds);
    var result = await _sender.Send(command, ct);

    await SendAsync(new AddRolesToRoleGroupResponse(result.RoleGroupId, result.AddedRolesCount), cancellation: ct);
  }
}