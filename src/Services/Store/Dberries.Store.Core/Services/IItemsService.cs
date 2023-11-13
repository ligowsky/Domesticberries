using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(IFilterSet<Item> filter);
    public Task<Item> AddAsync(IFilterSet<Item> filter, Item item);
    public Task<Item> UpdateAsync(IFilterSet<Item> filter, Item item);
    public Task RemoveAsync(IFilterSet<Item> filter);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
    public Task<Item> UpdateRatingAsync(IFilterSet<Item> itemFilter, IFilterSet<User> userFilter, byte value);
    public Task<Item> RemoveRatingAsync(IFilterSet<Item> itemFilter, IFilterSet<User> userFilter);
}