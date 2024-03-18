using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class ExtraOrderConfiguration:ConfigurationBase<ExtraOrder>
{
    protected override void ConfigEntity(EntityTypeBuilder<ExtraOrder> builder)
    {
        builder.ToTable("ExtraOrder");
        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        builder.Property(d => d.ExtraType)
            .HasMaxLength(10).IsRequired();
        builder.Property(d => d.TypeName).IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.OrderCode)
            .HasMaxLength(100)
            .IsRequired();

    }
}