using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface ILocationsRepository : IRepositoryBase
{
    public Task<PageResult<Location>> GetPageAsync(PageRequest pageRequest);
    public Task<Location> GetAsync(Guid id);
    public Task<Location> CreateAsync(Location location);
    public void Delete(Location location);
    public Task<bool> Exists(Guid id);
}