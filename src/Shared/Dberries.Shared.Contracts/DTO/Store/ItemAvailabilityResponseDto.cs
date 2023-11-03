using System.Text.Json.Serialization;

namespace Dberries;

public class ItemAvailabilityResponseDto
{
    [JsonPropertyName("availableInLocations")] 
    public IEnumerable<ItemAvailabilityInLocationDto>? AvailableInLocations { get; set; }
}