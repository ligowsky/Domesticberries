using System.Text.Json.Serialization;

namespace Dberries;

public class AuthRequestDto
{
    [JsonPropertyName("email")] 
    public string? Email { get; set; }
    
    [JsonPropertyName("password")] 
    public string? Password { get; set; }
}