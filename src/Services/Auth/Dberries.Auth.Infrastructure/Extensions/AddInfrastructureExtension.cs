using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
    }
}