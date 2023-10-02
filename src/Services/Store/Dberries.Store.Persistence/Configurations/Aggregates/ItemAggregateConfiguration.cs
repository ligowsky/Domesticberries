using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dberries.Store.Persistence;

public class ItemAggregateConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items", "Item");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description).IsRequired();

        builder.OwnsMany(x => x.Ratings, ratings =>
        {
            ratings.ToTable("Rating", "Item");
            ratings.WithOwner().HasForeignKey("ItemId");
            ratings.Configure();
        });
    }
}