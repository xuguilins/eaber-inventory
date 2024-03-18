using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class SystemDicInfoConfiguration:ConfigurationBase<SystemDicInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<SystemDicInfo> builder)
    {
        builder.ToTable("SystemDicInfo");
        builder.Property(d => d.DicCode)
            .HasMaxLength(100).IsRequired();
        builder.Property(d => d.DicName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(d => d.DicType)
            .HasMaxLength(50)
            .IsRequired();
    }
}