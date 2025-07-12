using Shared.Services.Securities;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;
using WinApps.Modules.Users.Users.Managers.Exceptions;

namespace Users.Managers.Features.TenantManagers.ChangePassword;

public record ChangePasswordCommand(Guid ManagerId, ChangePasswordDto ChangePassword) : ICommand<ChangePasswordResult>, ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => ["TenantManagers"];

  public List<string> PermissionRoles => [RoleNames.TenantManagerEdit];
}

public record ChangePasswordResult(bool Success);

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
  public ChangePasswordCommandValidator()
  {
    RuleFor(x => x.ChangePassword.CurrentPassword)
        .NotEmpty()
        .WithMessage("Current password is required");

    RuleFor(x => x.ChangePassword.NewPassword)
        .NotEmpty()
        .WithMessage("New password is required")
        .MinimumLength(6)
        .WithMessage("New password must be at least 6 characters")
        .MaximumLength(50)
        .WithMessage("New password cannot exceed 50 characters");

    RuleFor(x => x.ChangePassword.ConfirmPassword)
        .NotEmpty()
        .WithMessage("Password confirmation is required")
        .Equal(x => x.ChangePassword.NewPassword)
        .WithMessage("Password confirmation does not match new password");
  }
}

public class ChangePasswordHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<ChangePasswordHandler> logger) : ICommandHandler<ChangePasswordCommand, ChangePasswordResult>
{
  public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.ManagerId, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.ManagerId);
    }

    if (!manager.IsActive)
    {
      throw new ManagerValidationException(await localizationService.Translate("ManagerNotActive"));
    }

    // Verify current password
    var isCurrentPasswordValid = HashingHelper.VerifyPasswordHash(
        request.ChangePassword.CurrentPassword,
        manager.PasswordHash,
        manager.PasswordSalt);

    if (!isCurrentPasswordValid)
    {
      throw new InvalidPasswordException(await localizationService.Translate("InvalidCurrentPassword"));
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      // Create new password hash
      byte[] newPasswordHash, newPasswordSalt;
      HashingHelper.CreatePaswordHash(request.ChangePassword.NewPassword, out newPasswordHash, out newPasswordSalt);

      manager.ChangePassword(newPasswordHash, newPasswordSalt);

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new ChangePasswordResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to change password for manager. Id: {Id}", request.ManagerId);
      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToChangePassword"));
    }
  }
}