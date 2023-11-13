using BitzArt;

namespace Dberries.Store;

public class ItemFilterSet : IFilterSet<Item>
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }

    public IQueryable<Item> Apply(IQueryable<Item> query)
    {
        return query
            .AddFilter(x => x.Id, Id)
            .AddFilter(x => x.ExternalId, ExternalId);
    }
}