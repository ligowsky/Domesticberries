using System.Text.Json.Serialization;

namespace Dberries;

public class SearchRequestDto
{
    [JsonPropertyName("q")] 
    public string? Q { get; set; }

    public SearchRequestDto()
        : this(string.Empty)
    {
    }

    public SearchRequestDto(string? q)
    {
        Q = q;
    }
}