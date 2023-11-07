namespace Dberries.Auth;

public interface IPasswordService
{
    public string GenerateHash(string password);
    public void Validate(string password, string passwordHash);
}