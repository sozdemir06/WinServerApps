namespace Accounting.Taxes.Dtos;

public record TenantTaxTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid TenantTaxId
);