using Shared;
using Warehouse.Core;

namespace Warehouse.Presentation;

public static class CoordinatesMappingExtension
{
    public static CoordinatesDto ToDto(this Coordinates model)
    {
        return new CoordinatesDto
        {
            Latitude = model.Latitude,
            Longitude = model.Longitude
        };
    }

    public static Coordinates ToModel(this CoordinatesDto dto)
    {
        return new Coordinates
        {
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }
}