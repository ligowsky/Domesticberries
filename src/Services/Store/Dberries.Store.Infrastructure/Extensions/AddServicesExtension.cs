using Dberries.Auth;
using Dberries.Auth.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Infrastructure;

public static class AddServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IItemsService, ItemsService>();
        services.AddScoped<ILocationsService, LocationsService>();
    }
}