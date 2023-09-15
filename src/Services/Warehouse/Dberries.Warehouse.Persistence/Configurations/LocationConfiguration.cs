using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core;

namespace Warehouse.Persistence;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public virtual void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations", "Location");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Coordinates).IsRequired();

        builder.OwnsMany(x => x.Stock, stock =>
        {
            stock.ToTable("Stock", "Location");
            stock.Configure();
        });
    }
}