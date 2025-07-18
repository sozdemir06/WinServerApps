namespace Accounting.TaxGroups.Dtos;

public record TenantTaxGroupTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid TenantTaxGroupId
);