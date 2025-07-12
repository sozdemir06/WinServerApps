using Catalog.Categories.Models;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.UpdateAdminCategory;

public record UpdateAdminCategoryCommand(Guid Id, AdminCategoryDto Category) : ICommand<UpdateAdminCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminCategories];
}

public record UpdateAdminCategoryResult(bool Success);

public class UpdateAdminCategoryHandler(CatalogDbContext context) : ICommandHandler<UpdateAdminCategoryCommand, UpdateAdminCategoryResult>
{
  public async Task<UpdateAdminCategoryResult> Handle(UpdateAdminCategoryCommand request, CancellationToken cancellationToken)
  {
    if (request.Category.Translates.Count == 0)
    {
      throw new AdminCategoryBadRequestException("At least one translation is required.");
    }

    var category = await context.AdminCategories
        .Include(c => c.Translates)
        .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

    if (category == null)
    {
      throw new AdminCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

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

    var validTranslations = request.Category.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue).ToList();

    // Sadece geçerli çevirilerin isimlerini kontrol et (mevcut category hariç)
    var existingCategoryNames = validTranslations.Select(t => t.Name).ToList();

    var existingCategories = await context.AdminCategories
        .Where(c => c.Id != request.Id && c.Translates.Any(ct => existingCategoryNames.Contains(ct.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingCategories.Any())
    {
      throw new AdminCategoryBadRequestException("A category with one of the provided names already exists.");
    }

    // Mevcut çevirileri veritabanından sil
    context.CategoryTranslates.RemoveRange(category.Translates);

    // Yeni çevirileri ekle
    foreach (var transDto in validTranslations)
    {
      var newTranslate = CategoryTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, category.Id);
      context.CategoryTranslates.Add(newTranslate);
    }

    category.Update(request.Category.IsActive, request.Category.ParentId);

    await context.SaveChangesAsync(cancellationToken);

    return new UpdateAdminCategoryResult(true);
  }
}