using BitzArt;

namespace Dberries.Store;

public class LocationFilterSet : IFilterSet<Location>
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }

    public IQueryable<Location> Apply(IQueryable<Location> query)
    {
        return query
            .AddFilter(x => x.Id, Id)
            .AddFilter(x => x.ExternalId, ExternalId);
    }
}