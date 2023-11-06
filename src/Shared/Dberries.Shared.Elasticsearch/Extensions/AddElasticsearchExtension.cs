using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Dberries;

public static class AddElasticsearchExtension
{
    public static void AddElasticsearch(this WebApplicationBuilder builder,
        Action<ConnectionSettings> configureSettings)
    {
        var options =
            DberriesApplicationOptions.Get<ElasticsearchOptions>(builder.Services, builder.Configuration,
                "Elasticsearch");

        var settings = new ConnectionSettings(new Uri(options.ServerUrl!));
        settings.DefaultIndex(options.DefaultIndex);

        configureSettings(settings);

        builder.Services.AddScoped(_ => new ElasticClient(settings));
    }
}