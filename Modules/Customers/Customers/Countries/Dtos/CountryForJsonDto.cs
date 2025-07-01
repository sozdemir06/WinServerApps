using System.Text.Json.Serialization;


namespace Customers.Countries.Dtos;

public record CountryForJsonDto
{

  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("country_code")]
  public string? CountryCode { get; set; }

  [JsonPropertyName("latitude")]
  public string? Latitude { get; set; }

  [JsonPropertyName("longitude")]
  public string? Longitude { get; set; }

  [JsonPropertyName("iso3")]
  public string? Iso3 { get; set; }

  [JsonPropertyName("iso2")]
  public string? Iso2 { get; set; }

  [JsonPropertyName("numeric_code")]
  public string? NumericCode { get; set; }

  [JsonPropertyName("phonecode")]
  public string? PhoneCode { get; set; }

  [JsonPropertyName("capital")]
  public string? Capital { get; set; }

  [JsonPropertyName("currency")]
  public string? Currency { get; set; }

  [JsonPropertyName("currency_name")]
  public string? CurrencyName { get; set; }

  [JsonPropertyName("currency_symbol")]
  public string? CurrencySymbol { get; set; }

  [JsonPropertyName("tld")]
  public string? Tld { get; set; }

  [JsonPropertyName("native")]
  public string? Native { get; set; }

  [JsonPropertyName("region")]
  public string? Region { get; set; }

  [JsonPropertyName("subregion")]
  public string? Subregion { get; set; }

  [JsonPropertyName("nationality")]
  public string? Nationality { get; set; }

  [JsonPropertyName("emoji")]
  public string? Emoji { get; set; }

  [JsonPropertyName("emojiHtml")]
  public string? EmojiHtml { get; set; }
}