using Modules.Users.Users.AppRoles.Features.GetAppRoles;
using Users.AppRoles.DomainExtensions;
using Users.AppRoles.Dtos;
using Users.AppRoles.QueryParams;


namespace Users.AppRoles.Features.GetAppRoles;

public record GetAppRolesQuery(GetAppRolesRequest Request) : IQuery<GetAppRolesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AppRoles, Request);
  public string CacheGroupKey => CacheKeys.AppRoles;
  public TimeSpan? CacheExpiration => null;
}

public record GetAppRolesResult(IEnumerable<AppRoleDto> AppRoles, PaginationMetaData MetaData);

public class GetAppRolesHandler(UserDbContext dbContext) : IQueryHandler<GetAppRolesQuery, GetAppRolesResult>
{
  public async Task<GetAppRolesResult> Handle(GetAppRolesQuery request, CancellationToken cancellationToken)
  {
    var query = dbContext.AppRoles
        .OrderBy(x => x.CreatedAt)
        .ProjectAppRoleListToDto()
        .AsNoTracking();

    var count = await query.CountAsync(cancellationToken);
    if (count == 0)
      return new GetAppRolesResult(Enumerable.Empty<AppRoleDto>(), new PaginationMetaData());

    var appRoles = await PagedList<AppRoleDto>.ToPagedList(query, request.Request.PageNumber, request.Request.PageSize, cancellationToken);


    return new GetAppRolesResult(appRoles, appRoles.MetaData);
  }
}