using Users.Data;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.TenantUserRoles.DeleteTenantUserRole;

public record DeleteTenantUserRoleCommand(Guid Id) : ICommand<DeleteTenantUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record DeleteTenantUserRoleResult(bool Success);

public class DeleteTenantUserRoleHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<DeleteTenantUserRoleHandler> logger) : ICommandHandler<DeleteTenantUserRoleCommand, DeleteTenantUserRoleResult>
{
  public async Task<DeleteTenantUserRoleResult> Handle(DeleteTenantUserRoleCommand request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles
        .Include(x => x.Manager)
        .Where(x => !x.Manager.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (userRole == null)
    {
      throw new UserRoleNotFoundException($"User role with ID {request.Id} not found");
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      dbContext.UserRoles.Remove(userRole);
      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new DeleteTenantUserRoleResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to delete tenant user role. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new UserRoleValidationException(await localizationService.Translate("FailedToDeleteUserRole"));
    }
  }
}