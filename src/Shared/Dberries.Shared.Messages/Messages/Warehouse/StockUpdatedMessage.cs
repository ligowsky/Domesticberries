namespace Dberries.Warehouse;

public class StockUpdatedMessage
{
    public Guid LocationId { get; set; }
    public StockDto? Stock { get; set; }

    public StockUpdatedMessage(Guid locationId, StockDto? stock)
    {
        LocationId = locationId;
        Stock = stock;
    }

    public StockUpdatedMessage()
    {
    }
}