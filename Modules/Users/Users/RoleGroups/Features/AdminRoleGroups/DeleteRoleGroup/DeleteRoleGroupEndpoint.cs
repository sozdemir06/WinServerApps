using FastEndpoints;

namespace Users.RoleGroups.Features.DeleteRoleGroup;

public record DeleteRoleGroupRequest(Guid Id);
public record DeleteRoleGroupResponse(Guid Id);

public class DeleteRoleGroupEndpoint : Endpoint<DeleteRoleGroupRequest, DeleteRoleGroupResponse>
{
  private readonly ISender _sender;

  public DeleteRoleGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("DeleteRoleGroup")
        .WithTags("RoleGroups")
        .Produces<DeleteRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteRoleGroupRequest request, CancellationToken ct)
  {
    var command = new DeleteRoleGroupCommand(request.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteRoleGroupResponse(result.Id), cancellation: ct);
  }
}