using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Infrastructure;

public static class AddServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<IItemsService, ItemsService>();
    }
}