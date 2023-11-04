using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item> AddAsync(Item item);
    public Task<Item> UpdateAsync(Item item);
    public Task RemoveAsync(Guid id);
    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, ISearchRequest searchRequest);
    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id);
    public Task UpdateRatingAsync(Guid id, byte value);
}