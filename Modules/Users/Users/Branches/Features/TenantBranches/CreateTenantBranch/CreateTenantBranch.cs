using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Branches.Features.CreateTenantBranch;

public record CreateTenantBranchCommand(BranchDto Branch, Guid TenantId) : ICommand<CreateTenantBranchResult>, 
ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantBranches];

    public List<string> PermissionRoles => [RoleNames.BranchEdit];
}

public record CreateTenantBranchResult(Guid Id);

public class CreateTenantBranchHandler(UserDbContext context) : ICommandHandler<CreateTenantBranchCommand, CreateTenantBranchResult>
{
  public async Task<CreateTenantBranchResult> Handle(CreateTenantBranchCommand request, CancellationToken cancellationToken)
  {
    var appTenant = await context.AppTenants.FindAsync(request.TenantId);
    if (appTenant == null)
    {
      throw new AppTenantNotFoundException($"Tenant with ID '{request.TenantId}' does not exist.");
    }

    var branches = await context.Branches.Where(x => x.TenantId == request.TenantId).ToListAsync(cancellationToken);

    if(branches?.Count >appTenant?.AllowedBranchNumber)
    {
      throw new BranchBadRequestException($"A maximum of {appTenant?.AllowedBranchNumber} branches are allowed per tenant.");
    }

    // Check if code is unique within the tenant
    if (await context.Branches.AnyAsync(x => x.Code == request.Branch.Code && x.TenantId == request.TenantId, cancellationToken))
    {
      throw new BranchBadRequestException($"Branch with code '{request.Branch.Code}' already exists in this tenant.");
    }

    // Check if email is unique within the tenant if provided
    if (!string.IsNullOrEmpty(request.Branch.Email) &&
        await context.Branches.AnyAsync(x => x.Email == request.Branch.Email && x.TenantId == request.TenantId, cancellationToken))
    {
      throw new InvalidOperationException($"Branch with email '{request.Branch.Email}' already exists in this tenant.");
    }

    // Verify tenant exists
    var tenantExists = await context.AppTenants
        .AnyAsync(x => x.Id == request.TenantId, cancellationToken);

    if (!tenantExists)
    {
      throw new InvalidOperationException($"Tenant with ID '{request.TenantId}' does not exist.");
    }

    var branch = Branch.Create(
        request.Branch.Name,
        request.Branch.Code,
        request.Branch.Address,
        request.Branch.Phone,
        request.Branch.Email,
        request.Branch.IsActive,
        request.Branch.Description,
        request.TenantId);

    await context.Branches.AddAsync(branch, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return new CreateTenantBranchResult(branch.Id);
  }
}