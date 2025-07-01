


namespace Modules.Users.Users.AppTenants.Features.GetAppTenants;

public record GetAppTenantsQuery(AppTenantParams AppTenantParams) : IQuery<GetAppTenantsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AppTenants, AppTenantParams);
  public string CacheGroupKey => CacheKeys.AppTenants;
  public TimeSpan? CacheExpiration => null;

}

public record GetAppTenantsResult(IEnumerable<AppTenantDto> AppTenants, PaginationMetaData MetaData);

public class GetAppTenantsHandler(UserDbContext dbContext) : IQueryHandler<GetAppTenantsQuery, GetAppTenantsResult>
{
  public async Task<GetAppTenantsResult> Handle(GetAppTenantsQuery request, CancellationToken cancellationToken)
  {
    var query = dbContext.AppTenants
        .OrderByDescending(x => x.CreatedAt)
        .FilterBySearchTerm(request.AppTenantParams.Search)
        .ProjectAppTenantListToDto()
        .AsNoTracking()
        .AsQueryable();

    var appTenants = await PagedList<AppTenantDto>.ToPagedList(query, request.AppTenantParams.PageNumber, request.AppTenantParams.PageSize, cancellationToken);

    return new GetAppTenantsResult(appTenants, appTenants.MetaData);
  }
}