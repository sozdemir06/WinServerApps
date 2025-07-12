using Users.Managers.Exceptions;
using Users.UserRoles.Exceptions;
using Users.UserRoles.Models;

namespace Users.UserRoles.Features.TenantUserRoles.CreateTenantUserRoles;

public record CreateTenantUserRolesCommand(Guid ManagerId, List<Guid> RoleIds) : ICommand<CreateTenantUserRolesResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles, CacheKeys.TenantManagers];
}

public record CreateTenantUserRolesResult(bool IsSuccess);

public class CreateTenantUserRolesCommandValidator : AbstractValidator<CreateTenantUserRolesCommand>
{
  public CreateTenantUserRolesCommandValidator()
  {
    RuleFor(x => x.RoleIds)
        .NotEmpty()
        .WithMessage("At least one user role must be provided.");

    RuleFor(x => x.ManagerId)
        .NotEmpty()
        .WithMessage("Manager ID is required.");
  }
}

public class CreateTenantUserRolesHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<CreateTenantUserRolesHandler> logger) : ICommandHandler<CreateTenantUserRolesCommand, CreateTenantUserRolesResult>
{
  public async Task<CreateTenantUserRolesResult> Handle(CreateTenantUserRolesCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.ManagerId, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.ManagerId);
    }

    if (!manager.IsActive)
    {
      throw new ManagerValidationException(await localizationService.Translate("ManagerNotActive"));
    }

    // Verify all roles exist
    var existingRoles = await dbContext.AppRoles
        .Where(ar => request.RoleIds.Contains(ar.Id))
        .ToListAsync(cancellationToken);

    if (existingRoles.Count != request.RoleIds.Count)
    {
      throw new UserRoleValidationException(await localizationService.Translate("OneOrMoreRolesNotFound"));
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      // Get existing user roles for this manager
      var existingUserRoles = await dbContext.UserRoles
          .Where(x => x.ManagerId == request.ManagerId)
          .ToListAsync(cancellationToken);

      var existingRoleIds = existingUserRoles.Select(x => x.RoleId).ToList();

      // Find roles to add (incoming roles that don't exist)
      var rolesToAdd = request.RoleIds.Except(existingRoleIds).ToList();

      // Find roles to remove (existing roles that are not in incoming list)
      var rolesToRemove = existingRoleIds.Except(request.RoleIds).ToList();

      // Remove roles that are not in the incoming list
      var userRolesToRemove = existingUserRoles.Where(x => rolesToRemove.Contains(x.RoleId)).ToList();
      dbContext.UserRoles.RemoveRange(userRolesToRemove);

      // Create new user roles for roles that don't exist
      foreach (var roleId in rolesToAdd)
      {
        var userRole = CreateNewUserRole(request.ManagerId, roleId);
        await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
      }

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new CreateTenantUserRolesResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to create tenant user roles. ManagerId: {ManagerId}", request.ManagerId);
      await transaction.RollbackAsync(cancellationToken);
      throw new UserRoleValidationException(await localizationService.Translate("FailedToCreateUserRoles"));
    }
  }

  private UserRole CreateNewUserRole(Guid managerId, Guid roleId)
  {
    return UserRole.Create(managerId, roleId);
  }
}