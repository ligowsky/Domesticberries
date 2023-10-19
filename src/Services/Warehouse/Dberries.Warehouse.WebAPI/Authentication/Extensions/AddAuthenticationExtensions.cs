namespace Dberries.Warehouse.Authentication;

public static class AddAuthenticationExtensions
{
    public static void AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
        services.AddScoped<ApiKeyAuthFilter>();
    }
}