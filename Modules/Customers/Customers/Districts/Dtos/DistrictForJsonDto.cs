using System.Text.Json.Serialization;

namespace Customers.Districts.Dtos;

public record DistrictForJsonDto
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("city_id")]
  public long CityId { get; set; }

  [JsonPropertyName("state_code")]
  public string? StateCode { get; set; }

  [JsonPropertyName("state_name")]
  public string? StateName { get; set; }

  [JsonPropertyName("country_id")]
  public long CountryId { get; set; }

  [JsonPropertyName("country_code")]
  public string? CountryCode { get; set; }

  [JsonPropertyName("country_name")]
  public string? CountryName { get; set; }

  [JsonPropertyName("latitude")]
  public string? Latitude { get; set; }

  [JsonPropertyName("longitude")]
  public string? Longitude { get; set; }

  [JsonPropertyName("wiki_data_id")]
  public string? WikiDataId { get; set; }
}