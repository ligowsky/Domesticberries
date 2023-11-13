namespace Dberries.Store;

public class Stock
{
    public Guid? ItemId { get; set; }
    public Item? Item { get; set; }
    public int? Quantity { get; set; }

    public Stock(Guid itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    public Stock()
    {
    }
}