using Microsoft.AspNetCore.Builder;

namespace Dberries.Store.Persistence;

public static class AddPersistenceExtension
{
    public static void AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddMsSqlDbContext(builder.Configuration);

        builder.Services.AddRepositories();

        builder.AddElasticsearch(x =>
            x.ConfigureElasticMapping()
        );
    }
}