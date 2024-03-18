using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class PurchaseInDetailConfiguration:ConfigurationBase<PurchaseInDetail>
{
    protected override void ConfigEntity(EntityTypeBuilder<PurchaseInDetail> builder)
    {
        builder.ToTable("PurchaseInDetail");
        builder.Property(x => x.ProductId)
            .HasMaxLength(50).IsRequired();
        builder.Property(x => x.ProductCode).HasMaxLength(50)
            .IsRequired();
        builder.Property(d => d.ProductModel)
            .HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.ProductName)
            .IsRequired(true).HasMaxLength(100);
        builder.Property(d => d.ProductCount)
            .IsRequired().HasMaxLength(50);
        builder.Property(d => d.ProductPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Property(d => d.ProductAll)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Ignore(d => d.PurchaseInOrder);

        builder.HasOne(d => d.PurchaseInOrder)
            .WithMany(f => f.PurchaseInDetails)
            .HasForeignKey("PurchaseInId");

    }
}