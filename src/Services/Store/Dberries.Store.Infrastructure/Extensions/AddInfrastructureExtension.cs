using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Dberries.Store.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.AddTokenAuth();
        builder.Services.AddServices();
        builder.Services.AddMessaging(builder.Configuration, assembly);
    }
}