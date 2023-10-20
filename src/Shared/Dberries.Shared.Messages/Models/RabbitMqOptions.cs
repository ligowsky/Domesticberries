using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class RabbitMqOptions
{
    [Required]
    public string? Host { get; set; }
    
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }
}