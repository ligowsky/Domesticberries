using BitzArt.Pagination;

namespace Dberries.Store;

public class ItemsRepository : RepositoryBase, IItemsRepository
{
    public ItemsRepository(AppDbContext db) : base(db)
    {
    }

    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        throw new NotImplementedException();
    }

    public Task<Item> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Item Add(Item item)
    {
        throw new NotImplementedException();
    }

    public void Remove(Item item)
    {
        throw new NotImplementedException();
    }
}