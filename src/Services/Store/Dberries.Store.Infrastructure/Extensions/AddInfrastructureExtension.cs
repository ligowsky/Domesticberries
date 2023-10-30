using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        Assembly assembly)
    {
        services.AddServices();
        services.AddMessaging(configuration, assembly);
    }
}