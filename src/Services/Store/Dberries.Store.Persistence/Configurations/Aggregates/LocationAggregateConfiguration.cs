using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Store.Persistence;

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

        builder.OwnsMany(x => x.Stock, stock =>
        {
            stock.ToTable("Stock", "Location");
            stock.HasKey("LocationId", "ItemId");
            stock.Property<Guid>("LocationId");
            stock.WithOwner().HasForeignKey("LocationId");
            stock.Configure();
        }).Navigation(x => x.Stock).AutoInclude(false);
    }
}