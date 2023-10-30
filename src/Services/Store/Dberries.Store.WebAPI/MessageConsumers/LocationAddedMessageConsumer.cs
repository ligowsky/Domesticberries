using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class LocationAddedMessageConsumer : IConsumer<LocationAddedMessage>
{
    private readonly ILocationsService _locationsService;

    public LocationAddedMessageConsumer(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    public async Task Consume(ConsumeContext<LocationAddedMessage> context)
    {
        var location = context.Message.Location.ToModel();
        await _locationsService.AddAsync(location);
    }
}