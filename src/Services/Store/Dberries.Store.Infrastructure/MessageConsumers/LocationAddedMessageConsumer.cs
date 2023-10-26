using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class LocationAddedMessageConsumer : IConsumer<LocationAddedMessage>
{
    public async Task Consume(ConsumeContext<LocationAddedMessage> context)
    {
        var message = context.Message;
        var location = message.Location.ToModel();
    }
}