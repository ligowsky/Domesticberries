using System.Text.Json.Serialization;

namespace Shared;

public class BaseEntity
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    protected BaseEntity(Guid? id)
    {
        Id = id;
    }
}