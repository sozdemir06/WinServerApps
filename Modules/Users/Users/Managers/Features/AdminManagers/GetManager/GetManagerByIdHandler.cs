using Users.Managers.DomainExtensions;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.AdminManagers.GetManager;

public record GetManagerByIdQuery(Guid Id) : IQuery<GetManagerByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Managers, Id);
  public string CacheGroupKey => CacheKeys.Managers;
  public TimeSpan? CacheExpiration => null;
}
public record GetManagerByIdResult(ManagerDetailDto Manager);

public class GetManagerByIdHandler(
    UserDbContext dbContext) : IQueryHandler<GetManagerByIdQuery, GetManagerByIdResult>
{
  public async Task<GetManagerByIdResult> Handle(GetManagerByIdQuery request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .Include(x => x.Tenant)
        .Include(x => x.Branch)
        .IgnoreQueryFilters()
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.Id);
    }

    return new GetManagerByIdResult(manager.ToDetailDto());
  }
}