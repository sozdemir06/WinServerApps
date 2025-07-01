namespace Customers.Cities.Dtos;

public record CityDto
{
  public long Id { get; init; }
  public string Name { get; init; } = string.Empty;
  public string CityCode { get; init; } = string.Empty;
  public decimal Latitude { get; init; } = 0;
  public decimal Longitude { get; init; } = 0;
  public string StateCode { get; init; } = string.Empty;
  public string Type { get; init; } = string.Empty;
  public long CountryId { get; init; }
  public string CountryCode { get; init; } = string.Empty;
  public string CountryName { get; init; } = string.Empty;
  public List<DistrictForJsonDto> Districts { get; init; } = new();
}