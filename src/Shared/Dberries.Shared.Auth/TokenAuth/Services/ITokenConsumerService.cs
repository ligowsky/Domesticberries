namespace Dberries;

public interface ITokenConsumerService
{
    public AccessTokenData GetAccessTokenData(string accessToken);
}