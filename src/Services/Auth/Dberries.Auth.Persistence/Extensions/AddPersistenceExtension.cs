using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Persistence;

public static class AddPersistenceExtension
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMsSqlDbContext(configuration);
        
        services.AddRepositories();
    }
}