namespace Dberries.Warehouse;

public class Location : IEntity
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }
    public ICollection<Stock>? Stock { get; set; }

    public void Update(Location input)
    {
        Name = input.Name ?? Name;
        Coordinates = input.Coordinates ?? Coordinates;
    }
}