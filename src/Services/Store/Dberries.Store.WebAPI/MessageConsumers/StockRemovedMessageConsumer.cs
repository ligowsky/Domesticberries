using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class StockRemovedMessageConsumer : IConsumer<StockRemovedMessage>
{
    private readonly ILocationsService _locationsService;

    public StockRemovedMessageConsumer(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    public async Task Consume(ConsumeContext<StockRemovedMessage> context)
    {
        var locationId = context.Message.LocationId!.Value;
        var itemId = context.Message.ItemId!.Value;
        await _locationsService.RemoveStockAsync(locationId, itemId);
    }
}