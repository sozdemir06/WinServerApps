namespace Accounting.Taxes.Dtos;

public record TaxDto(
  Guid Id,
  decimal Rate,
  bool IsActive,
  bool IsDefault,
  Guid? TaxGroupId,
  ICollection<TaxTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);