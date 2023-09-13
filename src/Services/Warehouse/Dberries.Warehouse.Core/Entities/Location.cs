using Shared;

namespace Warehouse.Core;

public class Location : BaseEntity
{
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }

    public Location(Guid? id, string? name, Coordinates? coordinates) : base(id)
    {
        Name = name;
        Coordinates = coordinates;
    }
}