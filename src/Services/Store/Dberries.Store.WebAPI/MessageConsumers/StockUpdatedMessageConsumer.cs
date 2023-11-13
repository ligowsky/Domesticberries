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
        var locationFilter = new LocationFilterSet { ExternalId = context.Message.LocationId };
        var itemFilter = new ItemFilterSet { ExternalId = context.Message.Stock!.ItemId };
        var quantity = context.Message.Stock.Quantity!.Value;

        await _locationsService.UpdateStockAsync(locationFilter, itemFilter, quantity);
    }
}