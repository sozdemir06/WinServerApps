namespace Accounting.ExpensePens.Dtos;

public record ExpensePenTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string LanguageCode,
  Guid? LanguageId,
  Guid? ExpensePenId
);