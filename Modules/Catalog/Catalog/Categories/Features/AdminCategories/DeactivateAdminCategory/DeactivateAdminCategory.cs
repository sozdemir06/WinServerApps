using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.DeactivateAdminCategory;

public record DeactivateAdminCategoryCommand(Guid Id) : ICommand<DeactivateAdminCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminCategories];
}

public record DeactivateAdminCategoryResult(bool Success);

public class DeactivateAdminCategoryHandler(CatalogDbContext context) : ICommandHandler<DeactivateAdminCategoryCommand, DeactivateAdminCategoryResult>
{
  public async Task<DeactivateAdminCategoryResult> Handle(DeactivateAdminCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.AdminCategories
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new AdminCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    category.Deactivate();
    await context.SaveChangesAsync(cancellationToken);

    return new DeactivateAdminCategoryResult(true);
  }
}