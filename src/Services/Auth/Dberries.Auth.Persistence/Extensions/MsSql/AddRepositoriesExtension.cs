using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Persistence;

public static class AddRepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUsersRepository, UsersRepository>();
    }
}