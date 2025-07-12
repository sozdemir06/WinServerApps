using Users.Managers.DomainExtensions;
using Users.Managers.Dtos;
using Users.Managers.QueryParams;

namespace Users.Managers.Features.AdminManagers.GetManager; 

public record GetManagersQuery(ManagerParams? Params = null) : IQuery<GetManagersResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Managers, Params ?? new ManagerParams());
  public string CacheGroupKey => CacheKeys.Managers;
  public TimeSpan? CacheExpiration => null;
}

public record GetManagersResult(IEnumerable<ManagerDto> Managers, PaginationMetaData MetaData);

public class GetManagersHandler(UserDbContext dbContext) : IQueryHandler<GetManagersQuery, GetManagersResult>
{
  public async Task<GetManagersResult> Handle(GetManagersQuery request, CancellationToken cancellationToken)
  {
    var parameters = request.Params ?? new ManagerParams();

    var query = dbContext.Managers
        .Include(x=>x.Tenant)
        .Include(x=>x.Branch)
        .Include(x=>x.UserRoles)  
        .IgnoreQueryFilters()
        .AsNoTracking()
        .ApplyQueryParams(parameters);

    var managers = await PagedList<ManagerDto>.ToPagedList(
        query.ToDto(),
        parameters.PageNumber,
        parameters.PageSize,
        cancellationToken
    );

    return new GetManagersResult(managers, managers.MetaData);
  }
}