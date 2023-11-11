using System.ComponentModel.DataAnnotations;

namespace Dberries;

public class TokenAuthConsumerOptions
{
    [Required]
    public string? PublicKey { get; set; }
}