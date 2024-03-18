using LCPC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCPC.Infrastructure.EntityConfigurations;

public abstract class ConfigurationBase<TEntity>
:IEntityTypeConfiguration<TEntity>
where TEntity:EntityBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreateTime).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreateUser).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Enable).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Remark).HasMaxLength(200)
            .IsRequired(false);
        ConfigEntity(builder);
    }

    protected abstract void ConfigEntity(EntityTypeBuilder<TEntity> builder);
}