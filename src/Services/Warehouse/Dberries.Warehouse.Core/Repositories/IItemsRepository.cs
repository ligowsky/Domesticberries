namespace Dberries.Warehouse;

public interface IItemsRepository : IRepositoryBase
{
    public Task<List<Item>> GetAllAsync();
    public Task<Item> GetAsync(Guid? id);
    public Task<Item> CreateAsync(Item item);
    public void Delete(Item item);
}