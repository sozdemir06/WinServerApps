using WinApps.Modules.Users.Users.Branches.Exceptions;
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Branches.Features.DeleteTenantBranch;

public record DeleteTenantBranchCommand(Guid Id, Guid TenantId) : ICommand<DeleteTenantBranchResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Branches];
  public List<string> PermissionRoles => [RoleNames.BranchDelete];
}

public record DeleteTenantBranchResult(bool Success);

public class DeleteTenantBranchHandler(UserDbContext context) : ICommandHandler<DeleteTenantBranchCommand, DeleteTenantBranchResult>
{
  public async Task<DeleteTenantBranchResult> Handle(DeleteTenantBranchCommand request, CancellationToken cancellationToken)
  {
    var branch = await context.Branches
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == request.TenantId && !x.IsDeleted, cancellationToken);

    if (branch == null)
    {
      throw new BranchNotFoundException($"Branch with ID '{request.Id}' not found in this tenant.", request.Id);
    }

    // Check if branch is being used by any managers
    var hasManagers = await context.Managers.AnyAsync(x => x.BranchId == request.Id, cancellationToken);
    if (hasManagers)
    {
      throw new BranchBadRequestException($"Cannot delete branch '{branch.Name}' as it is being used by managers.");
    }

    // Soft delete implementation
    branch.IsDeleted = true;

    await context.SaveChangesAsync(cancellationToken);

    return new DeleteTenantBranchResult(true);
  }
}