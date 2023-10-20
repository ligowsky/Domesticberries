using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class AuthOptions
{
    [Required]
    public string? XApiKey { get; set; }
}