using Users.Data;
using Users.UserRoles.Dtos;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.TenantUserRoles.UpdateTenantUserRole;

public record UpdateTenantUserRoleCommand(Guid Id, UserRoleDto UserRole) : ICommand<UpdateTenantUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record UpdateTenantUserRoleResult(bool Success);

public class UpdateTenantUserRoleCommandValidator : AbstractValidator<UpdateTenantUserRoleCommand>
{
  public UpdateTenantUserRoleCommandValidator()
  {
    RuleFor(x => x.UserRole.ManagerId)
        .NotEmpty()
        .WithMessage("Manager ID is required");

    RuleFor(x => x.UserRole.RoleId)
        .NotEmpty()
        .WithMessage("Role ID is required");
  }
}

public class UpdateTenantUserRoleHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<UpdateTenantUserRoleHandler> logger) : ICommandHandler<UpdateTenantUserRoleCommand, UpdateTenantUserRoleResult>
{
  public async Task<UpdateTenantUserRoleResult> Handle(UpdateTenantUserRoleCommand request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles
        .Include(x => x.Manager)
        .Where(x => !x.Manager.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (userRole == null)
    {
      throw new UserRoleNotFoundException($"User role with ID {request.Id} not found");
    }

    // Verify manager exists and is a tenant manager
    var manager = await dbContext.Managers
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.UserRole.ManagerId, cancellationToken);

    if (manager == null)
    {
      throw new UserRoleValidationException(await localizationService.Translate("ManagerNotFound"));
    }

    // Verify role exists
    var role = await dbContext.AppRoles
        .FirstOrDefaultAsync(x => x.Id == request.UserRole.RoleId, cancellationToken);

    if (role == null)
    {
      throw new UserRoleValidationException(await localizationService.Translate("RoleNotFound"));
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      userRole.Update(request.UserRole.IsActive);

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateTenantUserRoleResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to update tenant user role. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new UserRoleValidationException(await localizationService.Translate("FailedToUpdateUserRole"));
    }
  }
}