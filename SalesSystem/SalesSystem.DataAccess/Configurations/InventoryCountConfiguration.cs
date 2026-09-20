using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Models;
using SalesSystem.Common.Enums;

namespace SalesSystem.DataAccess.Configurations;

public class InventoryCountConfiguration : IEntityTypeConfiguration<InventoryCount>
{
    public void Configure(EntityTypeBuilder<InventoryCount> builder)
    {
        builder.ToTable("InventoryCounts");
        builder.HasKey(ic => ic.Id);

        builder.Property(ic => ic.CountNo)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(ic => ic.CountNo)
            .IsUnique()
            .HasDatabaseName("IX_InventoryCounts_CountNo");

        builder.Property(ic => ic.Status)
            .HasConversion<byte>()
            .IsRequired()
            .HasDefaultValue(InventoryCountStatus.Draft)
            .HasSentinel((InventoryCountStatus)0);

        builder.Property(ic => ic.Notes)
            .HasMaxLength(300)
            .IsRequired(false);

        builder.Property(ic => ic.WarehouseId)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(ic => ic.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(ic => ic.CreatedByUserId)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(ic => ic.Warehouse)
            .WithMany()
            .HasForeignKey(ic => ic.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // FK to Branch
        builder.HasOne(ic => ic.Branch)
            .WithMany()
            .HasForeignKey(ic => ic.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ic => ic.Lines)
            .WithOne(l => l.InventoryCount)
            .HasForeignKey(l => l.InventoryCountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(ic => ic.Status != InventoryCountStatus.Cancelled);
    }
}
