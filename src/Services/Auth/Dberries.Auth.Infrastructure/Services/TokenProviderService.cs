using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BitzArt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dberries.Auth.Infrastructure;

public class TokenProviderService : ITokenProviderService
{
    private readonly TokenAuthProviderOptions _tokenAuthOptions;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _refreshTokenValidationParameters;


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
        
        var publicRsa = RSA.Create();
        var publicKey = Convert.FromBase64String(_tokenAuthOptions.PublicKey!);
        publicRsa.ImportSubjectPublicKeyInfo(publicKey, out _);

        var publicSecurityKey = new RsaSecurityKey(publicRsa);

        _refreshTokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = publicSecurityKey
        };
    }

    public AuthResponseDto BuildAuthResponse(Guid userId)
    {
        var accessTokenExpires = DateTime.UtcNow.AddMinutes(_tokenAuthOptions.AccessTokenDurationInMinutes);

        var accessToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            },
            expires: accessTokenExpires,
            signingCredentials: _signingCredentials
        ));
        
        var refreshTokenExpires = DateTime.UtcNow.AddMinutes(_tokenAuthOptions.RefreshTokenDurationInMinutes);

        var refreshToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            },
            expires: refreshTokenExpires,
            signingCredentials: _signingCredentials
        ));

        return new AuthResponseDto(accessToken, refreshToken);
    }
    
    public RefreshTokenData GetRefreshTokenData(string token)
    {
        try
        {
            _tokenHandler.ValidateToken(token, _refreshTokenValidationParameters, out _);
            var jwt = _tokenHandler.ReadJwtToken(token);
            var claims = jwt.Claims.ToList();

            var userId = Guid.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);

            return new RefreshTokenData(userId);
        }
        catch (Exception)
        {
            throw ApiException.Unauthorized("Refresh token validation failed");
        }
    }
}