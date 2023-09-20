using Dberries.Warehouse.Core;

namespace Dberries.Warehouse.Presentation;

public static class LocationMappingExtensions
{
    public static LocationDto ToDto(this Location model)
    {
        return new LocationDto
        {
            Id = model.Id,
            Name = model.Name,
            Coordinates = model.Coordinates?.ToDto(),
            Stock = model.Stock?.Select(x => x.ToDto()).ToList()
        };
    }

    public static Location ToModel(this LocationDto dto)
    {
        return new Location
        {
            Id = dto.Id,
            Name = dto.Name,
            Coordinates = dto.Coordinates?.ToModel(),
            Stock = dto.Stock?.Select(x => x.ToModel()).ToList()
        };
    }
}