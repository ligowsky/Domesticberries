using System.Text.Json.Serialization;

namespace Dberries;

public class ItemAvailabilityInLocationDto
{
    [JsonPropertyName("locationId")] 
    public Guid? LocationId { get; set; }
    
    [JsonPropertyName("locationName")] 
    public string? LocationName { get; set; }
    
    [JsonPropertyName("itemQuantity")] 
    public int? ItemQuantity { get; set; }
}