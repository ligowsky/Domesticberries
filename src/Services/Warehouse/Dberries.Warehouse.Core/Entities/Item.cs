using Shared;

namespace Warehouse.Core;

public class Item : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public Item(Guid? id, string? name, string? description) : base(id)
    {
        Name = name;
        Description = description;
    }
}