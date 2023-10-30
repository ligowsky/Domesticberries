using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddMessaging(configuration, Assembly.GetExecutingAssembly());
    }
}