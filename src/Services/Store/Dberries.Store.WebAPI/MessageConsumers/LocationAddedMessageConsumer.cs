using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class LocationAddedMessageConsumer : IConsumer<LocationAddedMessage>
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationAddedMessageConsumer(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }

    public async Task Consume(ConsumeContext<LocationAddedMessage> context)
    {
        var location = context.Message.Location.ToModel();

        await _locationsRepository.CheckExistsByExternalIdAsync(typeof(Location), location.ExternalId!.Value, true);
        _locationsRepository.Add(location);
        await _locationsRepository.SaveChangesAsync();
    }
}