namespace Shared;

public class StockDto
{
    public Guid? ItemId { get; set; }
    public Guid? LocationId { get; set; }
    public int? Quantity { get; set; }

    public StockDto(Guid? itemId, Guid? locationId, int? quantity)
    {
        ItemId = itemId;
        LocationId = locationId;
        Quantity = quantity;
    }
}