namespace Dberries;

public interface ITokenClientService
{
    public AccessTokenData GetAccessTokenData(string accessToken);
}