using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class ProductInfoConfiguration:ConfigurationBase<ProductInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<ProductInfo> builder)
    {
        builder.ToTable("ProductInfo");
        builder.Property(x => x.ProductCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProductModel).HasMaxLength(100).IsRequired(false); 
        builder.Property(x => x.CateId).HasColumnName("CateId").HasMaxLength(50).IsRequired();
        builder.Property(x => x.UnitId).HasColumnName("UnitId").HasMaxLength(50).IsRequired();
         builder.Property(x => x.SupilerId).HasColumnName("SupilerId").HasMaxLength(50).IsRequired();
        builder.Property(x => x.ConversionRate).HasColumnName("ConversionRate").IsRequired(false)
            .HasMaxLength(50);
        builder.Property(x => x.InitialCost).HasColumnType("decimal").HasPrecision(18,2).IsRequired();
        builder.Property(x => x.Purchase).HasColumnType("decimal").HasPrecision(18,2).IsRequired();
        builder.Property(x => x.SellPrice).HasColumnType("decimal").HasPrecision(18,2).IsRequired();
        builder.Property(x => x.Wholesale).HasColumnType("decimal").HasPrecision(18,2).IsRequired();
        builder.Property(x => x.InventoryCount).HasColumnName("InventoryCount").IsRequired().HasMaxLength(32);
        builder.Property(x => x.MaxStock).HasColumnName("MaxStock").IsRequired().HasMaxLength(32);
        builder.Property(x => x.MinStock).HasColumnName("MinStock").IsRequired().HasMaxLength(32);
        builder.Property(d => d.NameSpell)
            .HasColumnName("NameSpell")
            .HasMaxLength(200);
        builder.Ignore(x => x.Cate);
     
        builder.Ignore(x => x.Supplier);
        builder.HasOne(x => x.Cate)
            .WithMany(m => m.ProductInfos);
            builder.HasOne(x => x.Supplier)
                .WithMany(m => m.ProductInfos);

    }
}
