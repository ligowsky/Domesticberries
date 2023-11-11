using System.Text.Json.Serialization;

namespace Dberries;

public class UserDto
{
    [JsonPropertyName("id")] 
    public Guid? Id { get; set; }

    [JsonPropertyName("email")] 
    public string? Email { get; set; }

    public UserDto(Guid? id, string? email)
    {
        Id = id;
        Email = email;
    }

    public UserDto()
    {
    }
}