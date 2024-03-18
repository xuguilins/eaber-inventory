using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class PurchaseInOrderConfiguration:ConfigurationBase<PurchaseInOrder>
{
    protected override void ConfigEntity(EntityTypeBuilder<PurchaseInOrder> builder)
    {
        builder.ToTable("PurchaseInOrder");
        builder.Property(x => x.InOrderTime)
            .HasMaxLength(50).IsRequired();

        builder.Property(x => x.Logistics)
            .HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.ChannelType)
            .HasMaxLength(10).IsRequired();
        builder.Property(x => x.InUser)
            .HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.InPhone)
            .HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.SupplierId)
            .HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.InCount)
            .HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.InPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18,2)
            .IsRequired(true);
        //builder.Property(d=>d.in)
        builder.Property(x => x.InOStatus)
            .IsRequired(true).HasMaxLength(10);
        builder.Ignore(x => x.SupplierInfo);
        builder.HasOne(d => d.SupplierInfo)
            .WithMany()
            .HasForeignKey(v => v.SupplierId);
        builder.Ignore(d => d.PurchaseInDetails);
        builder.HasMany(d => d.PurchaseInDetails)
            .WithOne(m => m.PurchaseInOrder)
            .HasForeignKey(d => d.PurchaseInId);
        builder.Property(x => x.PurchaseCode)
            .HasMaxLength(50)
            .IsRequired();
    }
}