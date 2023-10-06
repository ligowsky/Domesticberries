namespace Dberries.Warehouse;

public class Stock
{
    public Item? Item { get; set; }
    public int? Quantity { get; set; }

    public void Update(Stock input)
    {
        Quantity = input.Quantity ?? Quantity;
    }
}