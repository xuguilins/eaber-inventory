using LCPC.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public class CateInfoConfiguration:ConfigurationBase<CateInfo>
{
    protected override void ConfigEntity(EntityTypeBuilder<CateInfo> builder)
    {
        builder.ToTable("CateInfo");
        builder.Property(x => x.CateName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ParentId).HasColumnName("ParentId")
        .HasMaxLength(50).IsRequired(false);
    }
}