using System.Text.Json.Serialization;

namespace Shared;

public class CoordinatesDto
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}