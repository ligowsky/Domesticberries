namespace Dberries.Auth;

public interface ITokenService
{
    public string GenerateAccessToken(Guid userId);
}