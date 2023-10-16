using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Warehouse.Persistence;

internal static class CoordinatesConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Coordinates> builder)
        where T : class
    {
        builder.Property(x => x.Latitude)
            .HasPrecision(7, 5)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .HasPrecision(7, 5)
            .IsRequired();
    }
}