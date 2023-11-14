using BitzArt;

namespace Dberries.Store.Infrastructure;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IItemsService _itemsService;

    public LocationsService(ILocationsRepository locationsRepository, IItemsService itemsService)
    {
        _locationsRepository = locationsRepository;
        _itemsService = itemsService;
    }

    public async Task<Location> AddAsync(Location location)
    {
        await _locationsRepository.AddAsync(location);
        await _locationsRepository.SaveChangesAsync();

        return location;
    }

    public async Task<Location> UpdateAsync(Guid id, Location location)
    {
        var filter = new LocationFilterSet { ExternalId = id };
        var existingLocation = await _locationsRepository.GetAsync(filter);

        if (existingLocation is null)
        {
            await _locationsRepository.AddAsync(location);
            await _locationsRepository.SaveChangesAsync();

            return location;
        }

        existingLocation.Patch(location)
            .Property(x => x.Name);
        
        await _locationsRepository.SaveChangesAsync();

        return existingLocation;
    }

    public async Task RemoveAsync(Guid id)
    {
        var filter = new LocationFilterSet { ExternalId = id };
        var existingLocation = await _locationsRepository.GetAsync(filter);

        if (existingLocation is null) return;

        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }

    public async Task UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        var filter = new ItemFilterSet { ExternalId = itemId };
        var item = await _itemsService.GetAsync(filter);
        var stock = new Stock(item.Id!.Value, quantity);

        await _locationsRepository.UpdateStockAsync(locationId, stock);
        await _locationsRepository.SaveChangesAsync();
    }

    public async Task RemoveStockAsync(Guid locationId, Guid itemId)
    {
        var filter = new ItemFilterSet { ExternalId = itemId };
        var item = await _itemsService.GetAsync(filter);

        await _locationsRepository.RemoveStockAsync(locationId, item.Id!.Value);
        await _locationsRepository.SaveChangesAsync();
    }
}