using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class LocationRemovedMessageConsumer : IConsumer<LocationRemovedMessage>
{
    public async Task Consume(ConsumeContext<LocationRemovedMessage> context)
    {
        var message = context.Message;
        var id = message.Id;
    }
}