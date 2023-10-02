using System.Text.Json.Serialization;

namespace Dberries;

public class ItemDto
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("ratings")]
    public IEnumerable<RatingDto>? Ratings { get; set; }
    
    [JsonPropertyName("locations")]
    public IEnumerable<LocationDto>? Locations { get; set; }
}