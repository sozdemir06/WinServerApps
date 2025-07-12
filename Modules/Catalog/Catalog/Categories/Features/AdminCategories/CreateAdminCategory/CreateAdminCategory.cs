
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.CreateAdminCategory;

public record CreateAdminCategoryCommand(AdminCategoryDto Category) : ICommand<CreateAdminCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminCategories];
}

public record CreateAdminCategoryResult(Guid Id);

public class CreateAdminCategoryHandler(CatalogDbContext context) : ICommandHandler<CreateAdminCategoryCommand, CreateAdminCategoryResult>
{
  public async Task<CreateAdminCategoryResult> Handle(CreateAdminCategoryCommand request, CancellationToken cancellationToken)
  {
    // Check if parent category exists if provided
    if (request.Category.ParentId.HasValue)
    {
      var parentExists = await context.AdminCategories
          .AnyAsync(x => x.Id == request.Category.ParentId.Value, cancellationToken);

      if (!parentExists)
      {
        throw new AdminCategoryBadRequestException($"Parent category with ID '{request.Category.ParentId.Value}' does not exist.");
      }
    }

    var category = await AdminCategory.Create(
        request.Category.Translates,
        request.Category.IsActive,
        request.Category.ParentId);

    await context.AdminCategories.AddAsync(category, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return new CreateAdminCategoryResult(category.Id);
  }
}