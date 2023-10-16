using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
    }
}