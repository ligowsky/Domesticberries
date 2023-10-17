namespace Dberries.Warehouse;

public record LocationRemovedMessage
{
    public Guid Id { get; set; }

    public LocationRemovedMessage(Guid id)
    {
        Id = id;
    }

    public LocationRemovedMessage()
    {
    }
};