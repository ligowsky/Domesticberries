using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class ApiKeyOptions
{
    [Required]
    public string XApiKey { get; set; }
}