using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.DeleteTenantCategory;

public record DeleteTenantCategoryCommand(Guid Id, Guid TenantId) : ICommand<DeleteTenantCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantCategories];
}

public record DeleteTenantCategoryResult(bool Success);

public class DeleteTenantCategoryHandler(CatalogDbContext context) : ICommandHandler<DeleteTenantCategoryCommand, DeleteTenantCategoryResult>
{
  public async Task<DeleteTenantCategoryResult> Handle(DeleteTenantCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.TenantCategories
        .Include(x => x.Children)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new TenantCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    // Check if category has children
    if (category.Children.Any())
    {
      throw new TenantCategoryBadRequestException($"Cannot delete category with ID '{request.Id}' because it has child categories.");
    }

    category.Deactivate();
    await context.SaveChangesAsync(cancellationToken);

    return new DeleteTenantCategoryResult(true);
  }
}