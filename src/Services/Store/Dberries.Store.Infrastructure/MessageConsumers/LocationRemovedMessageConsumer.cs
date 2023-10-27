using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.Infrastructure;

public class LocationRemovedMessageConsumer : IConsumer<LocationRemovedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationRemovedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }
    
    public async Task Consume(ConsumeContext<LocationRemovedMessage> context)
    {
        var message = context.Message;
        var id = message.Id;
        
        var existingLocation = await _locationsRepository.GetAsync(id);
        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }
}