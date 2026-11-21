using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        // ─── PK ────────────────────────────────────────────────
        builder.HasKey(p => p.Id);

        // ─── String properties ──────────────────────────────
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.ImagePath).HasMaxLength(500);
        builder.Property(p => p.ForeignName).HasMaxLength(200);
        builder.Property(p => p.LocationInStore).HasMaxLength(100);


        // ─── Numeric properties ─────────────────────────────
        builder.Property(p => p.ReorderLevel).HasPrecision(18, 3).HasDefaultValue(0m);
        builder.Property(p => p.MinSalePrice).HasPrecision(18, 2).HasDefaultValue(0m);
        builder.Property(p => p.IsActive).HasDefaultValue(true);

        // ─── Relationships ─────────────────────────────────────
        builder.HasOne(p => p.ProductCategory)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Units — dynamic UOM collection
        builder.HasMany(p => p.Units)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Inventory batches for FIFO/FEFO costing
        builder.HasMany(p => p.InventoryBatches)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // WarehouseStocks
        builder.HasMany(p => p.WarehouseStocks)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // ─── Global query filter ───────────────────────────────
        // فلتر الاستعلام العام — الحذف الناعم. يجب على شاشات الإدارة استخدام IgnoreQueryFilters() لعرض السجلات المحذوفة
        builder.HasQueryFilter(p => p.IsActive);
    }
}
