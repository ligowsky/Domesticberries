using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddApiKeyOptionsExtensions
{
    public static void AddApiKeyOptions(this IServiceCollection services, IConfiguration configuration)
    {
        DberriesApplicationOptions.Get<ApiKeyOptions>(services, configuration, "Auth");
    }
}