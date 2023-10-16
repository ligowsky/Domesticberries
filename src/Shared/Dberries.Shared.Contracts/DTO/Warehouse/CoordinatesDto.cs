using System.Text.Json.Serialization;

namespace Dberries;

public class CoordinatesDto
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}