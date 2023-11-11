namespace Dberries.Store;

public static class ItemMappingExtensions
{
    public static ItemDto ToDto(this Item model)
    {
        return new ItemDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            AverageRating = model.AverageRating,
        };
    }

    public static Item ToModel(this ItemDto dto)
    {
        return new Item
        {
            ExternalId = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}