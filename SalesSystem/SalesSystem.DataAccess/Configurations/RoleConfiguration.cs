using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnType("smallint");

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(r => r.Name).IsUnique();
        builder.Property(r => r.Description).IsRequired(false).HasMaxLength(300);

        builder.Property(r => r.IsSystem).IsRequired().HasDefaultValue(false);

        builder.Property(r => r.SalesPermissions).IsRequired().HasDefaultValue(0L);
        builder.Property(r => r.InventoryPermissions).IsRequired().HasDefaultValue(0L);
        builder.Property(r => r.AccountingPermissions).IsRequired().HasDefaultValue(0L);
        builder.Property(r => r.SystemPermissions).IsRequired().HasDefaultValue(0L);

        builder.HasQueryFilter(r => r.IsActive);
    }
}
