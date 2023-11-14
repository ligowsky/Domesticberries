namespace Dberries.Warehouse;

public class StockRemovedMessage
{
    public Guid? LocationId { get; set; }
    public Guid? ItemId { get; set; }

    public StockRemovedMessage(Guid locationId, Guid itemId)
    {
        LocationId = locationId;
        ItemId = itemId;
    }

    public StockRemovedMessage()
    {
    }
}