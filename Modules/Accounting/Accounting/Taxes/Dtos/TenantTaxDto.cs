namespace Accounting.Taxes.Dtos;

public record TenantTaxDto(
  Guid Id,
  decimal Rate,
  bool IsActive,
  bool IsDefault,
  Guid? TenantTaxGroupId,
  Guid? TenantId,
  ICollection<TenantTaxTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);