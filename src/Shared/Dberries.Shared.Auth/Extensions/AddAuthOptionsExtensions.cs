using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddAuthOptionsExtensions
{
    public static void AddAuthOptions(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions.GetOptions(services, configuration);
    }
}