using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddRabbitMqExtensions
{
    public static void AddRabbitMq(this IBusRegistrationConfigurator configurator, IServiceCollection services,
        IConfiguration configuration)
    {
        var options = DberriesApplicationOptions.Get<RabbitMqOptions>(services, configuration, "RabbitMQ");
        
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(options.Host, h =>
            {
                h.Username(options.Username);
                h.Password(options.Password);
            });

            cfg.ConfigureEndpoints(context);
        });
    }
}