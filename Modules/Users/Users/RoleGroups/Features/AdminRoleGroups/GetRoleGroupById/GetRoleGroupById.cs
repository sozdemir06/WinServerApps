using Users.RoleGroups.DomainExtensions;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;

namespace Users.RoleGroups.Features.GetRoleGroupById;

public record GetRoleGroupByIdQuery(Guid Id) : IQuery<GetRoleGroupByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.RoleGroups, Id);
  public string CacheGroupKey => CacheKeys.RoleGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetRoleGroupByIdResult(RoleGroupDto RoleGroup);

public class GetRoleGroupByIdHandler(UserDbContext context) : IQueryHandler<GetRoleGroupByIdQuery, GetRoleGroupByIdResult>
{
  public async Task<GetRoleGroupByIdResult> Handle(GetRoleGroupByIdQuery request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .ThenInclude(rt => rt.Language)
        .FirstOrDefaultAsync(rg => rg.Id == request.Id && !rg.IsDeleted, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"RoleGroup with ID '{request.Id}' not found.", request.Id);
    }

    var roleGroupDto = roleGroup.ProjectRoleGroupToDto();

    return new GetRoleGroupByIdResult(roleGroupDto);
  }
}