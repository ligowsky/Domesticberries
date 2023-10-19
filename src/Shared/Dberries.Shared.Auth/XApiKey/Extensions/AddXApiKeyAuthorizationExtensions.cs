using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddXApiKeyAuthorizationExtensions
{
    public static void AddXApiKeyAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection("Authentication"));
    }
}