using FastEndpoints;

namespace Users.RoleGroups.Features.RemoveRolesFromRoleGroup;

public record RemoveRolesFromRoleGroupRequest(Guid RoleGroupId, List<Guid> AppRoleIds);
public record RemoveRolesFromRoleGroupResponse(Guid RoleGroupId, int RemovedRolesCount);

public class RemoveRolesFromRoleGroupEndpoint : Endpoint<RemoveRolesFromRoleGroupRequest, RemoveRolesFromRoleGroupResponse>
{
  private readonly ISender _sender;

  public RemoveRolesFromRoleGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/role-groups/{RoleGroupId}/roles");
    AllowAnonymous();
    Description(x => x
        .WithName("RemoveRolesFromRoleGroup")
        .WithTags("RoleGroups")
        .Produces<RemoveRolesFromRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveRolesFromRoleGroupRequest request, CancellationToken ct)
  {
    var command = new RemoveRolesFromRoleGroupCommand(request.RoleGroupId, request.AppRoleIds);
    var result = await _sender.Send(command, ct);

    await SendAsync(new RemoveRolesFromRoleGroupResponse(result.RoleGroupId, result.RemovedRolesCount), cancellation: ct);
  }
}