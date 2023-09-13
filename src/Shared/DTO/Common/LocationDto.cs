namespace Shared;

public class LocationDto : BaseEntity
{
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }

    public LocationDto(Guid? id, string? name, Coordinates? coordinates) : base(id)
    {
        Name = name;
        Coordinates = coordinates;
    }
}