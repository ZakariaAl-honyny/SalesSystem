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

        builder.Property(u => u.RoleId).IsRequired().HasColumnType("smallint");
        builder.Property(u => u.BranchId).IsRequired(false).HasColumnType("smallint");

        builder.Property(u => u.EmployeeId).IsRequired(false);
        builder.Property(u => u.IsLocked).IsRequired().HasDefaultValue(false);
        builder.Property(u => u.MustChangePassword).IsRequired().HasDefaultValue(true);
        builder.Property(u => u.LoginAttempts).IsRequired().HasDefaultValue((short)0);
        builder.Property(u => u.LastLoginAt).IsRequired(false);

        builder.HasQueryFilter(u => u.IsActive);

        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
