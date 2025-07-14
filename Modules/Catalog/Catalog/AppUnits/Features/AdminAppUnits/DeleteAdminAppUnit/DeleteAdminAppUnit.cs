using Catalog.AppUnits.Exceptions;
using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.Features.AdminAppUnits.DeleteAdminAppUnit;

public record DeleteAdminAppUnitCommand(Guid Id) : ICommand<DeleteAdminAppUnitResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminAppUnits];
}

public record DeleteAdminAppUnitResult(Guid Id);

public class DeleteAdminAppUnitHandler : ICommandHandler<DeleteAdminAppUnitCommand, DeleteAdminAppUnitResult>
{
  private readonly CatalogDbContext _context;

  public DeleteAdminAppUnitHandler(CatalogDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteAdminAppUnitResult> Handle(DeleteAdminAppUnitCommand request, CancellationToken cancellationToken)
  {
    var appUnit = await _context.AppUnits.FindAsync(request.Id);

    if (appUnit == null)
    {
      throw new AppUnitNotFoundException($"AppUnit with ID '{request.Id}' not found.", request.Id);
    }

    appUnit.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteAdminAppUnitResult(appUnit.Id);
  }
}