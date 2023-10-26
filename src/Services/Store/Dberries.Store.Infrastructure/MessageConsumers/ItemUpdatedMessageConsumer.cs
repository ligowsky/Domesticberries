using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class ItemUpdatedMessageConsumer : IConsumer<ItemUpdatedMessage>
{
    public async Task Consume(ConsumeContext<ItemUpdatedMessage> context)
    {
        var message = context.Message;
        var item = message.Item.ToModel();
    }
}