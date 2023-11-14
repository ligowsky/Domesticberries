namespace Dberries.Store;

public class User : IEntityWithExternalId<Guid>
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }
    public string? Email { get; set; }

    public User(Guid? externalId, string? email)
    {
        ExternalId = externalId;
        Email = email;
    }

    public User()
    {
    }
}