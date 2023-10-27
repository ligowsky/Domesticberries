namespace Dberries.Store;

public class Location : IEntity, IExternalId
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }
    public string? Name { get; set; }
    public ICollection<Stock>? Stock { get; set; }
    
    public Location(string? name)
    {
        Name = name;
    }

    public Location()
    {
    }
}