using BitzArt.Pagination;

namespace Dberries.Store.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;

    public ItemsService(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await _itemsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(Guid id)
    {
        return await _itemsRepository.GetByExternalIdAsync(id);
    }

    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, string query)
    {
        throw new NotImplementedException();
    }

    public Task<PageResult<Location>> GetAvailabilityAsync(PageRequest pageRequest, Guid id)
    {
        return _itemsRepository.GetAvailabilityAsync(pageRequest, id);
    }

    public Task UpdateRatingAsync(Guid id, byte value)
    {
        throw new NotImplementedException();
    }
}