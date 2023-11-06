using Nest;

namespace Dberries.Store.Persistence;

public static class ConfigureElasticMappingExtension
{
    public static void ConfigureElasticMapping(this ConnectionSettings settings)
    {
        settings.DefaultMappingFor<Item>(mapping => mapping
            .IndexName("items")
            .IdProperty(p => p.Id!)
            .Ignore(p => p.ExternalId)
        );
    }
}