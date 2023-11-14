using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class LocationUpdatedMessageConsumer : IConsumer<LocationUpdatedMessage>
{
    private readonly ILocationsService _locationsService;

    public LocationUpdatedMessageConsumer(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    public async Task Consume(ConsumeContext<LocationUpdatedMessage> context)
    {
        var location = context.Message.Location.ToModel();
        await _locationsService.UpdateAsync(location.ExternalId!.Value, location);
    }
}