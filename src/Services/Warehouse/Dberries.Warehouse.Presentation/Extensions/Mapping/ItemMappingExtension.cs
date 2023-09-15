using Shared;
using Warehouse.Core;

namespace Warehouse.Presentation;

public static class ItemMappingExtension
{
    public static ItemDto ToDto(this Item model)
    {
        return new ItemDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description
        };
    }

    public static Item ToModel(this ItemDto dto)
    {
        return new Item
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}