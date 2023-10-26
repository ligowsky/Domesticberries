namespace Dberries.Store;

public class Location
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }
    public string? Name { get; set; }
    public ICollection<Item>? Items { get; set; }
}