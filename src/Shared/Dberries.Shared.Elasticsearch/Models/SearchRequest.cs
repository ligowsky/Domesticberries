using System.Text.Json.Serialization;

namespace Dberries;

public class SearchRequest : ISearchRequest
{
    [JsonPropertyName("q")] 
    public string? Q { get; set; }

    public SearchRequest()
        : this(string.Empty)
    {
    }

    public SearchRequest(string? q)
    {
        Q = q;
    }
}