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
        
        var apmUrl = section.GetValue<string>("ServerUrl");
        
        if (string.IsNullOrEmpty(apmUrl))
            throw new Exception("ElasticApm ServerUrl is required");

        var nodeUri = section.GetValue<string>("ElasticsearchNodeUri");

        if (string.IsNullOrEmpty(nodeUri))
            throw new Exception("ElasticApm ElasticsearchNodeUri is required");
        
        var environment = section.GetValue<string>("Environment");

        if (string.IsNullOrEmpty(environment))
            throw new Exception("ElasticApm Environment is required");
        
        var serviceName = section.GetValue<string>("ServiceName");

        if (string.IsNullOrEmpty(serviceName))
            throw new Exception("ElasticApm ServiceName is required");

        builder.Services.Configure<ElasticApmOptions>(section);
        
        return elasticApmOptions;
    }
        
}