using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dberries.Warehouse.Core;

namespace Dberries.Warehouse.Persistence;

internal static class CoordinatesConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Coordinates> builder)
        where T : class
    {
        builder.Property(x => x.Latitude).IsRequired();
        builder.Property(x => x.Longitude).IsRequired();
    }
}