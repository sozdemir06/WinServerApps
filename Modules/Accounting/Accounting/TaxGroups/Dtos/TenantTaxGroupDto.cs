using Accounting.Languages.Dtos;
using Accounting.Taxes.Dtos;

namespace Accounting.TaxGroups.Dtos;

public record TenantTaxGroupDto(
  Guid Id,
  bool IsActive,
  bool IsDefault,
  Guid? TenantId,
  ICollection<TenantTaxGroupTranslateDto> Translates,
  ICollection<TenantTaxDto> Taxes,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);