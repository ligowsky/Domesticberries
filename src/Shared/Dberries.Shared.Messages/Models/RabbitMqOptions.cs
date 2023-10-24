using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public class RabbitMqOptions
{
    public required string? Host { get; set; }
    public required string? Username { get; set; }
    public required string? Password { get; set; }

    public static RabbitMqOptions GetOptions(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("RabbitMQ");
        var rabbitMqOptions = section.Get<RabbitMqOptions>();

        if (rabbitMqOptions is null)
            throw new Exception("RabbitMQ options are required");
        
        if (string.IsNullOrEmpty(rabbitMqOptions.Host))
            throw new Exception("RabbitMQ Host is required");

        if (string.IsNullOrEmpty(rabbitMqOptions.Username))
            throw new Exception("RabbitMQ Username is required");

        if (string.IsNullOrEmpty(rabbitMqOptions.Password))
            throw new Exception("RabbitMQ Password is required");

        services.Configure<RabbitMqOptions>(section);
        
        return rabbitMqOptions;
    }
}