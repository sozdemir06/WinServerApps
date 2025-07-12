using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.ActivateAdminCategory;

public record ActivateAdminCategoryCommand(Guid Id) : ICommand<ActivateAdminCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminCategories];
}

public record ActivateAdminCategoryResult(bool Success);

public class ActivateAdminCategoryHandler(CatalogDbContext context) : ICommandHandler<ActivateAdminCategoryCommand, ActivateAdminCategoryResult>
{
  public async Task<ActivateAdminCategoryResult> Handle(ActivateAdminCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await context.AdminCategories
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new AdminCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    category.Activate();
    await context.SaveChangesAsync(cancellationToken);

    return new ActivateAdminCategoryResult(true);
  }
}