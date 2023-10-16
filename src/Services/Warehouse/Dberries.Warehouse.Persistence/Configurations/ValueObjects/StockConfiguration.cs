using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Warehouse.Persistence;

internal static class StockConfiguration
{
    public static void Configure<T>(this OwnedNavigationBuilder<T, Stock> builder)
        where T : class
    {
        builder.Property(x => x.ItemId).IsRequired();

        builder.Property(x => x.Quantity).IsRequired();

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId);
    }
}