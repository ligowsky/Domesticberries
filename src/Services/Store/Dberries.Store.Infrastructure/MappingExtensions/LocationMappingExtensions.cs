namespace Dberries.Store;

public static class LocationMappingExtensions
{
    public static LocationDto ToDto(this Location model)
    {
        return new LocationDto
        {
            Id = model.Id,
            Name = model.Name
        };
    }

    public static Location ToModel(this LocationDto dto)
    {
        return new Location
        {
            ExternalId = dto.Id,
            Name = dto.Name
        };
    }
}