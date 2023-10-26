using BitzArt;
using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class LocationUpdatedMessageConsumer : IConsumer<LocationUpdatedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationUpdatedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }
    
    public async Task Consume(ConsumeContext<LocationUpdatedMessage> context)
    {
        var message = context.Message;
        var location = message.Location.ToModel();
        
        var existingLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value);

        existingLocation.Patch(location)
            .Property(x => x.Name);

        await _locationsRepository.SaveChangesAsync();
    }
}