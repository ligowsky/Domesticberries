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

    public async Task<Location> AddAsync(IFilterSet<Location> filter, Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(filter);

        if (existingLocation is not null)
            return existingLocation;

        _locationsRepository.Add(location);
        await _locationsRepository.SaveChangesAsync();

        return location;
    }

    public async Task<Location> UpdateAsync(IFilterSet<Location> filter, Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(filter);

        if (existingLocation is null)
        {
            _locationsRepository.Add(location);
            await _locationsRepository.SaveChangesAsync();

            return location;
        }

        _locationsRepository.Update(existingLocation, location);
        await _locationsRepository.SaveChangesAsync();

        return existingLocation;
    }

    public async Task RemoveAsync(IFilterSet<Location> filter)
    {
        var existingLocation = await _locationsRepository.GetAsync(filter);

        if (existingLocation is null) return;

        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }

    public async Task UpdateStockAsync(IFilterSet<Location> locationFilter, IFilterSet<Item> itemFilter,
        int quantity)
    {
        var item = await _itemsService.GetAsync(itemFilter);
        var stock = new Stock(item.Id!.Value, quantity);

        await _locationsRepository.UpdateStockAsync(locationFilter, stock);
        await _locationsRepository.SaveChangesAsync();
    }

    public async Task RemoveStockAsync(IFilterSet<Location> locationFilter, IFilterSet<Item> itemFilter)
    {
        var item = await _itemsService.GetAsync(itemFilter);

        await _locationsRepository.RemoveStockAsync(locationFilter, item.Id!.Value);
        await _locationsRepository.SaveChangesAsync();
    }
}