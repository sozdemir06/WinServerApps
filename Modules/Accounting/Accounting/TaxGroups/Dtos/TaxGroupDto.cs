using Accounting.Taxes.Dtos;

namespace Accounting.TaxGroups.Dtos;

public record TaxGroupDto(
  Guid Id,
  bool IsActive,
  bool IsDefault,
  ICollection<TaxGroupTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt,
  ICollection<TaxDto> Taxes
);