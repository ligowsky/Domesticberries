namespace Dberries.Warehouse;

public record ItemRemovedMessage
{
    public Guid Id { get; set; }

    public ItemRemovedMessage(Guid id)
    {
        Id = id;
    }

    public ItemRemovedMessage()
    {
    }
}