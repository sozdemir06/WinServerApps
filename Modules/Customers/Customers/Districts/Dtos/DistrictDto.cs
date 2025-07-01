namespace Customers.Districts.Dtos;

public record DistrictDto
{
  public long Id { get; init; }
  public string Name { get; init; } = string.Empty;
  public string DistrictCode { get; init; } = string.Empty;
  public decimal Latitude { get; init; }
  public decimal Longitude { get; init; }
  public string StateCode { get; init; } = string.Empty;
  public string StateName { get; init; } = string.Empty;
  public long CityId { get; init; }
  public string CityName { get; init; } = string.Empty;
  public long CountryId { get; init; }
  public string CountryCode { get; init; } = string.Empty;
  public string CountryName { get; init; } = string.Empty;
  public string WikiDataId { get; init; } = string.Empty;
}