using Users.AppRoles.Constants;
using Users.AppRoles.Dtos;
using Users.AppRoles.Exceptions;
using Users.AppRoles.Models;

namespace Users.AppRoles.Features.CreateAppRole;

public record CreateAppRoleCommand(AppRoleDto AppRole) : ICommand<CreateAppRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AppRoles];
}

public record CreateAppRoleResult(Guid Id);

public class CreateAppRoleCommandValidator : AbstractValidator<CreateAppRoleCommand>
{
  public CreateAppRoleCommandValidator()
  {
    RuleFor(x => x.AppRole.Name)
        .NotEmpty()
        .WithMessage(AppRoleConstants.Validation.AppRoleNameRequired)
        .MaximumLength(AppRoleConstants.Validation.MaxNameLength)
        .WithMessage(string.Format(AppRoleConstants.Validation.AppRoleNameLength, 1, AppRoleConstants.Validation.MaxNameLength));

    RuleFor(x => x.AppRole.Description)
        .MaximumLength(AppRoleConstants.Validation.MaxDescriptionLength)
        .WithMessage(string.Format(AppRoleConstants.Validation.AppRoleDescriptionLength, AppRoleConstants.Validation.MaxDescriptionLength));

  }
}

public class CreateAppRoleHandler(UserDbContext dbContext) : ICommandHandler<CreateAppRoleCommand, CreateAppRoleResult>
{
  public async Task<CreateAppRoleResult> Handle(CreateAppRoleCommand request, CancellationToken cancellationToken)
  {
    var role = await dbContext.AppRoles.FirstOrDefaultAsync(x => x.NormalizedName == request.AppRole.Name.ToUpperInvariant(), cancellationToken);
    if (role is not null)
    {
      throw new AppRoleAlreadyExistsException(request.AppRole.Name);
    }
    
    var appRole = CreateNewAppRole(request.AppRole);
    await dbContext.AppRoles.AddAsync(appRole, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return new CreateAppRoleResult(appRole.Id);
  }

  private AppRole CreateNewAppRole(AppRoleDto appRoleDto)
  {
    var appRole = AppRole.Create(
        appRoleDto.Name,
        appRoleDto.Description,
        appRoleDto.RoleLanguageKey 
    );
    return appRole;
  }
}