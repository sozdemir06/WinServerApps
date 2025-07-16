using Catalog.Categories.Models;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.UpdateTenantCategory;

public record UpdateTenantCategoryCommand(Guid Id, TenantCategoryDto Category) : ICommand<UpdateTenantCategoryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantCategories];
}

public record UpdateTenantCategoryResult(bool Success);

public class UpdateTenantCategoryHandler(CatalogDbContext context) : ICommandHandler<UpdateTenantCategoryCommand, UpdateTenantCategoryResult>
{
  public async Task<UpdateTenantCategoryResult> Handle(UpdateTenantCategoryCommand request, CancellationToken cancellationToken)
  {
    if (request.Category.Translates.Count == 0)
    {
      throw new TenantCategoryBadRequestException("At least one translation is required.");
    }

    var category = await context.TenantCategories
        .Include(c => c.Translates)
        .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

    if (category == null)
    {
      throw new TenantCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    // Check if parent category exists if provided
    if (request.Category.ParentId.HasValue)
    {
      var parentExists = await context.TenantCategories
          .AnyAsync(x => x.Id == request.Category.ParentId.Value , cancellationToken);

      if (!parentExists)
      {
        throw new TenantCategoryBadRequestException($"Parent category with ID '{request.Category.ParentId.Value}' does not exist for this tenant.");
      }
    }

    var validTranslations = request.Category.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue).ToList();

    // Sadece geçerli çevirilerin isimlerini kontrol et (mevcut category hariç)
    var existingCategoryNames = validTranslations.Select(t => t.Name).ToList();

    var existingCategories = await context.TenantCategories
        .Where(c => c.Id != request.Id && c.TenantId == request.Category.TenantId && c.Translates.Any(ct => existingCategoryNames.Contains(ct.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingCategories.Any())
    {
      throw new TenantCategoryBadRequestException("A category with one of the provided names already exists for this tenant.");
    }

    // Mevcut çevirileri veritabanından sil
    context.TenantCategoryTranslates.RemoveRange(category.Translates);

    // Yeni çevirileri ekle
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantCategoryTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, category.Id);
      context.TenantCategoryTranslates.Add(newTranslate);
    }

    category.Update(request.Category.IsActive, request.Category.ParentId);

    await context.SaveChangesAsync(cancellationToken);

    return new UpdateTenantCategoryResult(true);
  }
}