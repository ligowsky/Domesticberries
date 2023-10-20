namespace Dberries.Warehouse;

public record ItemAddedMessage
{
    public ItemDto Item { get; set; }

    public ItemAddedMessage(ItemDto item)
    {
        Item = item;
    }

    public ItemAddedMessage()
    {
    }
}