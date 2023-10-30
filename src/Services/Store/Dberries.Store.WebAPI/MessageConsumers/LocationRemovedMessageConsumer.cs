using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class LocationRemovedMessageConsumer : IConsumer<LocationRemovedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationRemovedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }

    public async Task Consume(ConsumeContext<LocationRemovedMessage> context)
    {
        var id = context.Message.Id;

        var existingLocation = await _locationsRepository.GetAsync(id, false);

        if (existingLocation is null) return;

        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }
}