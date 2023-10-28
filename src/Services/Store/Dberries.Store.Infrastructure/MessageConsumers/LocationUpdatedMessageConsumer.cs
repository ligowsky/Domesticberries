using BitzArt;
using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.Infrastructure;

public class LocationUpdatedMessageConsumer : IConsumer<LocationUpdatedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationUpdatedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }
    
    public async Task Consume(ConsumeContext<LocationUpdatedMessage> context)
    {
        var location = context.Message.Location.ToModel();
        
        var existingLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value, false);

        if (existingLocation is null)
        {
            _locationsRepository.Add(location);
        }
        else
        {
            existingLocation.Patch(location)
                .Property(x => x.Name);
        }

        await _locationsRepository.SaveChangesAsync();
    }
}