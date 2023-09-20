using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dberries.Warehouse.Core;

namespace Dberries.Warehouse.Persistence;

internal static class StockConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Stock> builder)
        where T : class
    {
        builder.Property<Guid>("ItemId");
        builder.Property(x => x.Quantity).IsRequired();
        builder.HasOne(x => x.Item).WithMany().HasForeignKey("ItemId");
    }
}