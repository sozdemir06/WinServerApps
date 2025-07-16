using Users.RoleGroups.DomainExtensions;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Enums;
using Users.RoleGroups.QueryParams;

namespace Users.RoleGroups.Features.TenantRoleGroups.GetTenantRoleGroups;

public record GetTenantRoleGroupsQuery(RoleGroupParams? Parameters = null, Guid? TenantId = null) : IQuery<GetTenantRoleGroupsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.RoleGroups, Parameters ?? new RoleGroupParams(), TenantId!);
  public string CacheGroupKey => CacheKeys.RoleGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantRoleGroupsResult(IEnumerable<RoleGroupDto> RoleGroups, PaginationMetaData MetaData);

public class GetTenantRoleGroupsHandler(UserDbContext context) : IQueryHandler<GetTenantRoleGroupsQuery, GetTenantRoleGroupsResult>
{
  public async Task<GetTenantRoleGroupsResult> Handle(GetTenantRoleGroupsQuery request, CancellationToken cancellationToken)
  {
    var parameters = request.Parameters ?? new RoleGroupParams();

    var query = context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .Include(rg => rg.RoleGroupItems)
        .ThenInclude(ri => ri.AppRole)
        .Where(rg => rg.IsActive && rg.ViewPermission == RoleGroupViewPermission.Tenant)
        .AsNoTracking()
        .ProjectRoleGroupListToDto();

    var roleGroups = await PagedList<RoleGroupDto>.ToPagedList(
        query,
        parameters.PageNumber,
        parameters.PageSize,
        cancellationToken);

    return new GetTenantRoleGroupsResult(roleGroups, roleGroups.MetaData);
  }
}