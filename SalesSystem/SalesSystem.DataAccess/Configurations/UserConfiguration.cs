using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);

        builder.Property(u => u.BranchId).IsRequired(false).HasColumnType("smallint");
        builder.HasOne(u => u.Branch)
            .WithMany()
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.EmployeeId).IsRequired(false);
        builder.Property(u => u.IsLocked).IsRequired().HasDefaultValue(false);
        builder.Property(u => u.MustChangePassword).IsRequired().HasDefaultValue(true);
        builder.Property(u => u.LoginAttempts).IsRequired().HasDefaultValue((short)0);
        builder.Property(u => u.LastLoginAt).IsRequired(false);

        // فلتر الاستعلام العام — الحذف الناعم. يجب على شاشات الإدارة استخدام IgnoreQueryFilters() لعرض السجلات المحذوفة
        builder.HasQueryFilter(u => u.IsActive);
    }
}
