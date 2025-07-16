using Users.Managers.DomainExtensions;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.TenantManagers.GetTenantManager;

public record GetTenantManagerByIdQuery(Guid Id, Guid? TenantId = null) : IQuery<GetTenantManagerByIdResult>, ICachableRequest,IAuthorizeRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantManagers, Id, TenantId!);
  public string CacheGroupKey => CacheKeys.TenantManagers;
  public TimeSpan? CacheExpiration => null;

    public List<string> PermissionRoles => [RoleNames.TenantManagerRead];
}

public record GetTenantManagerByIdResult(ManagerDto Manager);

public class GetTenantManagerByIdHandler(UserDbContext dbContext) : IQueryHandler<GetTenantManagerByIdQuery, GetTenantManagerByIdResult>
{
  public async Task<GetTenantManagerByIdResult> Handle(GetTenantManagerByIdQuery request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .Include(x => x.Tenant)
        .Include(x => x.Branch)
        .Include(x => x.UserRoles)
        .AsNoTracking()
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.Id);
    }

    return new GetTenantManagerByIdResult(manager.ToDto());
  }
}