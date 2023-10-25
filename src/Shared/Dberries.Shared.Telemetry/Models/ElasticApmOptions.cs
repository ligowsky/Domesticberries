namespace Dberries;

public class ElasticApmOptions
{
    public required string ServerUrl { get; set; }
    public required string ElasticsearchNodeUri { get; set; }
    public required string Environment { get; set; }
    public required string ServiceName { get; set; }
    public bool EnrichOutboundHttpRequests { get; set; }
}