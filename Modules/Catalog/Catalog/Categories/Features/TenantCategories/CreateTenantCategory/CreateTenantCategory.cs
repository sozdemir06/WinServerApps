using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.CreateTenantCategory;

public record CreateTenantCategoryCommand(TenantCategoryDto Category, Guid TenantId) : ICommand<CreateTenantCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantCategories];
}

public record CreateTenantCategoryResult(Guid Id);

public class CreateTenantCategoryHandler(CatalogDbContext context) : ICommandHandler<CreateTenantCategoryCommand, CreateTenantCategoryResult>
{
  public async Task<CreateTenantCategoryResult> Handle(CreateTenantCategoryCommand request, CancellationToken cancellationToken)
  {
    // Check if parent category exists if provided
    if (request.Category.ParentId.HasValue)
    {
      var parentExists = await context.TenantCategories
          .AnyAsync(x => x.Id == request.Category.ParentId.Value, cancellationToken);

      if (!parentExists)
      {
        throw new TenantCategoryBadRequestException($"Parent category with ID '{request.Category.ParentId.Value}' does not exist for this tenant.");
      }
    }

    var category = await TenantCategory.Create(
        request.Category.Translates,
        request.TenantId,
        request.Category.IsActive,
        request.Category.ParentId);

    await context.TenantCategories.AddAsync(category, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return new CreateTenantCategoryResult(category.Id);
  }
}