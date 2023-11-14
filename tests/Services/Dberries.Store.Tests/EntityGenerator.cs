namespace Dberries.Store.Tests;

public static class EntityGenerator
{
    public static Location GenerateLocation()
    {
        return new Location
        {
            ExternalId = Guid.NewGuid(),
            Name = $"Location {Guid.NewGuid()}",
        };
    }

    public static Item GenerateItem()
    {
        return new Item
        {
            ExternalId = Guid.NewGuid(),
            Name = $"Item {Guid.NewGuid()}",
            Description = $"Description {Guid.NewGuid()}"
        };
    }

    public static User GenerateUser()
    {
        return new User
        {
            ExternalId = Guid.NewGuid(),
            Email = $"{Guid.NewGuid()}@example.com",
        };
    }
}