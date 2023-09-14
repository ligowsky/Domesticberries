using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core;

namespace Warehouse.Persistence;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public virtual void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description).IsRequired();
    }
}