using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(IFilterSet<Item> filterSet);
    public Task<Item> AddAsync(IFilterSet<Item> filterSet, Item item);
    public Task<Item> UpdateAsync(IFilterSet<Item> filterSet, Item item);
    public Task RemoveAsync(IFilterSet<Item> filterSet);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
    public Task<Item> UpdateRatingAsync(IFilterSet<Item> itemFilterSet, IFilterSet<User> userFilterSet, byte value);
    public Task<Item> RemoveRatingAsync(IFilterSet<Item> itemFilterSet, IFilterSet<User> userFilterSet);
}