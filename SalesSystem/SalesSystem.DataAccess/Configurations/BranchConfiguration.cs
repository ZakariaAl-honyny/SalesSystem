using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).UseIdentityColumn();
        builder.Property(b => b.Name).IsRequired().HasMaxLength(150);
        builder.Property(b => b.Address).HasMaxLength(500);
        builder.Property(b => b.Phone).HasMaxLength(30);
        builder.Property(b => b.IsActive).HasDefaultValue(true);

        builder.HasQueryFilter(b => b.IsActive);

        builder.HasIndex(b => b.Name).IsUnique();
    }
}
