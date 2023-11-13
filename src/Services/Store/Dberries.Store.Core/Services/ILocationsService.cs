using BitzArt;

namespace Dberries.Store;

public interface ILocationsService
{
    public Task<Location> AddAsync(IFilterSet<Location> filter, Location location);
    public Task<Location> UpdateAsync(IFilterSet<Location> filter, Location location);
    public Task RemoveAsync(IFilterSet<Location> filter);
    public Task UpdateStockAsync(IFilterSet<Location> locationFilter, IFilterSet<Item> itemFilter,
        int quantity);
    public Task RemoveStockAsync(IFilterSet<Location> locationFilter, IFilterSet<Item> itemFilter);
}