using FastEndpoints;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.UpdateRoleGroup;

public record UpdateRoleGroupRequest(Guid Id, RoleGroupDto RoleGroup);
public record UpdateRoleGroupResponse(Guid Id);

public class UpdateRoleGroupEndpoint : Endpoint<UpdateRoleGroupRequest, UpdateRoleGroupResponse>
{
  private readonly ISender _sender;

  public UpdateRoleGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("UpdateRoleGroup")
        .WithTags("RoleGroups")
        .Produces<UpdateRoleGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateRoleGroupRequest request, CancellationToken ct)
  {
    var command = new UpdateRoleGroupCommand(request.Id, request.RoleGroup);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateRoleGroupResponse(result.Id), cancellation: ct);
  }
}