using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.ActivateTenantCategory;

public record ActivateTenantCategoryCommand(Guid Id) : ICommand<ActivateTenantCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantCategories];
}

public record ActivateTenantCategoryResult(bool Success);

public class ActivateTenantCategoryHandler(CatalogDbContext context) : ICommandHandler<ActivateTenantCategoryCommand, ActivateTenantCategoryResult>
{
  public async Task<ActivateTenantCategoryResult> Handle(ActivateTenantCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.TenantCategories
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new TenantCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    category.Activate();
    await context.SaveChangesAsync(cancellationToken);

    return new ActivateTenantCategoryResult(true);
  }
}