namespace Dberries.Auth.Tests;

public static class EntityGenerator
{
    public static AuthRequestDto GenerateAuthRequest()
    {
        return new AuthRequestDto()
        {
            Email = $"{Guid.NewGuid()}@example.com",
            Password = Guid.NewGuid().ToString()
        };
    }
}