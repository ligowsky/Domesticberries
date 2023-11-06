using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsRepository : IRepository
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item?> GetByExternalIdAsync(Guid id);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest);
    public Task<Item> AddAsync(Item item);
    public Task<Item> UpdateAsync(Item item);
    public Task RemoveAsync(Item item);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
}