using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Features.UpdateTenantBranch;

public record UpdateTenantBranchCommand(Guid Id, BranchDto Branch, Guid TenantId) : ICommand<UpdateTenantBranchResult>, ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantBranches];
  public List<string> PermissionRoles => [RoleNames.BranchEdit];
}

public record UpdateTenantBranchResult(bool Success);

public class UpdateTenantBranchHandler(UserDbContext context) : ICommandHandler<UpdateTenantBranchCommand, UpdateTenantBranchResult>
{
  public async Task<UpdateTenantBranchResult> Handle(UpdateTenantBranchCommand request, CancellationToken cancellationToken)
  {
    var branch = await context.Branches
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == request.TenantId && !x.IsDeleted, cancellationToken);

    if (branch == null)
    {
      throw new BranchNotFoundException($"Branch with ID '{request.Id}' not found in this tenant.", request.Id);
    }

    // Check if code is unique within the tenant (excluding current branch)
    if (await context.Branches.AnyAsync(x => x.Code == request.Branch.Code && x.TenantId == request.TenantId && x.Id != request.Id, cancellationToken))
    {
      throw new BranchBadRequestException($"Branch with code '{request.Branch.Code}' already exists in this tenant.");
    }

    // Check if email is unique within the tenant if provided (excluding current branch)
    if (!string.IsNullOrEmpty(request.Branch.Email) &&
        await context.Branches.AnyAsync(x => x.Email == request.Branch.Email && x.TenantId == request.TenantId && x.Id != request.Id, cancellationToken))
    {
      throw new InvalidOperationException($"Branch with email '{request.Branch.Email}' already exists in this tenant.");
    }

    branch.Update(
        request.Branch.Name,
        request.Branch.Code,
        request.Branch.Address,
        request.Branch.Phone,
        request.Branch.Email,
        request.Branch.IsActive,
        request.Branch.Description);

    await context.SaveChangesAsync(cancellationToken);

    return new UpdateTenantBranchResult(true);
  }
}