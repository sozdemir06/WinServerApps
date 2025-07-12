namespace WinApps.Modules.Catalog.Catalog.Categories.Dtos;

public record AdminCategoryDto(
    Guid Id,
    bool IsActive,
    Guid? ParentId,
    AdminCategoryDto? Parent,
    ICollection<CategoryTranslateDto> Translates
);