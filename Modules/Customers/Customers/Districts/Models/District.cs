using Customers.Cities.Models;
using Customers.Countries.Models;
using Shared.DDD;

namespace Customers.Districts.Models;

public class District : Entity<long>
{
  public string? Name { get; private set; } = default!;
  public long CityId { get; private set; }
  public City? City { get; private set; }
  public string? StateCode { get; private set; } = default!;
  public string? StateName { get; private set; } = default!;
  public long CountryId { get; private set; }
  public Country? Country { get; private set; } = default!;
  public string? CountryCode { get; private set; } = default!;
  public string? CountryName { get; private set; } = default!;
  public decimal? Latitude { get; private set; } = default!;
  public decimal? Longitude { get; private set; } = default!;
  public string? WikiDataId { get; private set; } = default!;


  private District()
  {
    // For EF Core
  }

  public static District Create(
      string name,
      long cityId,
      string stateCode,
      string stateName,
      long countryId,
      string countryCode,
      string countryName,
      decimal latitude,
      decimal longitude,
      string wikiDataId)
  {
    return new District
    {
      CreatedAt = DateTime.UtcNow,
      Name = name,
      CityId = cityId,
      StateCode = stateCode,
      StateName = stateName,
      CountryId = countryId,
      CountryCode = countryCode,
      CountryName = countryName,
      Latitude = latitude,
      Longitude = longitude,
      WikiDataId = wikiDataId
    };
  }

  public static District CreateWithId(
      long id,
      string name,
      long cityId,
      string stateCode,
      string stateName,
      long countryId,
      string countryCode,
      string countryName,
      decimal latitude,
      decimal longitude,
      string wikiDataId)
  {
    return new District
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
      Name = name,
      CityId = cityId,
      StateCode = stateCode,
      StateName = stateName,
      CountryId = countryId,
      CountryCode = countryCode,
      CountryName = countryName,
      Latitude = latitude,
      Longitude = longitude,
      WikiDataId = wikiDataId
    };
  }

  public void Update(
      string name,
      long cityId,
      string stateCode,
      string stateName,
      long countryId,
      string countryCode,
      string countryName,
      decimal latitude,
      decimal longitude,
      string wikiDataId)
  {
    Name = name;
    CityId = cityId;
    StateCode = stateCode;
    StateName = stateName;
    CountryId = countryId;
    CountryCode = countryCode;
    CountryName = countryName;
    Latitude = latitude;
    Longitude = longitude;
    WikiDataId = wikiDataId;
  }
}