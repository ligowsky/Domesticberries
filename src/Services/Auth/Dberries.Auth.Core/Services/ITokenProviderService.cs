namespace Dberries;

public interface ITokenProviderService
{
    public string GenerateAccessToken(Guid userId);
}