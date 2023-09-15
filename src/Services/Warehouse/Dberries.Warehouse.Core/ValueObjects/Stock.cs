using Shared;

namespace Warehouse.Core;

public class Stock
{
    public Item? Item { get; set; }
    public int? Quantity { get; set; }

    public StockDto ToDto()
    {
        return new StockDto
        {
            Item = Item?.ToDto(),
            Quantity = Quantity
        };
    }

    public static Stock ToModel(StockDto dto)
    {
        return new Stock
        {
            Item = Item.ToModel(dto.Item),
            Quantity = dto.Quantity
        };
    }
}