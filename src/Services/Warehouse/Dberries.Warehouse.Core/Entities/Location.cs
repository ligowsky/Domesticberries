using Shared;

namespace Warehouse.Core;

public class Location
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Coordinates? Coordinates { get; set; }
    public ICollection<Stock>? Stock { get; set; }

    public LocationDto ToDto()
    {
        return new LocationDto
        {
            Id = Id,
            Name = Name,
            Coordinates = Coordinates?.ToDto(),
            Stock = Stock?.Select(x => x.ToDto()).ToList()
        };
    }

    public static Location? ToModel(LocationDto? dto)
    {
        if (dto is null) return null;

        return new Location
        {
            Id = dto.Id,
            Name = dto.Name,
            Coordinates = Coordinates.ToModel(dto.Coordinates),
            Stock = dto.Stock?.Select(Core.Stock.ToModel).ToList()
        };
    }
}