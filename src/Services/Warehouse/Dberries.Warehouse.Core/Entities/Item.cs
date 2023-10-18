namespace Dberries.Warehouse;

public class Item : IEntity
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public Item(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public Item()
    {
    }
}