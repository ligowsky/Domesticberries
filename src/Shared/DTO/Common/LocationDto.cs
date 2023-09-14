using System.Text.Json.Serialization;

namespace Shared;

public class LocationDto : BaseEntity
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("coordinates")]
    public Coordinates? Coordinates { get; set; }

    public LocationDto(Guid? id, string? name, Coordinates? coordinates) : base(id)
    {
        Name = name;
        Coordinates = coordinates;
    }
}