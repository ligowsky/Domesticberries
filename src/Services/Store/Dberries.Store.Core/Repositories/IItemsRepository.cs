using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsRepository : IRepository
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item?> GetByExternalIdAsync(Guid id);
    public Task<Item> AddAsync(Item item);
    public void Remove(Item item);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
}