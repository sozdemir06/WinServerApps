using Customers.Cities.Models;
using Customers.Districts.Models;

namespace Customers.Data.Seed;

public static class InitialData
{
  public static IEnumerable<Country> GetCountries()
  {
    var assemblyLocation = typeof(InitialData).Assembly.Location;
    var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
    var path = Path.Combine(assemblyDirectory, "RegionJson", "Countries.json");
    Console.WriteLine($"Loading countries from: {path}");

    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"Countries data file not found at: {path}");
    }

    var countriesJson = File.ReadAllText(path);
    var countryDtos = JsonSerializer.Deserialize<List<CountryForJsonDto>>(countriesJson);

    if (countryDtos == null)
    {
      throw new InvalidOperationException("Failed to deserialize countries.json");
    }

    return countryDtos.Select(dto => Country.Create(
        dto.Id,
        dto.Name ?? "",
        dto.CountryCode ?? "",
        decimal.Parse(dto.Latitude ?? "0"),
        decimal.Parse(dto.Longitude ?? "0"),
        dto.Iso3 ?? "",
        dto.Iso2 ?? dto.CountryCode ?? "",
        dto.NumericCode ?? "",
        dto.PhoneCode ?? "",
        dto.Capital ?? "",
        dto.Currency ?? "",
        dto.CurrencyName ?? "",
        dto.CurrencySymbol ?? "",
        dto.Tld ?? "",
        dto.Native ?? "",
        dto.Region ?? "",
        dto.Subregion ?? "",
        dto.Nationality ?? "",
        dto.Emoji ?? "",
        dto.EmojiHtml ?? ""));
  }

  public static IEnumerable<City> GetCities()
  {
    var assemblyLocation = typeof(InitialData).Assembly.Location;
    var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
    var path = Path.Combine(assemblyDirectory, "RegionJson", "Cities.json");

    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"Cities data file not found at: {path}");
    }

    var citiesJson = File.ReadAllText(path);
    var cities = JsonSerializer.Deserialize<List<CityForJsonDto>>(citiesJson);

    if (cities == null)
    {
      throw new InvalidOperationException("Failed to deserialize cities.json");
    }

    return cities.Select(c => City.CreateWithId(
        c.Id,
        c.Name ?? "",
        c.CountryId,
        c.CountryCode ?? "",
        c.CountryName ?? "",
        c.StateCode ?? "",
        c.Type ?? "City",
        decimal.Parse(c.Latitude ?? "0"),
        decimal.Parse(c.Longitude ?? "0")));
  }

  public static IEnumerable<District> GetDistricts()
  {
    var assemblyLocation = typeof(InitialData).Assembly.Location;
    var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
    var path = Path.Combine(assemblyDirectory, "RegionJson", "Districts.json");

    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"Districts data file not found at: {path}");
    }

    var districtsJson = File.ReadAllText(path);
    var districtDtos = JsonSerializer.Deserialize<List<DistrictForJsonDto>>(districtsJson);

    if (districtDtos == null)
    {
      throw new InvalidOperationException("Failed to deserialize districts.json");
    }

    return districtDtos.Select(d => District.CreateWithId(
        d.Id,
        d.Name ?? "",
        d.CityId,
        d.StateCode ?? "",
        d.StateName ?? "",
        d.CountryId,
        d.CountryCode ?? "",
        d.CountryName ?? "",
        decimal.Parse(d.Latitude ?? "0"),
        decimal.Parse(d.Longitude ?? "0"),
        d.WikiDataId ?? ""));
  }
}