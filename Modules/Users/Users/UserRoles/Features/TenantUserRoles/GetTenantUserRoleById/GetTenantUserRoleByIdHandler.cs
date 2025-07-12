using Users.Data;
using Users.UserRoles.DomainExtensions;
using Users.UserRoles.Dtos;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.TenantUserRoles.GetTenantUserRoleById;

public record GetTenantUserRoleByIdQuery(Guid Id) : IQuery<GetTenantUserRoleByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles, Id);
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantUserRoleByIdResult(UserRoleDto UserRole);

public class GetTenantUserRoleByIdHandler(UserDbContext dbContext) : IQueryHandler<GetTenantUserRoleByIdQuery, GetTenantUserRoleByIdResult>
{
  public async Task<GetTenantUserRoleByIdResult> Handle(GetTenantUserRoleByIdQuery request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles
        .Include(x => x.Manager)
        .Include(x => x.AppRole)
        .Where(x => !x.Manager.IsAdmin) // Filter for tenant managers only
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (userRole == null)
    {
      throw new UserRoleNotFoundException($"User role with ID {request.Id} not found");
    }

    return new GetTenantUserRoleByIdResult(userRole.ProjectUserRoleToDto());
  }
}