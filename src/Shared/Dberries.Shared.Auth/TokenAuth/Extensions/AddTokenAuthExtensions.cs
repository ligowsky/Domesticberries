using Dberries.Auth.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddTokenAuthExtensions
{
    public static void AddTokenAuth(this IServiceCollection services, IConfiguration configuration)
    {
        DberriesApplicationOptions.Get<TokenAuthConsumerOptions>(services, configuration, "Auth");

        services.AddSingleton<ITokenConsumerService, TokenConsumerService>();
    }
}