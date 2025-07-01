using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Branches.Features.CreateBranch;

public record CreateBranchCommand(BranchDto Branch) : ICommand<CreateBranchResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Branches];
}

public record CreateBranchResult(Guid Id);

public class CreateBranchHandler(UserDbContext context) : ICommandHandler<CreateBranchCommand, CreateBranchResult>
{


  public async Task<CreateBranchResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
  {

    // Check if code is unique
    if (await context.Branches.AnyAsync(x => x.Code == request.Branch.Code, cancellationToken))
    {
      throw new BranchBadRequestException($"Branch with code '{request.Branch.Code}' already exists.");
    }

    // Check if email is unique if provided
    if (!string.IsNullOrEmpty(request.Branch.Email) &&
        await context.Branches.AnyAsync(x => x.Email == request.Branch.Email, cancellationToken))
    {
      throw new InvalidOperationException($"Branch with email '{request.Branch.Email}' already exists.");
    }

    // Verify tenant exists
    var tenantExists = await context.AppTenants
        .AnyAsync(x => x.Id == request.Branch.TenantId, cancellationToken);

    if (!tenantExists)
    {
      throw new InvalidOperationException($"Tenant with ID '{request.Branch.TenantId}' does not exist.");
    }

    var branch = Branch.Create(
        request.Branch.Name,
        request.Branch.Code,
        request.Branch.Address,
        request.Branch.Phone,
        request.Branch.Email,
        request.Branch.IsActive,
        request.Branch.Description,
        request.Branch.TenantId);

    await context.Branches.AddAsync(branch, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return new CreateBranchResult(branch.Id);
  }
}