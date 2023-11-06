using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dberries.Auth.Infrastructure;

public class TokenService : ITokenService
{
    private readonly IOptions<TokenAuthOptions> _tokenAuthOptions;

    public TokenService(IServiceProvider serviceProvider)
    {
        _tokenAuthOptions = serviceProvider.GetRequiredService<IOptions<TokenAuthOptions>>();
    }

    public string GenerateAccessToken(Guid userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
        };

        var privateKey = _tokenAuthOptions.Value.PrivateKey;
        var securityKey = Encoding.UTF8.GetBytes(privateKey!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}