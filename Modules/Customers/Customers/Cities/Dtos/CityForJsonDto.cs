using System.Text.Json.Serialization;


namespace Customers.Cities.Dtos;

public record CityForJsonDto
{

  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("country_id")]
  public long CountryId { get; set; }

  [JsonPropertyName("country_code")]
  public string? CountryCode { get; set; }

  [JsonPropertyName("country_name")]
  public string? CountryName { get; set; }

  [JsonPropertyName("state_code")]
  public string? StateCode { get; set; }

  [JsonPropertyName("type")]
  public string? Type { get; set; }

  [JsonPropertyName("latitude")]
  public string? Latitude { get; set; }

  [JsonPropertyName("longitude")]
  public string? Longitude { get; set; }

}