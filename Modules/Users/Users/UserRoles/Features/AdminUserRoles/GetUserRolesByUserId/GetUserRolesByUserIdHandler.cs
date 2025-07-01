using Users.UserRoles.DomainExtensions;
using Users.UserRoles.Dtos;

namespace Users.UserRoles.Features.GetUserRolesByUserId;

public record GetUserRolesByUserIdQuery(Guid ManagerId) : IQuery<GetUserRolesByUserIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles,ManagerId);
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetUserRolesByUserIdResult(IEnumerable<UserRoleWithRoleNameDto> UserRoles);

public class GetUserRolesByUserIdHandler(UserDbContext dbContext) : IQueryHandler<GetUserRolesByUserIdQuery, GetUserRolesByUserIdResult>
{
  public async Task<GetUserRolesByUserIdResult> Handle(GetUserRolesByUserIdQuery request, CancellationToken cancellationToken)
  {
    var userRoles = await dbContext.UserRoles
        .Include(x => x.AppRole)
        .Where(x => x.ManagerId == request.ManagerId)
        .OrderBy(x => x.CreatedAt)
        .ProjectUserRoleListToDtoWithRole()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    return new GetUserRolesByUserIdResult(userRoles);
  }
}