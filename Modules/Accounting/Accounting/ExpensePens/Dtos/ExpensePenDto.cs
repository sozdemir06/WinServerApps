namespace Accounting.ExpensePens.Dtos;

public record ExpensePenDto(
  Guid Id,
  bool IsActive,
  bool IsDefault,
  ICollection<ExpensePenTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);