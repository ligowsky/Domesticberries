using Elastic.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace Dberries;

public static class AddElasticLoggingExtension
{
    public static WebApplicationBuilder AddElasticLogging(this WebApplicationBuilder builder)
    {
        var options =
            DberriesApplicationOptions.Get<ElasticApmOptions>(builder.Services, builder.Configuration, "ElasticApm");

        var nodeUris = new List<Uri> { new(options.ElasticsearchNodeUri!) }.ToArray();

        builder.Logging
            .AddElasticsearch(x =>
            {
                x.Tags = new List<string> { options.Environment!, options.ServiceName! }.ToArray();
                x.ShipTo.NodeUris = nodeUris;
            });


        return builder;
    }
}