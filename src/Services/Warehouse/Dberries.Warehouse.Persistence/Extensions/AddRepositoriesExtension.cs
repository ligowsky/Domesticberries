using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Persistence;

public static class AddRepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IItemsRepository, ItemsRepository>();
    }
}