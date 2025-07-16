namespace Accounting.TaxGroups.Dtos;

public record TaxGroupTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid? TaxGroupId
);