using Users.Data;
using Users.UserRoles.DomainExtensions;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRolesByUserId;

public record GetTenantUserRolesByUserIdQuery(Guid UserId) : IQuery<GetTenantUserRolesByUserIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles, UserId);
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantUserRolesByUserIdResult(IEnumerable<UserRoleWithRoleNameDto> UserRoles);

public class GetTenantUserRolesByUserIdHandler(UserDbContext dbContext) : IQueryHandler<GetTenantUserRolesByUserIdQuery, GetTenantUserRolesByUserIdResult>
{
  public async Task<GetTenantUserRolesByUserIdResult> Handle(GetTenantUserRolesByUserIdQuery request, CancellationToken cancellationToken)
  {
    var userRoles = await dbContext.UserRoles
        .Include(x => x.AppRole)
        .Where(x => x.ManagerId == request.UserId)
        .OrderBy(x => x.CreatedAt)
        .ProjectUserRoleListToDtoWithRole()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    return new GetTenantUserRolesByUserIdResult(userRoles);
  }
}