using Users.Data;
using Users.UserRoles.DomainExtensions;
using Users.UserRoles.Dtos;
using Users.UserRoles.QueryParams;

namespace Users.UserRoles.Features.GetUserRoles;

public record GetUserRolesQuery(UserRoleQueryParams UserRoleParams) : IQuery<GetUserRolesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.UserRoles, UserRoleParams);
  public string CacheGroupKey => CacheKeys.UserRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetUserRolesResult(IEnumerable<UserRoleDto> UserRoles, PaginationMetaData MetaData);

public class GetUserRolesHandler(UserDbContext dbContext) : IQueryHandler<GetUserRolesQuery, GetUserRolesResult>
{
  public async Task<GetUserRolesResult> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
  {
    var query = dbContext.UserRoles
        .OrderBy(x => x.CreatedAt)
        .ProjectUserRoleListToDto()
        .AsNoTracking()
        .AsQueryable();

    var userRoles = await PagedList<UserRoleDto>.ToPagedList(query, request.UserRoleParams.PageNumber, request.UserRoleParams.PageSize, cancellationToken);

    return new GetUserRolesResult(userRoles, userRoles.MetaData);
  }
}