using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class StockUpdatedMessageConsumer : IConsumer<StockUpdatedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public StockUpdatedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }
    
    public async Task Consume(ConsumeContext<StockUpdatedMessage> context)
    {
        var message = context.Message;
        var locationId = message.LocationId;
        var stock = message.Stock;
        var itemId = stock!.Item!.Id!.Value;
        var quantity = stock.Quantity!.Value;

        await _locationsRepository.UpdateStockAsync(locationId, itemId, quantity);
        await _locationsRepository.SaveChangesAsync();
    }
}