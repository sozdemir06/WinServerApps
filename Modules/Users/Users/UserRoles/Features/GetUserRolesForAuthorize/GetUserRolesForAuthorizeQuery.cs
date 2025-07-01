using Shared.Dtos;
using StackExchange.Redis;


namespace Users.UserRoles.Features.GetUserRolesForAuthorize;

public record GetUserRolesForAuthorizeQuery(Guid ManagerId, int TimeExpirations = 1) : IQuery<GetUserRolesForAuthorizeResult>
{

}

public record GetUserRolesForAuthorizeResult(IEnumerable<ManagerRoleDto> UserRoles);

public class GetUserRolesForAuthorizeHandler(UserDbContext dbContext, IDatabase redis) : IQueryHandler<GetUserRolesForAuthorizeQuery, GetUserRolesForAuthorizeResult>
{
  public async Task<GetUserRolesForAuthorizeResult> Handle(GetUserRolesForAuthorizeQuery request, CancellationToken cancellationToken)
  {

    var cacheKey = $"Manager:Roles:{request.ManagerId}";

    var userRoles = await dbContext.UserRoles
        .Include(x => x.AppRole)
        .Where(x => x.ManagerId == request.ManagerId && x.IsActive)
        .OrderBy(x => x.CreatedAt)
        .AsNoTracking()
        .ToListAsync(cancellationToken) ?? [];


    var managerRoleDto = userRoles.Select(x => new ManagerRoleDto(
      x.AppRole.Id,
      x.AppRole.Name,
      x.AppRole.NormalizedName,
      x.AppRole.Description
    )).ToList();

    var result = await redis.StringSetAsync(cacheKey, JsonSerializer.Serialize(managerRoleDto), TimeSpan.FromDays(request.TimeExpirations));
    if (result)
    {
      return new GetUserRolesForAuthorizeResult(managerRoleDto ?? []);
    }

    return new GetUserRolesForAuthorizeResult(managerRoleDto);
  }
}