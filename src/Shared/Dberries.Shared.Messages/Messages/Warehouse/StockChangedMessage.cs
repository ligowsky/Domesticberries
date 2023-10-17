namespace Dberries.Warehouse;

public class StockChangedMessage
{
    public StockDto? Stock { get; set; }

    public StockChangedMessage(StockDto? stock)
    {
        Stock = stock;
    }

    public StockChangedMessage()
    {
    }
}