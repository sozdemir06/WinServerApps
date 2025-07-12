namespace WinApps.Modules.Catalog.Catalog.Categories.Dtos;

public record CategoryTranslateDto(Guid? LanguageId, string Name, string? Description, string LanguageCode);