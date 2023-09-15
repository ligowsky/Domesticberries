namespace Warehouse.Core;

public class Location
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }
    public ICollection<Stock>? Stock { get; set; }
}