using BitzArt;

namespace Dberries.Warehouse;

public class Location : IEntity
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }
    public ICollection<Stock>? Stock { get; set; }

    public void Update(Location input)
    {
        Name = input.Name ?? Name;
        Coordinates = input.Coordinates ?? Coordinates;
    }

    public Stock? UpdateStock(Guid itemId, int quantity)
    {
        if (quantity < 0)
            ApiException.BadRequest("Quantity must be grater than 0");

        var stock = Stock?.FirstOrDefault();

        if (stock is null)
        {
            stock = new Stock()
            {
                ItemId = itemId,
            };

            Stock?.Add(stock);
        }

        stock.Quantity = quantity;

        if (quantity != 0)
            return stock;

        Stock?.Remove(stock);

        return null;
    }
}