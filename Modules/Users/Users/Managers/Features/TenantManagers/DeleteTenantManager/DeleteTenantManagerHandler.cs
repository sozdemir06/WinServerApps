using Shared.Exceptions;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.TenantManagers.DeleteTenantManager;

public record DeleteTenantManagerCommand(Guid Id) : ICommand<DeleteTenantManagerResult>, ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => ["TenantManagers"];

  public List<string> PermissionRoles => [RoleNames.TenantManagerDelete];
}

public record DeleteTenantManagerResult(bool Success);

public class DeleteTenantManagerHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<DeleteTenantManagerHandler> logger) : ICommandHandler<DeleteTenantManagerCommand, DeleteTenantManagerResult>
{
  public async Task<DeleteTenantManagerResult> Handle(DeleteTenantManagerCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
      .Where(x => !x.IsAdmin) // Filter for tenant managers only
      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.Id);
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      manager.Delete();
      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new DeleteTenantManagerResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to delete tenant manager. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToDeleteTenantManager"));
    }
  }
}