using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddAuthOptionsExtensions
{
    public static void AddAuthOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Authentication");
        var authOptions = section.Get<AuthOptions>();

        if (authOptions is null)
            throw new Exception("Authentication options are required");
        
        if (string.IsNullOrEmpty(authOptions.XApiKey))
            throw new Exception("X-API-Key is required");
        
        services.Configure<AuthOptions>(section);
    }
}