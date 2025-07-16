namespace WinApps.Modules.Catalog.Catalog.Categories.Dtos;

public record TenantCategoryTranslateDto(Guid? LanguageId, string Name, string? Description, string LanguageCode);