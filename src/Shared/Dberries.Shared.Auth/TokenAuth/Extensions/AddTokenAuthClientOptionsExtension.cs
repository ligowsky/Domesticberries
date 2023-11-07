using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddTokenAuthClientOptionsExtension
{
    public static void AddTokenAuthClientOptions(this IServiceCollection services, IConfiguration configuration)
    {
        DberriesApplicationOptions.Get<TokenAuthClientOptions>(services, configuration, "Auth");
    }
}