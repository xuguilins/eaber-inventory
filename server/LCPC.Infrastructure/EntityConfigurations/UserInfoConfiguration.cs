using LCPC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class UserInfoConfiguration:ConfigurationBase<UserInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("UserInfo");
        builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.UserAddress).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.UserPass).HasMaxLength(50).IsRequired();
        builder.Property(x => x.UserTel).HasMaxLength(50).IsRequired(false);
    }
}