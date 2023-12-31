using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Persistence;

public static class AddMsSqlDbContextExtension
{
    public static void AddMsSqlDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MsSqlDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("StoreMsSql"),
                x => { x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); }));

        services.AddScoped<AppDbContext>(x => x.GetRequiredService<MsSqlDbContext>());
    }
}