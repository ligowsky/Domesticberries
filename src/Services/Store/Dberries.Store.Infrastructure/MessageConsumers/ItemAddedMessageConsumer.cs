using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class ItemAddedMessageConsumer : IConsumer<ItemAddedMessage>
{
    public async Task Consume(ConsumeContext<ItemAddedMessage> context)
    {
        var message = context.Message;
        var item = message.Item.ToModel();
    }
}