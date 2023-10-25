using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class DberriesApplicationOptions
{
    public static T Get<T>(IServiceCollection services, IConfiguration configuration, string sectionName)
        where T : class
    {
        var section = configuration.GetSection(sectionName);
        var options = section.Get<T>();

        if (options is null)
            throw new Exception($"{sectionName} options are required");

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(options)?.ToString();

            if (string.IsNullOrEmpty(value))
                throw new Exception($"{sectionName} {property.Name} is required");
        }

        services.Configure<T>(section);

        return options;
    }
}