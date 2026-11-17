using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class CashBoxConfiguration : IEntityTypeConfiguration<CashBox>
{
    public void Configure(EntityTypeBuilder<CashBox> builder)
    {
        builder.ToTable("CashBoxes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(300);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        // Global query filter for soft delete
        // فلتر الاستعلام العام — الحذف الناعم. يجب على شاشات الإدارة استخدام IgnoreQueryFilters() لعرض السجلات المحذوفة
        builder.HasQueryFilter(x => x.IsActive);
    }
}
