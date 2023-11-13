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

    public async Task<Item> GetAsync(IFilterSet<Item> filterSet)
    {
        var item = await _itemsRepository.GetAsync(filterSet);

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} is not found");

        return item;
    }

    public async Task<Item> AddAsync(IFilterSet<Item> filterSet, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(filterSet);

        if (existingItem is not null)
            return existingItem;

        await _itemsRepository.AddAsync(item);
        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(filterSet);
    }

    public async Task<Item> UpdateAsync(IFilterSet<Item> filterSet, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(filterSet);

        if (existingItem is null)
        {
            await _itemsRepository.AddAsync(item);
        }
        else
        {
            await _itemsRepository.UpdateAsync(existingItem, item);
        }

        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(filterSet);
    }

    public async Task RemoveAsync(IFilterSet<Item> filterSet)
    {
        var existingItem = await _itemsRepository.GetAsync(filterSet);

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

    public async Task<Item> UpdateRatingAsync(IFilterSet<Item> itemFilterSet, IFilterSet<User> userFilterSet,
        byte value)
    {
        var user = await _usersService.GetAsync(userFilterSet);
        var rating = new Rating(user.Id!.Value, value);

        await _itemsRepository.UpdateRatingAsync(itemFilterSet, rating);
        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(itemFilterSet);
    }

    public async Task<Item> RemoveRatingAsync(IFilterSet<Item> filterSet, IFilterSet<User> userFilterSet)
    {
        var user = await _usersService.GetAsync(userFilterSet);

        await _itemsRepository.RemoveRatingAsync(filterSet, user.Id!.Value);
        await _itemsRepository.SaveChangesAsync();

        return await GetAsync(filterSet);
    }
}