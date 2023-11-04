using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class ElasticsearchOptions
{
    [Required]
    public string? ServerUrl { get; set; }
    
    [Required]
    public string? DefaultIndex { get; set; }
}