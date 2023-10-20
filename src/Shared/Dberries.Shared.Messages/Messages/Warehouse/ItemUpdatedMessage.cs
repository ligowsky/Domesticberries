namespace Dberries.Warehouse;

public record ItemUpdatedMessage
{
    public ItemDto Item { get; set; }

    public ItemUpdatedMessage(ItemDto item)
    {
        Item = item;
    }

    public ItemUpdatedMessage()
    {
    }
}