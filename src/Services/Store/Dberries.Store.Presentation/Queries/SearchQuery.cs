using System.Text.Json.Serialization;

namespace Dberries.Store.Presentation;

public class SearchQuery
{
    [JsonPropertyName("q")] 
    public string? Q { get; set; }

    public SearchQuery()
        : this(string.Empty)
    {
    }

    public SearchQuery(string? q)
    {
        Q = q;
    }
}