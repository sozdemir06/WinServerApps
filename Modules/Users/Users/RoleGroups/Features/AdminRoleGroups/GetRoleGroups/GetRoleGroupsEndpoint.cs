using FastEndpoints;
using Shared.Pagination;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.QueryParams;

namespace Users.RoleGroups.Features.GetRoleGroups;

public record GetRoleGroupsRequest() : RoleGroupParams
{

}

public record GetRoleGroupsResponse(IEnumerable<RoleGroupDto> RoleGroups, PaginationMetaData MetaData);

public class GetRoleGroupsEndpoint : Endpoint<GetRoleGroupsRequest, GetRoleGroupsResponse>
{
  private readonly ISender _sender;

  public GetRoleGroupsEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/role-groups");
    AllowAnonymous();
    Description(x => x
        .WithName("GetRoleGroups")
        .WithTags("RoleGroups")
        .Produces<GetRoleGroupsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetRoleGroupsRequest request, CancellationToken ct)
  {
    var query = new GetRoleGroupsQuery(request);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetRoleGroupsResponse(result.RoleGroups, result.MetaData), cancellation: ct);
  }
}