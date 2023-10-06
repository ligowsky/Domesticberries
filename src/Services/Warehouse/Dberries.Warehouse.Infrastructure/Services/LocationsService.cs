using BitzArt.Pagination;

namespace Dberries.Warehouse.Infrastructure;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationsService(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }

    public async Task<PageResult<Location>> GetLocationsPageAsync(PageRequest pageRequest)
    {
        return await _locationsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Location> GetLocationAsync(Guid id)
    {
        return await _locationsRepository.GetAsync(id);
    }

    public async Task<Location> CreateLocationAsync(Location location)
    {
        var createdLocation = await _locationsRepository.CreateAsync(location);
        await _locationsRepository.SaveChangesAsync();

        return createdLocation;
    }

    public async Task<Location> UpdateLocationAsync(Guid id, Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);
        existingLocation.Update(location);
        await _locationsRepository.SaveChangesAsync();

        return existingLocation;
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        var existingLocation = await _locationsRepository.GetAsync(id);
        _locationsRepository.Delete(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }
}