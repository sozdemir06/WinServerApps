using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.DeleteAdminCategory;

public record DeleteAdminCategoryCommand(Guid Id) : ICommand<DeleteAdminCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminCategories];
}

public record DeleteAdminCategoryResult(bool Success);

public class DeleteAdminCategoryHandler(CatalogDbContext context) : ICommandHandler<DeleteAdminCategoryCommand, DeleteAdminCategoryResult>
{
  public async Task<DeleteAdminCategoryResult> Handle(DeleteAdminCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.AdminCategories
        .Include(x => x.Children)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new AdminCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    // Check if category has children
    if (category.Children.Any())
    {
      throw new AdminCategoryBadRequestException($"Cannot delete category with ID '{request.Id}' because it has child categories.");
    }

    context.AdminCategories.Remove(category);
    await context.SaveChangesAsync(cancellationToken); 

    return new DeleteAdminCategoryResult(true);
  }
}