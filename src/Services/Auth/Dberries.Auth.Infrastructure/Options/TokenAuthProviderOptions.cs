using System.ComponentModel.DataAnnotations;

namespace Dberries.Auth.Infrastructure;

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
    
    [Required]
    public int RefreshTokenDurationInMinutes { get; set; }
}