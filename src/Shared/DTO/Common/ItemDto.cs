namespace Shared;

public class ItemDto : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ItemDto(Guid? id, string? name, string? description) : base(id)
    {
        Name = name;
        Description = description;
    }
}