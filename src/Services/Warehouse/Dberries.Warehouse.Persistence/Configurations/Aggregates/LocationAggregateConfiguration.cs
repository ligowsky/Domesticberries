using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dberries.Warehouse.Core;

namespace Dberries.Warehouse.Persistence;

public class LocationAggregateConfiguration : IEntityTypeConfiguration<Location>
{
    public virtual void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations", "Location");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.OwnsOne(x => x.Coordinates, coordinates => { coordinates.Configure(); })
            .Navigation(x => x.Coordinates)
            .IsRequired();
    
        builder.OwnsMany(x => x.Stock, stock =>
        {
            stock.ToTable("Stock", "Location");
            stock.HasKey("LocationId", "ItemId");
            stock.Property<Guid>("LocationId");
            stock.WithOwner().HasForeignKey("LocationId");
            stock.Configure();
        });
    }
}