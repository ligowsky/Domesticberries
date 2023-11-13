using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsRepository : IRepository
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item?> GetAsync(IFilterSet<Item> filterSet);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest);
    public Task AddAsync(Item item);
    public Task UpdateAsync(Item existingItem, Item item);
    public Task RemoveAsync(Item item);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
    public Task UpdateRatingAsync(IFilterSet<Item> itemFilterSet, Rating rating);
    public Task RemoveRatingAsync(IFilterSet<Item> filterSet, Guid userId);
}