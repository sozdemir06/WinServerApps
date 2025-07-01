using Users.AppRoles.Dtos;
using Users.AppRoles.Exceptions;


namespace Users.AppRoles.Features.GetAppRoleById;

public record GetAppRoleByIdQuery(Guid Id) : IQuery<GetAppRoleByIdResult>;

public record GetAppRoleByIdResult(AppRoleDto AppRole);

public class GetAppRoleByIdHandler(UserDbContext dbContext) : IQueryHandler<GetAppRoleByIdQuery, GetAppRoleByIdResult>
{
  public async Task<GetAppRoleByIdResult> Handle(GetAppRoleByIdQuery request, CancellationToken cancellationToken)
  {
    var appRole = await dbContext.AppRoles
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (appRole == null)
    {
      throw new AppRoleNotFoundException(request.Id);
    }

    var appRoleDto = new AppRoleDto(
        appRole.Id,
        appRole.Name,
        appRole.NormalizedName,
        appRole.Description,
        appRole.RoleLanguageKey,
        appRole.IsActive
    );

    return new GetAppRoleByIdResult(appRoleDto);
  }
}