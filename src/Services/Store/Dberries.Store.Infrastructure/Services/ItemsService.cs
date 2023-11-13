using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;
    private readonly IUsersService _usersService;

    public ItemsService(IItemsRepository itemsRepository, IUsersService usersService)
    {
        _itemsRepository = itemsRepository;
        _usersService = usersService;
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await _itemsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(IFilterSet<Item> filter)
    {
        var item = await _itemsRepository.GetAsync(filter);

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} is not found");

        return item;
    }

    public async Task<Item> AddAsync(IFilterSet<Item> filter, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(filter);

        if (existingItem is not null)
            throw ApiException.Conflict($"{nameof(Item)} already exists");

        await _itemsRepository.AddAsync(item);
        await _itemsRepository.SaveChangesAsync();

        return item;
    }

    public async Task<Item> UpdateAsync(IFilterSet<Item> filter, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(filter);

        if (existingItem is null)
        {
            await _itemsRepository.AddAsync(item);
            await _itemsRepository.SaveChangesAsync();
            return item;
        }

        await _itemsRepository.UpdateAsync(existingItem, item);
        await _itemsRepository.SaveChangesAsync();

        return existingItem;
    }

    public async Task RemoveAsync(IFilterSet<Item> filter)
    {
        var existingItem = await _itemsRepository.GetAsync(filter);

        if (existingItem is null) return;

        await _itemsRepository.RemoveAsync(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }

    public async Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest)
    {
        return await _itemsRepository.SearchAsync(pageRequest, searchRequest);
    }

    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        return _itemsRepository.GetAvailabilityAsync(id);
    }

    public async Task<Item> UpdateRatingAsync(IFilterSet<Item> itemFilter, IFilterSet<User> userFilter, byte value)
    {
        var user = await _usersService.GetAsync(userFilter);
        var rating = new Rating(user.Id!.Value, value);

        await _itemsRepository.UpdateRatingAsync(itemFilter, rating);
        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(itemFilter);
    }

    public async Task<Item> RemoveRatingAsync(IFilterSet<Item> filter, IFilterSet<User> userFilter)
    {
        var user = await _usersService.GetAsync(userFilter);

        await _itemsRepository.RemoveRatingAsync(filter, user.Id!.Value);
        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(filter);
    }
}