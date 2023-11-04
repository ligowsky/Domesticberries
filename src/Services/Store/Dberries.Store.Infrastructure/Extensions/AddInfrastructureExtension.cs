using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this WebApplicationBuilder builder,
        Assembly assembly)
    {
        builder.Services.AddServices();
        builder.Services.AddMessaging(builder.Configuration, assembly);

        builder.AddElasticsearch()
            .CreateIndices();
    }
}