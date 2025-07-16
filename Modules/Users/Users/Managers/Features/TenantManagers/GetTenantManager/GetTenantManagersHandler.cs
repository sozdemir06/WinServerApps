using Users.Managers.DomainExtensions;
using Users.Managers.Dtos;
using Users.Managers.QueryParams;

namespace Users.Managers.Features.TenantManagers.GetTenantManager;

public record GetTenantManagersQuery(ManagerParams? Params = null, Guid? TenantId = null) : IQuery<GetTenantManagersResult>, ICachableRequest,IAuthorizeRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantManagers, Params ?? new ManagerParams(), TenantId!);
  public string CacheGroupKey => CacheKeys.TenantManagers;
  public TimeSpan? CacheExpiration => null;

    public List<string> PermissionRoles => [RoleNames.TenantManagerRead];
}

public record GetTenantManagersResult(IEnumerable<ManagerDto> Managers, PaginationMetaData MetaData);

public class GetTenantManagersHandler(UserDbContext dbContext) : IQueryHandler<GetTenantManagersQuery, GetTenantManagersResult>
{
  public async Task<GetTenantManagersResult> Handle(GetTenantManagersQuery request, CancellationToken cancellationToken)
  {
    var parameters = request.Params ?? new ManagerParams();

    var query = dbContext.Managers
        .Include(x => x.Tenant)
        .Include(x => x.Branch)
        .Include(x => x.UserRoles)
        .AsNoTracking()
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .ApplyQueryParams(parameters);

    var managers = await PagedList<ManagerDto>.ToPagedList(
        query.ToDto(),
        parameters.PageNumber,
        parameters.PageSize,
        cancellationToken
    );

    return new GetTenantManagersResult(managers, managers.MetaData);
  }
}