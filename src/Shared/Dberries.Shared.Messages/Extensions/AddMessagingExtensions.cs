using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public static class AddMessagingExtensions
{
    public static void AddMessaging(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddRabbitMq(services, configuration);

            x.AddConsumers(assembly);
        });
    }
}