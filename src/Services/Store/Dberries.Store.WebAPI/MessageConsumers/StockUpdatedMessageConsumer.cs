using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class StockUpdatedMessageConsumer : IConsumer<StockUpdatedMessage>
{
    private readonly ILocationsService _locationsService;

    public StockUpdatedMessageConsumer(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    public async Task Consume(ConsumeContext<StockUpdatedMessage> context)
    {
        var locationId = context.Message.LocationId!.Value;
        var itemId = context.Message.Stock!.ItemId!.Value;
        var quantity = context.Message.Stock.Quantity!.Value;
        await _locationsService.UpdateStockAsync(locationId, itemId, quantity);
    }
}