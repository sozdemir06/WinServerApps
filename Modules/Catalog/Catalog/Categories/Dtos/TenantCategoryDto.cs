using Catalog.AppTenants.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Dtos;

public record TenantCategoryDto(
    Guid Id,
    bool IsActive,
    Guid? TenantId,
    AppTenantDto? AppTenant,
    Guid? ParentId,
    TenantCategoryDto? Parent,
    ICollection<TenantCategoryDto> Children,
    ICollection<TenantCategoryTranslateDto> Translates,
    string? CreatedBy,
    string? ModifiedBy,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);