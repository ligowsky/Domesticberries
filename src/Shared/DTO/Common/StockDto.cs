using System.Text.Json.Serialization;

namespace Shared;

public class StockDto
{
    [JsonPropertyName("itemId")]
    public Guid? ItemId { get; set; }
    
    [JsonPropertyName("locationId")]
    public Guid? LocationId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int? Quantity { get; set; }

    public StockDto(Guid? itemId, Guid? locationId, int? quantity)
    {
        ItemId = itemId;
        LocationId = locationId;
        Quantity = quantity;
    }
}