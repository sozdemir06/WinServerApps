using System.Text.Json.Serialization;

namespace Customers.Countries.Dtos;

public class CountryDto
{
  public long Id { get; init; }
  public string Name { get; init; } = string.Empty;
  public string CountryCode { get; init; } = string.Empty;
  public decimal Latitude { get; init; } = 0;
  public decimal Longitude { get; init; } = 0;
  public string Iso3 { get; init; } = string.Empty;
  public string Iso2 { get; init; } = string.Empty;
  public string NumericCode { get; init; } = string.Empty;
  public string PhoneCode { get; init; } = string.Empty;
  public string Capital { get; init; } = string.Empty;
  public string Currency { get; init; } = string.Empty;
  public string CurrencyName { get; init; } = string.Empty;
  public string CurrencySymbol { get; init; } = string.Empty;
  public string Tld { get; init; } = string.Empty;
  public string Native { get; init; } = string.Empty;
  public string Region { get; init; } = string.Empty;
  public string Subregion { get; init; } = string.Empty;
  public string Nationality { get; init; } = string.Empty;
  public string Emoji { get; init; } = string.Empty;
  public string EmojiHtml { get; init; } = string.Empty;
  public List<CityForJsonDto> Cities { get; init; } = new();
}