namespace Dberries;

public interface ITokenClientService
{
    public TokenData GetAccessTokenData(string accessToken);
}