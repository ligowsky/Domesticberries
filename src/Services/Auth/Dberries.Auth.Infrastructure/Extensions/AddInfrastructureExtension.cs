using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTokenAuthProviderOptions(configuration);
        services.AddServices();
    }
}