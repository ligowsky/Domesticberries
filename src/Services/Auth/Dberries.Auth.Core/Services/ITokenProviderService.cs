namespace Dberries;

public interface ITokenProviderService
{
    public AuthResponseDto BuildAuthResponse(Guid userId);
    public RefreshTokenData GetRefreshTokenData(string token);
}