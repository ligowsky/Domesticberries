using System.Text;
using BitzArt;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Infrastructure;

public class PasswordService : IPasswordService
{
    private readonly TokenAuthProviderOptions _tokenAuthOptions;

    public PasswordService(IServiceProvider serviceProvider)
    {
        _tokenAuthOptions = serviceProvider.GetRequiredService<TokenAuthProviderOptions>();
    }

    public string GenerateHash(string password)
    {
        var salt = _tokenAuthOptions.Salt;
        var argon = new Argon2d(Encoding.UTF8.GetBytes(password))
        {
            Salt = Encoding.UTF8.GetBytes(salt!),
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 8192
        };

        return Convert.ToBase64String(argon.GetBytes(16));
    }

    public void Validate(string password, string passwordHash)
    {
        var hash = GenerateHash(password);

        if (hash != passwordHash)
            throw ApiException.Unauthorized("Invalid password");
    }
}