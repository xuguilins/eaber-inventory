using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class PurchaseOutOrderDetailConfiguration:ConfigurationBase<PurchaseOutOrderDetail>
{
    protected override void ConfigEntity(EntityTypeBuilder<PurchaseOutOrderDetail> builder)
    {
        builder.ToTable("PurchaseOutOrderDetail");
        builder.Property(x => x.ProductCode).HasMaxLength(50)
            .IsRequired();
        builder.Property(d => d.ProductModel)
            .HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.ProductName)
            .IsRequired(true).HasMaxLength(100);
        builder.Property(d => d.InCount)
            .IsRequired().HasMaxLength(50);
        builder.Property(d => d.OutCount)
            .IsRequired().HasMaxLength(50);
        builder.Property(d => d.InPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Property(d => d.OutPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Property(d => d.OutAllPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Ignore(d => d.PurchaseOutOrder);
        builder.HasOne(d => d.PurchaseOutOrder)
            .WithMany(f => f.PurashOutDetails)
            .HasForeignKey(f=>f.PurchaseId);
    }
}