namespace Dberries.Warehouse.Tests;

public static class EntityGenerator
{
    public static Location GenerateLocation(int number = 1)
    {
        return new Location
        {
            Name = $"Location {number}",
            Coordinates = new Coordinates
            {
                Latitude = 1,
                Longitude = 1
            }
        };
    }

    public static Item GenerateItem(int number = 1)
    {
        return new Item
        {
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