using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Infrastructure;

public static class AddServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddSingleton<ITokenProviderService, TokenProviderService>();
    }
}