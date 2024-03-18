using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class CustomerInfoConfiguration:ConfigurationBase<CustomerInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<CustomerInfo> builder)
    {
        builder.ToTable("CustomerInfo");

        builder.Property(d => d.CustomerCode).IsRequired()
            .HasMaxLength(100);
        builder.Property(d => d.CustomerName)
            .IsRequired().HasMaxLength(100);
        builder.Property(d => d.Address)
            .HasMaxLength(100);
        builder.Property(d => d.TelNumber)
            .HasMaxLength(100);
        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(100);
        builder.Property(d => d.Remark)
            .HasMaxLength(100);
        builder.Property(d => d.NameSpell)
            .HasMaxLength(200);
    }
}