using Dberries.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Warehouse.Persistence;

public class LocationAggregateConfiguration : IEntityTypeConfiguration<Location>
{
    public virtual void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations", "Location");

        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.ExternalId)
            .IsUnique()
            .IsDescending();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasMany(x => x.Items)
            .WithMany(x => x.Locations)
            .UsingEntity(x => x.ToTable("LocationItem"));
    }
}