using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core;

namespace Warehouse.Persistence;

internal static class CoordinatesConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Coordinates> builder)
        where T : class
    {
        builder.WithOwner();
        builder.Property(x => x.Latitude).IsRequired();
        builder.Property(x => x.Longitude).IsRequired();
    }
}