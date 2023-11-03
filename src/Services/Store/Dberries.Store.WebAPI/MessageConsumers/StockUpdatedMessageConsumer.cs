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
        var message = context.Message;
        var locationId = message.LocationId;
        var stock = message.Stock;
        var itemId = stock!.ItemId!.Value;
        var quantity = stock.Quantity!.Value;

        await _locationsService.UpdateStockAsync(locationId, itemId, quantity);
    }
}