namespace Dberries.Auth;

public class User : IEntity
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public User()
    {
        
    }

    public User(string? email, string? passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}