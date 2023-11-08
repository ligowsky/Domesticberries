using System.ComponentModel.DataAnnotations;

namespace Dberries.Auth;

public class TokenAuthProviderOptions
{
    [Required]
    public string? PublicKey { get; set; }
    
    [Required]
    public string? PrivateKey { get; set; }
    
    [Required]
    public string? Salt { get; set; }
    
    [Required]
    public int AccessTokenDurationInMinutes { get; set; }
}