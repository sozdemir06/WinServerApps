using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.Dtos;

public record AppUnitDto(
  Guid Id,
  bool IsActive,
  bool IsDefault,
  MeasureUnitType MeasureUnitType,
  ICollection<AppUnitTranslateDto> Translates,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);