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
        var locationFilter = new LocationFilterSet { ExternalId = context.Message.LocationId };
        var itemFilter = new ItemFilterSet { ExternalId = context.Message.ItemId };

        await _locationsService.RemoveStockAsync(locationFilter, itemFilter);
    }
}