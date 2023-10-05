namespace Dberries.Store;

public class Item
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public ICollection<Location>? Locations { get; set; }
}