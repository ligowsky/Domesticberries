using BitzArt;
using BitzArt.Pagination;
using MassTransit;

namespace Dberries.Warehouse.Infrastructure;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public LocationsService(ILocationsRepository locationsRepository, IPublishEndpoint publishEndpoint)
    {
        _locationsRepository = locationsRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<PageResult<Location>> GetPageAsync(PageRequest pageRequest)
    {
        return await _locationsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Location> GetAsync(Guid id)
    {
        return await _locationsRepository.GetAsync(id);
    }

    public async Task<Location> AddAsync(Location location)
    {
        var createdLocation = _locationsRepository.Add(location);
        await _locationsRepository.SaveChangesAsync();

        var message = new LocationAddedMessage(createdLocation.ToDto());
        await _publishEndpoint.Publish(message);

        return createdLocation;
    }

    public async Task<Location> UpdateAsync(Guid id, Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);

        existingLocation.Patch(location)
            .Property(x => x.Name)
            .Property(x => x.Coordinates);

        await _locationsRepository.SaveChangesAsync();

        var message = new LocationUpdatedMessage(existingLocation.ToDto());
        await _publishEndpoint.Publish(message);

        return existingLocation;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);
        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();

        var message = new LocationRemovedMessage(id);
        await _publishEndpoint.Publish(message);
    }

    public async Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest)
    {
        return await _locationsRepository.GetStockPageAsync(locationId, pageRequest);
    }

    public async Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        return await _locationsRepository.GetStockAsync(locationId, itemId);
    }

    public async Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, Stock stock)
    {
        var quantity = stock.Quantity!.Value;
        var updatedStock = await _locationsRepository.UpdateStockAsync(locationId, itemId, quantity);
        await _locationsRepository.SaveChangesAsync();

        var message = new StockChangedMessage(updatedStock?.ToDto());
        await _publishEndpoint.Publish(message);

        return updatedStock;
    }
}