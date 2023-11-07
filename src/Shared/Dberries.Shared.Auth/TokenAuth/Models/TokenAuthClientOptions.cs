using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class TokenAuthClientOptions
{
    [Required]
    public string? PublicKey { get; set; }
}