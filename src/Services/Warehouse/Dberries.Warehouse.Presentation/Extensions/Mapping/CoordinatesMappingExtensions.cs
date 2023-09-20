using Dberries.Warehouse.Core;

namespace Dberries;

public static class CoordinatesMappingExtensions
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