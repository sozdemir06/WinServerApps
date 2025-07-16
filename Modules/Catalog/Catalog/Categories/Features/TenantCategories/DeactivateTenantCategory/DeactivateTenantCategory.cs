using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.DeactivateTenantCategory;

public record DeactivateTenantCategoryCommand(Guid Id) : ICommand<DeactivateTenantCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantCategories];
}

public record DeactivateTenantCategoryResult(bool Success);

public class DeactivateTenantCategoryHandler(CatalogDbContext context) : ICommandHandler<DeactivateTenantCategoryCommand, DeactivateTenantCategoryResult>
{
  public async Task<DeactivateTenantCategoryResult> Handle(DeactivateTenantCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.TenantCategories
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new TenantCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    category.Deactivate();
    await context.SaveChangesAsync(cancellationToken);

    return new DeactivateTenantCategoryResult(true);
  }
}