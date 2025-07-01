using Customers.Cities.DomainEvents;
using Customers.Districts.DomainEvents;
using Customers.Districts.Models;
using Shared.DDD;

namespace Customers.Cities.Models;

public class City : Aggregate<long>
{


  public string? WikiDataId { get; private set; } = default!;
  public string? Name { get; private set; } = default!;
  public long CountryId { get; private set; }
  public Country? Country { get; private set; }
  public string? StateCode { get; private set; } = default!;
  public string? Type { get; private set; } = default!;
  public string? CountryCode { get; private set; } = default!;
  public string? CountryName { get; private set; } = default!;
  public decimal? Latitude { get; private set; } = default!;
  public decimal? Longitude { get; private set; } = default!;
  private readonly List<District> _districts = new();
  public IReadOnlyCollection<District> Districts => _districts.AsReadOnly();

  private City()
  {
    // For EF Core
  }

  public static City Create(
      string name,
      long countryId,
      string countryCode,
      string countryName,
      string stateCode,
      string type,
      decimal latitude,
      decimal longitude)
  {
    return new City
    {
      CreatedAt = DateTime.UtcNow,
      Name = name,
      CountryId = countryId,
      CountryCode = countryCode,
      CountryName = countryName,
      StateCode = stateCode,
      Type = type,
      Latitude = latitude,
      Longitude = longitude
    };
  }

  public static City CreateWithId(
      long id,
      string name,
      long countryId,
      string countryCode,
      string countryName,
      string stateCode,
      string type,
      decimal latitude,
      decimal longitude)
  {
    return new City
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
      Name = name,
      CountryId = countryId,
      CountryCode = countryCode,
      CountryName = countryName,
      StateCode = stateCode,
      Type = type,
      Latitude = latitude,
      Longitude = longitude
    };
  }

  public void Update(
      string name,
      long countryId,
      string countryCode,
      string countryName,
      string stateCode,
      string type,
      decimal latitude,
      decimal longitude)
  {
    Name = name;
    CountryId = countryId;
    CountryCode = countryCode;
    CountryName = countryName;
    StateCode = stateCode;
    Type = type;
    Latitude = latitude;
    Longitude = longitude;
  }

  public District AddDistrict(
      long id,
      string name,
      string stateCode,
      string stateName,
      decimal latitude,
      decimal longitude,
      string wikiDataId)
  {
    var district = District.CreateWithId(
        id,
        name,
        Id,
        stateCode,
        stateName,
        CountryId,
        CountryCode!,
        CountryName!,
        latitude,
        longitude,
        wikiDataId);

    _districts.Add(district);
    AddDomainEvent(new DistrictCreatedEvent(this, district));
    return district;
  }

  public void UpdateDistrict(
      long districtId,
      string name,
      string stateCode,
      string stateName,
      decimal latitude,
      decimal longitude,
      string wikiDataId)
  {
    var district = _districts.FirstOrDefault(d => d.Id == districtId)
        ?? throw new InvalidOperationException($"District with id {districtId} not found");

    district.Update(
        name,
        Id,
        stateCode,
        stateName,
        CountryId,
        CountryCode!,
        CountryName!,
        latitude,
        longitude,
        wikiDataId);

    AddDomainEvent(new DistrictUpdatedEvent(district));
  }

  public void RemoveDistrict(long districtId)
  {
    var district = _districts.FirstOrDefault(d => d.Id == districtId)
        ?? throw new InvalidOperationException($"District with id {districtId} not found");

    _districts.Remove(district);
    AddDomainEvent(new DistrictRemovedEvent(this, district));
  }

  public void Delete()
  {
    if (IsDeleted) return;

    IsDeleted = true;
    UpdatedAt = DateTime.UtcNow;

    AddDomainEvent(new CityRemovedEvent(this));
  }
}