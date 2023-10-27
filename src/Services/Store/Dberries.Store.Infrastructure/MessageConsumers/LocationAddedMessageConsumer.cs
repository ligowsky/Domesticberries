using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class LocationAddedMessageConsumer : IConsumer<LocationAddedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationAddedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }
    
    public async Task Consume(ConsumeContext<LocationAddedMessage> context)
    {
        var message = context.Message;
        var location = message.Location.ToModel();

        await _locationsRepository.Add(location);
        await _locationsRepository.SaveChangesAsync();
    }
}