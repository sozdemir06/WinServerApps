using Users.UserRoles.DomainExtensions;
using Users.UserRoles.Dtos;
using Users.UserRoles.QueryParams;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRoles;

public record GetTenantUserRolesQuery(UserRoleQueryParams? UserRoleParams = null) : IQuery<GetTenantUserRolesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles, UserRoleParams ?? new UserRoleQueryParams());
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantUserRolesResult(IEnumerable<UserRoleDto> UserRoles, PaginationMetaData MetaData);

public class GetTenantUserRolesHandler(UserDbContext dbContext) : IQueryHandler<GetTenantUserRolesQuery, GetTenantUserRolesResult>
{
  public async Task<GetTenantUserRolesResult> Handle(GetTenantUserRolesQuery request, CancellationToken cancellationToken)
  {
    var parameters = request.UserRoleParams ?? new UserRoleQueryParams();

    var query = dbContext.UserRoles
        .Include(x => x.Manager)
        .Include(x => x.AppRole)
        .Where(x => !x.Manager.IsAdmin) // Filter for tenant managers only
        .OrderBy(x => x.CreatedAt)
        .ProjectUserRoleListToDto()
        .AsNoTracking()
        .AsQueryable();

    var userRoles = await PagedList<UserRoleDto>.ToPagedList(
        query,
        parameters.PageNumber, 
        parameters.PageSize,
        cancellationToken
    );

    return new GetTenantUserRolesResult(userRoles, userRoles.MetaData);
  }
}