using System.Net.Mime;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core;

namespace Warehouse.Persistence;

internal static class StockConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Stock> builder)
        where T : class
    {
        builder.Property(x => x.Item).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
    }
}