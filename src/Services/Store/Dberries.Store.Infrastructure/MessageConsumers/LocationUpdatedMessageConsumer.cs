using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class LocationUpdatedMessageConsumer : IConsumer<LocationUpdatedMessage>
{
    public async Task Consume(ConsumeContext<LocationUpdatedMessage> context)
    {
        var message = context.Message;
        var location = message.Location.ToModel();
    }
}