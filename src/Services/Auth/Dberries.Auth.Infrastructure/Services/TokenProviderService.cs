using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dberries.Auth.Infrastructure;

public class TokenProviderService : ITokenProviderService
{
    private readonly TokenAuthProviderOptions _tokenAuthOptions;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SigningCredentials _signingCredentials;

    public TokenProviderService(IServiceProvider serviceProvider)
    {
        _tokenAuthOptions = serviceProvider
            .GetRequiredService<TokenAuthProviderOptions>();

        _tokenHandler = new JwtSecurityTokenHandler();

        var privateRsa = RSA.Create();
        var privateKey = Convert.FromBase64String(_tokenAuthOptions.PrivateKey!);
        privateRsa.ImportRSAPrivateKey(privateKey, out _);

        var privateSecurityKey = new RsaSecurityKey(privateRsa);
        _signingCredentials = new SigningCredentials(privateSecurityKey, SecurityAlgorithms.RsaSha256);
    }

    public string GenerateAccessToken(Guid userId)
    {
        var expires = DateTime.UtcNow.AddMinutes(_tokenAuthOptions.AccessTokenDurationInMinutes);

        var accessToken = new JwtSecurityToken(
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            },
            expires: expires,
            signingCredentials: _signingCredentials
        );

        return _tokenHandler.WriteToken(accessToken);
    }
}