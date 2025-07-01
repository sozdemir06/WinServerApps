
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Features.UpdateBranch;

public record UpdateBranchCommand(Guid Id, BranchDto Branch) : ICommand<UpdateBranchResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Branches];
}

public record UpdateBranchResult(bool Success);

public class UpdateBranchHandler(UserDbContext context) : ICommandHandler<UpdateBranchCommand, UpdateBranchResult>
{


  public async Task<UpdateBranchResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
  {
    var branch = await context.Branches
                      .IgnoreQueryFilters()
                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (branch == null)
    {
      throw new BranchNotFoundException("Branch not found", request.Id);
    }

    // Check if code is unique (excluding current branch)
    if (await context.Branches.AnyAsync(x => x.Code == request.Branch.Code && x.Id != request.Id, cancellationToken))
    {
      throw new BranchBadRequestException($"Branch with code '{request.Branch.Code}' already exists.");
    }

    // Check if email is unique if provided (excluding current branch)
    if (!string.IsNullOrEmpty(request.Branch.Email) &&
        await context.Branches.AnyAsync(x => x.Email == request.Branch.Email && x.Id != request.Id, cancellationToken))
    {
      throw new BranchBadRequestException($"Branch with email '{request.Branch.Email}' already exists.");
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

    return new UpdateBranchResult(true);
  }
}