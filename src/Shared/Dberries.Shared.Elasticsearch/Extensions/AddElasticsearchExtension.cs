using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Dberries;

public static class AddElasticsearchExtension
{
    public static IElasticClient AddElasticsearch(this WebApplicationBuilder builder)
    {
        var options =
            DberriesApplicationOptions.Get<ElasticsearchOptions>(builder.Services, builder.Configuration,
                "Elasticsearch");

        var settings = new ConnectionSettings(new Uri(options.ServerUrl!));
        settings.DefaultIndex(options.DefaultIndex);
        
        var client = new ElasticClient(settings);
        
        builder.Services.AddSingleton<IElasticClient>(client);

        return client;
    }
}