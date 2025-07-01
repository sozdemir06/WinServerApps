using Customers.Cities.DomainEvents;
using Customers.Cities.Models;
using Customers.Countries.DomainEvents;
using Shared.DDD;

namespace Customers.Countries.Models;

public class Country : Aggregate<long>
{
  private readonly List<City> _cities = new();
  public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

  public string? Name { get; private set; } = default!;
  public string? CountryCode { get; private set; } = default!;
  public string? Type { get; private set; } = default!;
  public decimal? Latitude { get; private set; } = default!;
  public decimal? Longitude { get; private set; } = default!;
  public string? Iso3 { get; private set; } = default!;
  public string? Iso2 { get; private set; } = default!;
  public string? NumericCode { get; private set; } = default!;
  public string? PhoneCode { get; private set; } = default!;
  public string? Capital { get; private set; } = default!;
  public string? Currency { get; private set; } = default!;
  public string? CurrencyName { get; private set; } = default!;
  public string? CurrencySymbol { get; private set; } = default!;
  public string? Tld { get; private set; } = default!;
  public string? Native { get; private set; } = default!;
  public string? Region { get; private set; } = default!;
  public string? Subregion { get; private set; } = default!;
  public string? Nationality { get; private set; } = default!;
  public string? Emoji { get; private set; } = default!;
  public string? EmojiHtml { get; private set; } = default!;

  private Country()
  {
    // For EF Core
  }

  public static Country Create(
      long id,
      string name,
      string countryCode,
      decimal latitude,
      decimal longitude,
      string iso3,
      string iso2,
      string numericCode,
      string phoneCode,
      string capital,
      string currency,
      string currencyName,
      string currencySymbol,
      string tld,
      string native,
      string region,
      string subregion,
      string nationality,
      string emoji,
      string emojiHtml)
  {
    var country = new Country
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
      Name = name,
      CountryCode = countryCode,
      Latitude = latitude,
      Longitude = longitude,
      Iso3 = iso3,
      Iso2 = iso2,
      NumericCode = numericCode,
      PhoneCode = phoneCode,
      Capital = capital,
      Currency = currency,
      CurrencyName = currencyName,
      CurrencySymbol = currencySymbol,
      Tld = tld,
      Native = native,
      Region = region,
      Subregion = subregion,
      Nationality = nationality,
      Emoji = emoji,
      EmojiHtml = emojiHtml
    };

    country.AddDomainEvent(new CountryCreatedEvent(country));
    return country;
  }

  public void Update(
      string name,
      string countryCode,
      decimal latitude,
      decimal longitude,
      string iso3,
      string iso2,
      string numericCode,
      string phoneCode,
      string capital,
      string currency,
      string currencyName,
      string currencySymbol,
      string tld,
      string native,
      string region,
      string subregion,
      string nationality,
      string emoji,
      string emojiHtml)
  {
    Name = name;
    CountryCode = countryCode;
    Latitude = latitude;
    Longitude = longitude;
    Iso3 = iso3;
    Iso2 = iso2;
    NumericCode = numericCode;
    PhoneCode = phoneCode;
    Capital = capital;
    Currency = currency;
    CurrencyName = currencyName;
    CurrencySymbol = currencySymbol;
    Tld = tld;
    Native = native;
    Region = region;
    Subregion = subregion;
    Nationality = nationality;
    Emoji = emoji;
    EmojiHtml = emojiHtml;

    AddDomainEvent(new CountryUpdatedEvent(this));
  }

  public City AddCity(
      long id,
      string name,
      string stateCode,
      string type,
      decimal latitude,
      decimal longitude)
  {
    var city = City.CreateWithId(
        id,
        name,
        Id,
        CountryCode!,
        Name!,
        stateCode,
        type,
        latitude,
        longitude);

    _cities.Add(city);
    AddDomainEvent(new Customers.Cities.DomainEvents.CityAddedEvent(this, city));
    return city;
  }

  public void UpdateCity(
      long cityId,
      string name,
      string stateCode,
      string type,
      decimal latitude,
      decimal longitude)
  {
    var city = _cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
      throw new InvalidOperationException($"City with id {cityId} not found");

    city.Update(
        name,
        Id,
        CountryCode!,
        Name!,
        stateCode,
        type,
        latitude,
        longitude);

    AddDomainEvent(new Cities.DomainEvents.CityUpdatedEvent(city));
  }

  public void RemoveCity(long cityId)
  {
    var city = _cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
      throw new InvalidOperationException($"City with id {cityId} not found");

    _cities.Remove(city);
    AddDomainEvent(new CityRemovedEvent(city));
  }
}