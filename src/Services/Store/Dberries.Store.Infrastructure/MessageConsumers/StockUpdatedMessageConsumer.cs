using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class StockUpdatedMessageConsumer : IConsumer<StockUpdatedMessage>
{
    public async Task Consume(ConsumeContext<StockUpdatedMessage> context)
    {
        var message = context.Message;
        var stock = message.Stock;
    }
}