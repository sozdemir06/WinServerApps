using Users.RoleGroups.DomainExtensions;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.QueryParams;

namespace Users.RoleGroups.Features.GetRoleGroups;

public record GetRoleGroupsQuery(RoleGroupParams Parameters) : IQuery<GetRoleGroupsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.RoleGroups, Parameters);
  public string CacheGroupKey => CacheKeys.RoleGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetRoleGroupsResult(IEnumerable<RoleGroupDto> RoleGroups, PaginationMetaData MetaData);

public class GetRoleGroupsHandler(UserDbContext context) : IQueryHandler<GetRoleGroupsQuery, GetRoleGroupsResult>
{
  public async Task<GetRoleGroupsResult> Handle(GetRoleGroupsQuery request, CancellationToken cancellationToken)
  {
    var query = context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .ThenInclude(rt => rt.Language)
        .Include(rg => rg.RoleGroupItems)
        .ThenInclude(ri => ri.AppRole)
        .AsNoTracking()
        .ApplyRoleGroupFilters(request.Parameters)
        .ApplyRoleGroupOrdering()
        .ProjectRoleGroupListToDto();

    var roleGroups = await PagedList<RoleGroupDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetRoleGroupsResult(roleGroups, roleGroups.MetaData);
  }
}