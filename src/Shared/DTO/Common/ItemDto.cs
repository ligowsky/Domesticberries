using System.Text.Json.Serialization;

namespace Shared;

public class ItemDto : BaseEntity
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public ItemDto(Guid? id, string? name, string? description) : base(id)
    {
        Name = name;
        Description = description;
    }
}