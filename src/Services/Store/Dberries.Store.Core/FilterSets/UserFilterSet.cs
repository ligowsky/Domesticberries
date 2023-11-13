using BitzArt;

namespace Dberries.Store;

public class UserFilterSet : IFilterSet<User>
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }

    public IQueryable<User> Apply(IQueryable<User> query)
    {
        return query
            .AddFilter(x => x.Id, Id)
            .AddFilter(x => x.ExternalId, ExternalId);
    }
}