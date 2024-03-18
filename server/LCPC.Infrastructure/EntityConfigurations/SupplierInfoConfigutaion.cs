using LCPC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class SupplierInfoConfigutaion : ConfigurationBase<SupplierInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<SupplierInfo> builder)
    {
        builder.ToTable("SupplierInfo");

        builder.Property(x => x.Address)
            .HasMaxLength(200);
        builder.Property(x => x.ProviderUser).HasMaxLength(100);
        builder.Property(x => x.SupTel).HasMaxLength(100);
        builder.Property(x => x.SupTelT).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.SupPhone).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.SupPhoneT).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.SupName).HasMaxLength(100);
        builder.Property(x => x.ProviderUserT).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.SupNumber).HasMaxLength(100);
    }
}