namespace Accounting.Taxes.Dtos;

public record TaxTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid? TaxId
);