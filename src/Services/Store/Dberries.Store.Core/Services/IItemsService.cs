using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(IFilterSet<Item> filter);
    public Task<Item> AddAsync(Item item);
    public Task<Item> UpdateAsync(Guid id, Item item);
    public Task RemoveAsync(Guid id);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
    public Task<Item> UpdateRatingAsync(Guid itemId, Guid userId, byte value);
    public Task<Item> RemoveRatingAsync(Guid itemId, Guid userId);
}