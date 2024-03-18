using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations
{
    public class OrderInfoConfiguration : ConfigurationBase<OrderInfo>
    {
        protected override void ConfigEntity(EntityTypeBuilder<OrderInfo> builder)
        {
            builder.ToTable("OrderInfo");
            builder.Property(x => x.OrderClient).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OrderCode).HasMaxLength(100).IsRequired();
            builder.Property(x => x.OrderUser).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OrderPay).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.OrderMoney)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            builder.Property(x => x.ActuailMoney)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(x => x.OffsetMoney)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(d => d.OrderUserId)
                .HasMaxLength(100)
                .HasColumnName("OrderUserId");
            builder.Property(x => x.OrderStatus).HasMaxLength(10).IsRequired();
            builder.Property(x => x.OrderTel).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OrderTime).IsRequired(true).HasMaxLength(50);
            builder.HasMany(x => x.OrderInfoDetails)
            .WithOne(x => x.Order);
        }
    }
}