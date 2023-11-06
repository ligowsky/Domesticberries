using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddTokenAuthOptionsExtension
{
    public static void AddTokenAuthOptions(this IServiceCollection services, IConfiguration configuration)
    {
        DberriesApplicationOptions.Get<TokenAuthOptions>(services, configuration, "Auth");
    }
}