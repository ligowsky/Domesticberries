using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Dberries.Store.Persistence;

public class ItemsRepository : RepositoryBase, IItemsRepository
{
    private readonly ElasticClient _elasticClient;

    public ItemsRepository(AppDbContext db, ElasticClient elasticClient) : base(db)
    {
        _elasticClient = elasticClient;
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await Db.Set<Item>()
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task<Item?> GetAsync(IFilterSet<Item> filter)
    {
        var item = await Db.Set<Item>()
            .Apply(filter)
            .FirstOrDefaultAsync();

        if (item is not null)
        {
            var averageRating = await Db.Set<Item>()
                .Apply(filter)
                .SelectMany(x => x.Ratings!)
                .Select(x => x.Value!.Value)
                .DefaultIfEmpty()
                .AverageAsync(x => x);

            item.AverageRating = (decimal?)Math.Round(averageRating, 2);
        }

        return item;
    }

    public async Task<Item?> GetByExternalIdAsync(Guid id)
    {
        return await Db.Set<Item>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest)
    {
        var searchResponse = await _elasticClient.SearchAsync<Item>(x => x
            .From(pageRequest.Offset!.Value)
            .Size(pageRequest.Limit!.Value)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(searchRequest.Q!)
                    .Fields(fs => fs
                        .Field(f => f.Name)
                        .Field(f => f.Description)
                    )
                )
            )
            .Source(s => s
                .Includes(i => i
                    .Field(f => f.Id)
                )
            )
        );

        var itemIds = searchResponse.Documents.Select(x => x.Id!.Value);

        return await Db.Set<Item>()
            .Where(item => itemIds.Contains(item.Id!.Value))
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task AddAsync(Item item)
    {
        Db.Add(item);
        await IndexAsync(item);
    }

    public async Task UpdateAsync(Item existingItem, Item item)
    {
        existingItem.Patch(item)
            .Property(x => x.Name)
            .Property(x => x.Description);

        await IndexAsync(existingItem);
    }

    public async Task RemoveAsync(Item item)
    {
        Db.Remove(item);
        await _elasticClient.DeleteAsync<Item>(item.Id!);
    }

    public async Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        await Db.ThrowIfNotExistsAsync<Item>(id);

        var availableInLocations = await Db.Set<Location>()
            .Where(x => x.Stock!
                .Any(y => y.ItemId == id))
            .OrderBy(x => x.Id)
            .Select(x => new ItemAvailabilityInLocation
            {
                LocationId = x.Id,
                LocationName = x.Name,
                Quantity = x.Stock!.First(y => y.ItemId == id).Quantity
            })
            .ToListAsync();

        return new ItemAvailabilityResponse(availableInLocations);
    }

    public async Task UpdateRatingAsync(IFilterSet<Item> filter, Rating input)
    {
        var item = await GetWithUserRatingAsync(filter, input.UserId!.Value);
        var rating = item.Ratings!.FirstOrDefault(x => x.UserId == input.UserId!.Value);

        if (rating is null)
        {
            item.Ratings!.Add(input);
        }
        else
        {
            rating.Value = input.Value;
        }
    }

    public async Task RemoveRatingAsync(IFilterSet<Item> filter, Guid userId)
    {
        var item = await GetWithUserRatingAsync(filter, userId);
        var rating = item.Ratings!.FirstOrDefault(x => x.UserId == userId);

        if (rating is not null)
            item.Ratings!.Remove(rating);
    }

    private async Task<Item> GetWithUserRatingAsync(IFilterSet<Item> filter, Guid userId)
    {
        var item = await Db.Set<Item>()
            .Apply(filter)
            .Include(x => x.Ratings!
                .Where(y => y.UserId == userId)
            )
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} is not found");

        return item;
    }

    private async Task IndexAsync(Item item)
    {
        await _elasticClient.IndexDocumentAsync(item);
    }
}