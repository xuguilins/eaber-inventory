using LCPC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class RuleInfoConfiguration:ConfigurationBase<RuleInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<RuleInfo> builder)
    {
        builder.ToTable("RuleInfo");
        builder.Property(x => x.Formatter).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IdentityNum).IsRequired().HasMaxLength(10);
        builder.Property(x => x.RulePix).IsRequired().HasMaxLength(20);
        builder.Property(x => x.NowValue).IsRequired().HasMaxLength(50);
        builder.Property(x => x.RuleAppend).IsRequired().HasMaxLength(50);
        builder.Property(x => x.RuleName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.RuleType).IsRequired().HasMaxLength(10);
    }
}