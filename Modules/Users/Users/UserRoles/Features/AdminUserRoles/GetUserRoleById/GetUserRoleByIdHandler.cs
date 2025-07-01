using Shared.Dtos;
using Users.UserRoles.Constants;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.GetUserRoleById;

public record GetUserRoleByIdQuery(Guid managerId) : IQuery<GetUserRoleByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles, managerId);
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}
public record GetUserRoleByIdResult(ManagerRoleDto ManagerRole);

public class GetUserRoleByIdHandler(UserDbContext dbContext) : IQueryHandler<GetUserRoleByIdQuery, GetUserRoleByIdResult>
{
  public async Task<GetUserRoleByIdResult> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles
        .Include(x=>x.AppRole)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.managerId, cancellationToken);

    if (userRole == null)
      throw new UserRoleNotFoundException(string.Format(UserRoleConstants.Operations.UserRoleNotFound, request.managerId));

    var managerRoleDto = new ManagerRoleDto(
      userRole.AppRole.Id,
      userRole.AppRole.Name,
      userRole.AppRole.NormalizedName,
      userRole.AppRole.Description
    );

    return new GetUserRoleByIdResult(managerRoleDto);
  }
}