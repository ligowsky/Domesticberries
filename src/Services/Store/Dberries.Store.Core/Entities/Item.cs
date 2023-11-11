namespace Dberries.Store;

public class Item : IEntityWithExternalId<Guid>
{
    public Guid? Id { get; set; }
    public Guid? ExternalId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Rating>? Ratings { get; set; }

    public decimal? AverageRating => Math.Round((decimal)Ratings?.Average(x => x.Value)!, 2);

    public Item(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public Item()
    {
    }
}