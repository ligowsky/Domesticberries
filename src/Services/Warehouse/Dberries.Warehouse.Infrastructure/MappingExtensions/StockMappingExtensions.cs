namespace Dberries.Warehouse;

public static class StockMappingExtensions
{
    public static StockDto ToDto(this Stock model)
    {
        return new StockDto
        {
            ItemId = model.ItemId,
            Item = model.Item?.ToDto(),
            Quantity = model.Quantity
        };
    }

    public static Stock ToModel(this StockDto dto)
    {
        return new Stock
        {
            Item = dto.Item?.ToModel(),
            Quantity = dto.Quantity
        };
    }
}