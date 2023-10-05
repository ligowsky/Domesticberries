namespace Dberries.Store;

public class Location
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Item>? Items { get; set; }
}