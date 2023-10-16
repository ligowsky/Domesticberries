namespace Dberries.Warehouse;

public class Item : IEntity
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
