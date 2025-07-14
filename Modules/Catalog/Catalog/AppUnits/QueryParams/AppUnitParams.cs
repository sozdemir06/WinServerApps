using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.QueryParams;

public record AppUnitParams : PaginationParams
{
  public string? Search { get; init; }
  public bool? IsActive { get; init; }
  public MeasureUnitType? MeasureUnitType { get; init; }
}