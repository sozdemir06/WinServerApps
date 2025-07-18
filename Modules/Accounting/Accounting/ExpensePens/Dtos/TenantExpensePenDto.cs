namespace Accounting.ExpensePens.Dtos;

public record TenantExpensePenDto(
  Guid Id,
  bool IsActive,
  bool IsDefault,
  ICollection<TenantExpensePenTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);