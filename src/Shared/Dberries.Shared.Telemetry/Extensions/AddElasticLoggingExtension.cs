using Elastic.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace Dberries;

public static class AddElasticLoggingExtension
{
    public static WebApplicationBuilder AddElasticLogging(this WebApplicationBuilder builder)
    {
        var elasticApmOptions = ElasticApmOptions.GetOptions(builder);

        var nodeUris = new List<Uri> { new Uri(elasticApmOptions.ElasticsearchNodeUri) }.ToArray();

        builder.Logging
            .AddElasticsearch(x =>
            {
                x.Tags = new List<string> { elasticApmOptions.Environment, elasticApmOptions.ServiceName }.ToArray();
                x.ShipTo.NodeUris = nodeUris;
            });


        return builder;
    }
}