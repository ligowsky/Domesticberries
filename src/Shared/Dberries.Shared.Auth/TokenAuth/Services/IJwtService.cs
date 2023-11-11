namespace Dberries;

public interface IJwtService
{
    public AccessTokenData GetAccessTokenData(string accessToken);
}