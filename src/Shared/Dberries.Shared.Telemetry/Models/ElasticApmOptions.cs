using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class ElasticApmOptions
{
    [Required]
    public string? ServerUrl { get; set; }
    
    [Required]
    public string? ElasticsearchNodeUri { get; set; }
    
    [Required]
    public string? Environment { get; set; }
    
    [Required]
    public string? ServiceName { get; set; }
    
    public bool EnrichOutboundHttpRequests { get; set; }
}