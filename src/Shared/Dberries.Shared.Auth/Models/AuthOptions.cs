using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public class AuthOptions
{
    public required string XApiKey { get; set; }

    public static AuthOptions GetOptions(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Authentication");
        var authOptions = section.Get<AuthOptions>();

        if (authOptions is null)
            throw new Exception("Authentication options are required");
        
        if (string.IsNullOrEmpty(authOptions.XApiKey))
            throw new Exception($"{nameof(XApiKey)} is required");
        
        services.Configure<AuthOptions>(section);

        return authOptions;
    }
}