using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class LocationRemovedMessageConsumer : IConsumer<LocationRemovedMessage>
{
    private readonly ILocationsService _locationsService;

    public LocationRemovedMessageConsumer(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    public async Task Consume(ConsumeContext<LocationRemovedMessage> context)
    {
        var id = context.Message.Id;
        var filter = new LocationFilterSet { ExternalId = id };
        
        await _locationsService.RemoveAsync(filter);
    }
}