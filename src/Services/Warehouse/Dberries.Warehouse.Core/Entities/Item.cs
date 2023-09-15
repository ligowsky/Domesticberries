using Shared;

namespace Warehouse.Core;

public class Item
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ItemDto ToDto()
    {
        return new ItemDto
        {
            Id = Id,
            Name = Name,
            Description = Description
        };
    }

    public static Item? ToModel(ItemDto? dto)
    {
        if (dto is null) return null;

        return new Item
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}