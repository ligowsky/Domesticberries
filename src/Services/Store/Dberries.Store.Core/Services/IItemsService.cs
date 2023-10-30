using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task AddAsync(Item item);
    public Task UpdateAsync(Item item);
    public Task RemoveAsync(Guid id);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, string query);
    public Task<PageResult<Location>> GetAvailabilityAsync(PageRequest pageRequest, Guid id);
    public Task UpdateRatingAsync(Guid id, byte value);
}