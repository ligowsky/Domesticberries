using System.Text.Json.Serialization;

namespace Dberries;

public class RefreshTokenRequestDto
{
    [JsonPropertyName("refreshToken")] 
    public string? RefreshToken { get; set; }

    public RefreshTokenRequestDto(string refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public RefreshTokenRequestDto()
    {
        
    }
}