using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public class ElasticApmOptions
{
    public required string ServerUrl { get; set; }
    public required string ElasticsearchNodeUri { get; set; }
    public required string Environment { get; set; }
    public required string ServiceName { get; set; }
    public bool EnrichOutboundHttpRequests { get; set; }

    public static ElasticApmOptions GetOptions(WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("ElasticApm");
        var elasticApmOptions = section.Get<ElasticApmOptions>();
        
        if (elasticApmOptions is null)
            throw new Exception("ElasticApm options are required");
        
        if (string.IsNullOrEmpty(elasticApmOptions.ServerUrl))
            throw new Exception($"ElasticApm {nameof(ServerUrl)} is required");

        if (string.IsNullOrEmpty(elasticApmOptions.ElasticsearchNodeUri))
            throw new Exception($"ElasticApm {nameof(ElasticsearchNodeUri)} is required");

        if (string.IsNullOrEmpty(elasticApmOptions.Environment))
            throw new Exception($"ElasticApm {nameof(Environment)} is required");

        if (string.IsNullOrEmpty(elasticApmOptions.ServiceName))
            throw new Exception($"ElasticApm {nameof(ServiceName)} is required");

        builder.Services.Configure<ElasticApmOptions>(section);
        
        return elasticApmOptions;
    }
        
}