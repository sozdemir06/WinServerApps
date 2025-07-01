using Shared.Exceptions;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.AdminManagers.DeleteManager;

public record DeleteManagerCommand(Guid Id) : ICommand<DeleteManagerResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => ["Managers"];
}

public record DeleteManagerResult(bool Success);

public class DeleteManagerHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<DeleteManagerHandler> logger) : ICommandHandler<DeleteManagerCommand, DeleteManagerResult>
{
  public async Task<DeleteManagerResult> Handle(DeleteManagerCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
      .IgnoreQueryFilters()
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

      return new DeleteManagerResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to delete manager. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToDeleteManager"));
    }
  }
}