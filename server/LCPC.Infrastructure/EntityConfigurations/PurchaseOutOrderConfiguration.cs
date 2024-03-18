using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class PurchaseOutOrderConfiguration:ConfigurationBase<PurchaseOutOrder>
{
    protected override void ConfigEntity(EntityTypeBuilder<PurchaseOutOrder> builder)
    {
        builder.ToTable("PurchaseOutOrder");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Logicse).HasMaxLength(100);
        builder.Property(d => d.InPhone).HasMaxLength(100).IsRequired();
        builder.Property(d => d.InUser).HasMaxLength(50).IsRequired();
        builder.Property(d => d.SupilerId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.OutOrderCount)
            .HasMaxLength(50).IsRequired();
        builder.Property(d => d.OutOrderPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired();
        builder.Ignore(d => d.SupplierInfo);
        builder.Ignore(d => d.PurashOutDetails);
        builder.Property(d => d.InOrderCode).HasMaxLength(100).IsRequired();
        builder.Property(d => d.PurchaseCode).HasMaxLength(100).IsRequired();

        builder.HasOne(d => d.SupplierInfo)
            .WithMany()
            .HasForeignKey(f => f.SupilerId);
        builder.Property(d => d.OutStatus).HasMaxLength(10).IsRequired();
    }
}