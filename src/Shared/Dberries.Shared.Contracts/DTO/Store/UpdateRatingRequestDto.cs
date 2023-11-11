using System.Text.Json.Serialization;

namespace Dberries;

public class UpdateRatingRequestDto
{
    [JsonPropertyName("value")]
    public int? Value { get; set; }
}