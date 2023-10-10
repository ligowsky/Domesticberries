using BitzArt.Pagination;

namespace Dberries.Warehouse.Infrastructure;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationsService(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
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

        return createdLocation;
    }

    public async Task<Location> UpdateAsync(Guid id, Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);
        existingLocation.Update(location);
        await _locationsRepository.SaveChangesAsync();

        return existingLocation;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);
        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }

    public async Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest)
    {
        return await _locationsRepository.GetStockPageAsync(locationId, pageRequest);
    }

    public async Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        return await _locationsRepository.GetStockAsync(locationId, itemId);
    }

    public async Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        var stock = await _locationsRepository.UpdateStockAsync(locationId, itemId, quantity);
        await _locationsRepository.SaveChangesAsync();

        return stock;
    }
}