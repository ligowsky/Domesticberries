namespace Shared;

public class Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public CoordinatesDto ToDto()
    {
        return new CoordinatesDto
        {
            Latitude = Latitude,
            Longitude = Longitude
        };
    }

    public static Coordinates? ToModel(CoordinatesDto? dto)
    {
        if (dto is null) return null;

        return new Coordinates
        {
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }
}