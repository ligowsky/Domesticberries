namespace Dberries.Store.Tests;

public static class EntityGenerator
{
    public static Location GenerateLocation(int number = 1)
    {
        return new Location
        {
            ExternalId = Guid.NewGuid(),
            Name = $"Location {number}",
        };
    }

    public static Item GenerateItem(int number = 1)
    {
        return new Item
        {
            ExternalId = Guid.NewGuid(),
            Name = $"Item {number}",
            Description = $"Description {number}"
        };
    }

    public static Stock GenerateStock(int quantity = 1)
    {
        return new Stock
        {
            Quantity = quantity
        };
    }
}