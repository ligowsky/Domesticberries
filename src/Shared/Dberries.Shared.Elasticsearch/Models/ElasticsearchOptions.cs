namespace Dberries;

public class ElasticsearchOptions
{
    public required string ServerUrl { get; set; }
    public required string ElasticsearchNodeUri { get; set; }
    public required string Environment { get; set; }
    public required string ServiceName { get; set; }
}