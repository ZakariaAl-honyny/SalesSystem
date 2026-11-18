using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        // Supplier.Id is auto-increment PK
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        // Direct contact fields (replaces Party relationship)
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.Property(s => s.Email)
            .HasMaxLength(100);

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.Property(s => s.CreditLimit)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        // CategoryId is a lookup — no FK constraint (type mismatch with smallint PK)
        builder.Property(s => s.CategoryId).IsRequired(false);

        // Indexes
        builder.HasIndex(s => s.CategoryId)
            .HasDatabaseName("IX_Suppliers_CategoryId");

        builder.HasIndex(s => s.Name)
            .HasDatabaseName("IX_Suppliers_Name");

        builder.HasIndex(s => s.Phone)
            .HasDatabaseName("IX_Suppliers_Phone");

        // فلتر الاستعلام العام — الحذف الناعم. يجب على شاشات الإدارة استخدام IgnoreQueryFilters() لعرض السجلات المحذوفة
        builder.HasQueryFilter(s => s.IsActive);
    }
}
