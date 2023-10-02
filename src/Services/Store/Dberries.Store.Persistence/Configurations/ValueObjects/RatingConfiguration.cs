using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Store.Persistence;

internal static class RatingConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Rating> builder)
        where T : class
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Value).IsRequired();
    }
}