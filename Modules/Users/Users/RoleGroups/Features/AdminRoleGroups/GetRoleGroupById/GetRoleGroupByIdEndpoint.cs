using FastEndpoints;
using Users.RoleGroups.Dtos;

namespace Users.RoleGroups.Features.GetRoleGroupById;

public record GetRoleGroupByIdRequest(Guid Id);
public record GetRoleGroupByIdResponse(RoleGroupDto RoleGroup);

public class GetRoleGroupByIdEndpoint : Endpoint<GetRoleGroupByIdRequest, GetRoleGroupByIdResponse>
{
  private readonly ISender _sender;

  public GetRoleGroupByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/role-groups/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetRoleGroupById")
        .WithTags("RoleGroups")
        .Produces<GetRoleGroupByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetRoleGroupByIdRequest request, CancellationToken ct)
  {
    var query = new GetRoleGroupByIdQuery(request.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetRoleGroupByIdResponse(result.RoleGroup), cancellation: ct);
  }
}