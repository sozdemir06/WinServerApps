namespace Catalog.AppUnits.Dtos;

public record AppUnitTranslateDto(
  Guid Id,
  string Name,
  string? Description,
  string? LanguageCode,
  Guid? LanguageId,
  Guid? UnitId
);