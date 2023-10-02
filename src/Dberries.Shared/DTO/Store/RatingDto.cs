using System.Text.Json.Serialization;

namespace Dberries;

public class RatingDto
{
    [JsonPropertyName("userId")]
    public Guid? UserId { get; set; }
    
    [JsonPropertyName("itemId")]
    public Guid? ItemId { get; set; }
    
    [JsonPropertyName("value")]
    public byte? Value { get; set; }
}