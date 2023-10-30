using BitzArt;

namespace Dberries.Store.Infrastructure;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;

    public LocationsService(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }

    public async Task<Location> AddAsync(Location location)
    {
        await _locationsRepository.AddAsync(location);
        await _locationsRepository.SaveChangesAsync();

        return location;
    }

    public async Task UpdateAsync(Location location)
    {
        var existingLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value, false);

        if (existingLocation is null)
        {
            await _locationsRepository.AddAsync(location);
        }
        else
        {
            existingLocation.Patch(location)
                .Property(x => x.Name);
        }

        await _locationsRepository.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingLocation = await _locationsRepository.GetAsync(id, false);

        if (existingLocation is null) return;

        _locationsRepository.Remove(existingLocation);
        await _locationsRepository.SaveChangesAsync();
    }
    
    public async Task UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        await _locationsRepository.UpdateStockAsync(locationId, itemId, quantity);
        await _locationsRepository.SaveChangesAsync();
    }
}