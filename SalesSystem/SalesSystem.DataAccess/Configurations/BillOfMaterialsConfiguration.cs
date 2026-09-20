using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Configurations;

/// <summary>
/// EF Core configuration for the BillOfMaterials entity.
/// Maps to the "BillOfMaterials" table with proper column types and relationships.
/// </summary>
public class BillOfMaterialsConfiguration : IEntityTypeConfiguration<BillOfMaterials>
{
    public void Configure(EntityTypeBuilder<BillOfMaterials> builder)
    {
        builder.ToTable("BillOfMaterials");

        // ─── PK ────────────────────────────────────────────────
        builder.HasKey(x => x.Id);

        // ─── Properties ─────────────────────────────────────────
        builder.Property(x => x.FinishedProductId)
            .IsRequired();

        builder.Property(x => x.ComponentProductId)
            .IsRequired();

        builder.Property(x => x.QuantityRequired)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(x => x.UnitId)
            .HasColumnType("smallint")
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasColumnType("nvarchar")
            .HasMaxLength(500)
            .IsRequired(false);

        // ─── Relationships ──────────────────────────────────────
        // BillOfMaterials → FinishedProduct: Many-to-One, NO cascade delete
        builder.HasOne(x => x.FinishedProduct)
            .WithMany()
            .HasForeignKey(x => x.FinishedProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // BillOfMaterials → ComponentProduct: Many-to-One, NO cascade delete
        builder.HasOne(x => x.ComponentProduct)
            .WithMany()
            .HasForeignKey(x => x.ComponentProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // BillOfMaterials → Unit: Many-to-One, NO cascade delete
        builder.HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // ─── Indexes ────────────────────────────────────────────
        // Unique index on (FinishedProductId, ComponentProductId) prevents duplicate BOM entries
        builder.HasIndex(x => new { x.FinishedProductId, x.ComponentProductId })
            .IsUnique()
            .HasDatabaseName("IX_BillOfMaterials_FinishedProductId_ComponentProductId");

        // ─── Global query filter (soft-delete) ──────────────────
        builder.HasQueryFilter(x => x.IsActive);
    }
}
