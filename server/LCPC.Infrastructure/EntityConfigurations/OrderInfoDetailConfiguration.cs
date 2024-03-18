using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations
{
    public class OrderInfoDetailConfiguration : ConfigurationBase<OrderInfoDetail>
    {
        protected override void ConfigEntity(EntityTypeBuilder<OrderInfoDetail> builder)
        {
            builder.ToTable("OrderInfoDetail");
            builder.Property(d=>d.ProductId).IsRequired()
            .HasMaxLength(100);
            builder.Property(x=>x.ProductName)
            .HasMaxLength(100)
            .IsRequired();
            builder.Property(d=>d.ProductCode)
            .HasMaxLength(100)
            .IsRequired();
            builder.Property(x=>x.OrderCount)
            .HasMaxLength(50)
            .IsRequired();
            builder.Property(x=>x.OrderPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            builder.Property(x=>x.OrderSigle)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            builder.Property(x => x.UnitName)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasOne(c=>c.Order)
            .WithMany(v=>v.OrderInfoDetails);
        }
    }
}