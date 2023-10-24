using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddRabbitMqExtensions
{
    public static void AddRabbitMq(this IBusRegistrationConfigurator configurator, IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqOptions = RabbitMqOptions.GetOptions(services, configuration);
        
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqOptions.Host, h =>
            {
                h.Username(rabbitMqOptions.Username);
                h.Password(rabbitMqOptions.Password);
            });

            cfg.ConfigureEndpoints(context);
        });
    }
}