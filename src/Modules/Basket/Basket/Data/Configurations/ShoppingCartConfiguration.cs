using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.UserName)
            .IsUnique();

        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(100);

        // Additional configurations can be added here if needed

        builder.HasMany(e => e.Items)
            .WithOne()
            .HasForeignKey(e => e.ShoppingCartId);
            //.OnDelete(DeleteBehavior.Cascade);
    }
}
