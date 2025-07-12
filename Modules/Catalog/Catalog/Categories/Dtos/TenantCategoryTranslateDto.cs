namespace WinApps.Modules.Catalog.Catalog.Categories.Dtos;

public record TenantCategoryTranslateDto(
    Guid Id,
    string Name,
    string? Description,
    Guid? LanguageId,
    Guid? CategoryId
);