using System.Text.Json.Serialization;

namespace Dberries;

public class AuthResponseDto
{
    [JsonPropertyName("accessToken")] 
    
    public string? AccessToken { get; set; }
    
    [JsonPropertyName("refreshToken")] 
    public string? RefreshToken { get; set; }

    public AuthResponseDto(string accessToken, string refreshToke)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToke;
    }
}