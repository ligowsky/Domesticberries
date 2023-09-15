using Shared;
using Warehouse.Core;

namespace Warehouse.Presentation;

public static class StockMappingExtension
{
    public static StockDto ToDto(this Stock model)
    {
        return new StockDto
        {
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