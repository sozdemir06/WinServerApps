using FastEndpoints;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.TenantRoleGroups.CreateTenantRoleGroup;

public record CreateTenantRoleGroupRequest(RoleGroupDto RoleGroup);
public record CreateTenantRoleGroupResponse(Guid Id);

public class CreateTenantRoleGroupEndpoint(ISender sender) : Endpoint<CreateTenantRoleGroupRequest, CreateTenantRoleGroupResponse>
{
  public override void Configure()
  {
    Post("/tenant/role-groups");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateTenantRoleGroup")
        .WithTags("TenantRoleGroups")
        .Produces<CreateTenantRoleGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantRoleGroupRequest request, CancellationToken ct)
  {
    var command = new CreateTenantRoleGroupCommand(request.RoleGroup);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantRoleGroupResponse(result.Id), StatusCodes.Status201Created, ct);
  }
}