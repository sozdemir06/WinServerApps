using FastEndpoints;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.CreateRoleGroup;

public record CreateRoleGroupRequest(RoleGroupDto RoleGroup);
public record CreateRoleGroupResponse(Guid Id);

public class CreateRoleGroupEndpoint : Endpoint<CreateRoleGroupRequest, CreateRoleGroupResponse>
{
  private readonly ISender _sender;

  public CreateRoleGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/role-groups");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateRoleGroup")
        .WithTags("RoleGroups")
        .Produces<CreateRoleGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateRoleGroupRequest request, CancellationToken ct)
  {
    var command = new CreateRoleGroupCommand(request.RoleGroup);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateRoleGroupResponse(result.Id), cancellation: ct);
  }
}