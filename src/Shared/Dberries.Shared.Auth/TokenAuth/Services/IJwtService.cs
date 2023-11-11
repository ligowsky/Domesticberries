namespace Dberries;

public interface IJwtService
{
    public AccessTokenData GetTokenData(string accessToken);
}