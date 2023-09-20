using System.Text.Json.Serialization;

namespace Dberries;

public class LocationDto
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("coordinates")]
    public CoordinatesDto? Coordinates { get; set; }
    
    [JsonPropertyName("stock")]
    public IEnumerable<StockDto>? Stock { get; set; }
}