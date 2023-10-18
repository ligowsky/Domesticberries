namespace Dberries.Tests.Shared;

public static class EntityGenerator
{
    public static Item GenerateItem(int number = 1)
    {
        return new Item
        {
            Name = $"Item {number}",
            Description = $"Description {number}"
        };
    }

    private Stock GenerateStock(int quantity = 1)
    {
        return new Stock
        {
            Quantity = quantity
        };
    }
}