using System.Text;
using BitzArt;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dberries.Auth.Infrastructure;

public class PasswordService : IPasswordService
{
    private readonly IOptions<TokenAuthProviderOptions> _tokenAuthOptions;

    public PasswordService(IServiceProvider serviceProvider)
    {
        _tokenAuthOptions = serviceProvider.GetRequiredService<IOptions<TokenAuthProviderOptions>>();
    }

    public string GenerateHash(string password)
    {
        var salt = _tokenAuthOptions.Value.Salt;
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
            throw ApiException.Forbidden("Invalid password");
    }
}