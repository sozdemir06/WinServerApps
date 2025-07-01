using Users.AppRoles.Constants;
using Users.AppRoles.Dtos;
using Users.AppRoles.Exceptions;
using Users.AppRoles.Models;


namespace Users.AppRoles.Features.UpdateAppRole;

public record UpdateAppRoleCommand(AppRoleDto AppRole) : ICommand<UpdateAppRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AppRoles];
}

public record UpdateAppRoleResult(bool Success);

public class UpdateAppRoleCommandValidator : AbstractValidator<UpdateAppRoleCommand>
{
  public UpdateAppRoleCommandValidator()
  {
    RuleFor(x => x.AppRole.Id)
        .NotEmpty()
        .WithMessage(AppRoleConstants.Operations.AppRoleInvalidId);

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

public class UpdateAppRoleHandler : ICommandHandler<UpdateAppRoleCommand, UpdateAppRoleResult>
{
  private readonly UserDbContext _dbContext;
  private readonly ILogger<UpdateAppRoleHandler> _logger;

  public UpdateAppRoleHandler(UserDbContext dbContext, ILogger<UpdateAppRoleHandler> logger)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<UpdateAppRoleResult> Handle(UpdateAppRoleCommand request, CancellationToken cancellationToken)
  {
    var appRole = await _dbContext.AppRoles.FindAsync([request.AppRole.Id], cancellationToken) ?? throw new AppRoleNotFoundException(request.AppRole.Id);

    await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      await ValidateRoleNameUniqueness(appRole, request.AppRole.Name, cancellationToken);
            UpdateAppRole(appRole, request.AppRole);

      await _dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateAppRoleResult(true);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex,
          "Failed to update app role. AppRoleId: {AppRoleId}, Name: {AppRoleName}",
          request.AppRole.Id,
          request.AppRole.Name
      );

      await transaction.RollbackAsync(cancellationToken);
      throw new AppRoleUpdateException(AppRoleConstants.Operations.AppRoleUpdateFailed);
    }
  }

  private async Task ValidateRoleNameUniqueness(AppRole currentRole, string newName, CancellationToken cancellationToken)
  {
    var normalizedNewName = newName.ToUpperInvariant();
    if (currentRole.NormalizedName == normalizedNewName)
    {
      return;
    }

    var isNameInUse = await _dbContext.AppRoles
        .AnyAsync(x => x.NormalizedName == normalizedNewName && x.Id != currentRole.Id, cancellationToken);

    if (isNameInUse)
    {
      throw new AppRoleAlreadyExistsException(newName);
    }
  }

  private static void UpdateAppRole(AppRole appRole, AppRoleDto appRoleDto)
  {
    appRole.Update(
        appRoleDto.Name,
        appRoleDto.Description,
        appRoleDto.RoleLanguageKey
    );
  }
}