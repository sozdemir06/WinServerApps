

namespace WinApps.Modules.Users.Users.Branches.Features.DeleteBranch;

public record DeleteBranchCommand(Guid Id) : ICommand<DeleteBranchResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Branches];
}

public record DeleteBranchResult(bool Success);

public class DeleteBranchHandler(UserDbContext context) : ICommandHandler<DeleteBranchCommand, DeleteBranchResult>
{

  public async Task<DeleteBranchResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
  {
    var branch = await context.Branches
    .IgnoreQueryFilters()
    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (branch == null)
    {
      return new DeleteBranchResult(false);
    }

    // Soft delete implementation
    branch.Deactivate();
    await context.SaveChangesAsync(cancellationToken);

    return new DeleteBranchResult(true);
  }
}