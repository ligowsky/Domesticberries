namespace Dberries;

public class AuthResponseDto
{
    public string? AccessToken { get; set; }

    public AuthResponseDto(string accessToken)
    {
        AccessToken = accessToken;
    }
}