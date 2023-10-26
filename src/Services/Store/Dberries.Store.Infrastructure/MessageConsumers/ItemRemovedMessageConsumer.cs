using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class ItemRemovedMessageConsumer : IConsumer<ItemRemovedMessage>
{
    public async Task Consume(ConsumeContext<ItemRemovedMessage> context)
    {
        var message = context.Message;
        var id = message.Id;
    }
}