using System.Text.Json.Serialization;

namespace Shared;

public class StockDto
{
    [JsonPropertyName("item")]
    public ItemDto? Item { get; set; }
    
    [JsonPropertyName("quantity")]
    public int? Quantity { get; set; }
}