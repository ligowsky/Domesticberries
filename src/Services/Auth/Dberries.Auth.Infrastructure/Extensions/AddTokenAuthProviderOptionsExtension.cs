using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Infrastructure;

public static class AddTokenAuthProviderOptionsExtension
{
    public static void AddTokenAuthProviderOptions(this IServiceCollection services, IConfiguration configuration)
    {
        DberriesApplicationOptions.Get<TokenAuthProviderOptions>(services, configuration, "Auth");
    }
}