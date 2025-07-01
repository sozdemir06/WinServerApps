
namespace Modules.Users.Users.AppTenants.Features.DeleteAppTenant;

public record DeleteAppTenantCommand(Guid Id) : ICommand<DeleteAppTenantResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AppTenants];
}

public record DeleteAppTenantResult(bool Success);

public class DeleteAppTenantHandler(UserDbContext dbContext) : ICommandHandler<DeleteAppTenantCommand, DeleteAppTenantResult>
{
  public async Task<DeleteAppTenantResult> Handle(DeleteAppTenantCommand request, CancellationToken cancellationToken)
  {
    var appTenant = await dbContext.AppTenants.FindAsync([request.Id], cancellationToken);

    if (appTenant == null)
    {
      return new DeleteAppTenantResult(false);
    }

    // Soft delete implementation
    appTenant.IsDeleted = true;

    await dbContext.SaveChangesAsync(cancellationToken);

    return new DeleteAppTenantResult(true);
  }
}