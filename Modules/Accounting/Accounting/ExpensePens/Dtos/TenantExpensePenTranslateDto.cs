namespace Accounting.ExpensePens.Dtos;

public record TenantExpensePenTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid? TenantExpensePenId
);