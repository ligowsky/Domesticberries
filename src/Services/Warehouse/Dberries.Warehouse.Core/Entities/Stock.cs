namespace Warehouse.Core;

public class Stock
{
    public Guid? ItemId { get; set; }
    public Guid? LocationId { get; set; }
    public int? Quantity { get; set; }

    public Stock(Guid? itemId, Guid? locationId, int? quantity)
    {
        ItemId = itemId;
        LocationId = locationId;
        Quantity = quantity;
    }
}