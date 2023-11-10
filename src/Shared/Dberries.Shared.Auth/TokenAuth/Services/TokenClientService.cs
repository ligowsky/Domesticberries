using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using BitzArt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dberries.Auth.Infrastructure;

internal class TokenClientService : ITokenClientService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly TokenValidationParameters _accessTokenValidationParameters;

    public TokenClientService(IServiceProvider serviceProvider)
    {
        var tokenAuthOptions = serviceProvider
            .GetRequiredService<TokenAuthClientOptions>();

        _tokenHandler = new JwtSecurityTokenHandler();

        var publicRsa = RSA.Create();
        var publicKey = Convert.FromBase64String(tokenAuthOptions.PublicKey!);
        publicRsa.ImportRSAPublicKey(publicKey, out _);

        var publicSecurityKey = new RsaSecurityKey(publicRsa);

        _accessTokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = publicSecurityKey
        };
    }

    public AccessTokenData GetAccessTokenData(string accessToken)
    {
        try
        {
            _tokenHandler.ValidateToken(accessToken, _accessTokenValidationParameters, out _);
            var jwt = _tokenHandler.ReadJwtToken(accessToken);
            var claims = jwt.Claims.ToList();

            var userId = Guid.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);

            return new AccessTokenData(userId);
        }
        catch (Exception)
        {
            throw ApiException.Unauthorized("Access token validation failed");
        }
    }
}