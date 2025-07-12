using Users.Data;
using Users.UserRoles.Constants;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.TenantUserRoles.RemoveTenantUserRoles;

public record RemoveTenantUserRolesCommand(List<Guid> UserRoleIds) : ICommand<RemoveTenantUserRolesResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles, CacheKeys.TenantManagers];
}

public record RemoveTenantUserRolesResult(int RemovedCount, int NotFoundCount, List<Guid> RemovedIds);

public class RemoveTenantUserRolesCommandValidator : AbstractValidator<RemoveTenantUserRolesCommand>
{
  public RemoveTenantUserRolesCommandValidator()
  {
    RuleFor(x => x.UserRoleIds)
        .NotEmpty()
        .WithMessage("At least one user role ID must be provided.");

    RuleForEach(x => x.UserRoleIds)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.UserIdRequired);
  }
}

public class RemoveTenantUserRolesHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<RemoveTenantUserRolesHandler> logger) : ICommandHandler<RemoveTenantUserRolesCommand, RemoveTenantUserRolesResult>
{
  public async Task<RemoveTenantUserRolesResult> Handle(RemoveTenantUserRolesCommand request, CancellationToken cancellationToken)
  {
    var removedIds = new List<Guid>();
    var notFoundCount = 0;

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      foreach (var userRoleId in request.UserRoleIds)
      {
        var userRole = await dbContext.UserRoles
            .Include(ur => ur.Manager)
            .Where(ur => !ur.Manager.IsAdmin) // Filter for tenant managers only
            .FirstOrDefaultAsync(x => x.Id == userRoleId, cancellationToken);

        if (userRole == null)
        {
          notFoundCount++;
          continue;
        }

        dbContext.UserRoles.Remove(userRole);
        removedIds.Add(userRoleId);
      }

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new RemoveTenantUserRolesResult(removedIds.Count, notFoundCount, removedIds);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to remove tenant user roles");
      await transaction.RollbackAsync(cancellationToken);
      throw new UserRoleValidationException(await localizationService.Translate("FailedToRemoveUserRoles"));
    }
  }
}