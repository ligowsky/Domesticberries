using Dberries.Store.Core;

namespace Dberries;

public static class LocationMappingExtensions
{
    public static LocationDto ToDto(this Location model)
    {
        return new LocationDto
        {
            Id = model.Id,
            Name = model.Name,
            Items = model.Items?.Select(x => x.ToDto()).ToList()
        };
    }

    public static Location ToModel(this LocationDto dto)
    {
        return new Location
        {
            Id = dto.Id,
            Name = dto.Name,
            Items = dto.Items?.Select(x => x.ToModel()).ToList()
        };
    }
}