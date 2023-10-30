using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsRepository : IEntityRepository
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item?> GetByExternalIdAsync(Guid id, bool trowException);
    public Item AddAsync(Item item);
    public void Remove(Item item);
    public Task<PageResult<Location>> GetAvailabilityAsync(PageRequest pageRequest, Guid id);
}