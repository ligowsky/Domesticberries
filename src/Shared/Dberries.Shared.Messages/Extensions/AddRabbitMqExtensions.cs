using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddRabbitMqExtensions
{
    public static void AddRabbitMq(this IBusRegistrationConfigurator configurator, IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("RabbitMQ");
        var rabbitMqOptions = section.Get<RabbitMqOptions>();

        if (rabbitMqOptions is null)
            throw new Exception("RabbitMQ options are required");

        services.Configure<RabbitMqOptions>(section);
        
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqOptions!.Host, h =>
            {
                h.Username(rabbitMqOptions.Username);
                h.Password(rabbitMqOptions.Password);
            });

            cfg.ConfigureEndpoints(context);
        });
    }
}