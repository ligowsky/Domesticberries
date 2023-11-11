namespace Dberries;

public interface ITokenProviderService
{
    public AuthResponseDto BuildAuthResponse(Guid userId);
    public TokenData GetRefreshTokenData(string token);
}