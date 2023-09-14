using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core;

namespace Warehouse.Persistence;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public virtual void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.Property(x => x.ItemId).IsRequired();
        
        builder.Property(x => x.LocationId).IsRequired();

        builder.Property(x => x.Quantity).IsRequired();
    }
}