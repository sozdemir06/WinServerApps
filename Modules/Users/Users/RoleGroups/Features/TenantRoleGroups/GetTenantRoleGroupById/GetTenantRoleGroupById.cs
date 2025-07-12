
using Users.RoleGroups.DomainExtensions;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;

namespace Users.RoleGroups.Features.TenantRoleGroups.GetTenantRoleGroupById;

public record GetTenantRoleGroupByIdQuery(Guid Id) : IQuery<GetTenantRoleGroupByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.RoleGroups, Id);
  public string CacheGroupKey => CacheKeys.RoleGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantRoleGroupByIdResult(RoleGroupDto RoleGroup);

public class GetTenantRoleGroupByIdHandler(UserDbContext context) : IQueryHandler<GetTenantRoleGroupByIdQuery, GetTenantRoleGroupByIdResult>
{
  public async Task<GetTenantRoleGroupByIdResult> Handle(GetTenantRoleGroupByIdQuery request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .ThenInclude(rt => rt.Language)
        .Include(rg => rg.RoleGroupItems)
        .ThenInclude(ri => ri.AppRole)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"Role group with ID {request.Id} not found", request.Id);
    }

    var roleGroupDto = roleGroup.ProjectRoleGroupToDto();
    return new GetTenantRoleGroupByIdResult(roleGroupDto);
  }
}