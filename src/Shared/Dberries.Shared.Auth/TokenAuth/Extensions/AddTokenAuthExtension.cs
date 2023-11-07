using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dberries;

public static class AddTokenAuthExtension
{
    public static void AddTokenAuth(this WebApplicationBuilder builder)
    {
        var options =
            DberriesApplicationOptions.Get<TokenAuthClientOptions>(builder.Services, builder.Configuration,
                "Auth");

        builder.Services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var securityKey = Encoding.UTF8.GetBytes(options.PublicKey!);

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey)
                };
            });

        builder.Services.AddAuthorization();
    }
}