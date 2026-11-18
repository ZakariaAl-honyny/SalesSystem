using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SalesSystem.Common;
using SalesSystem.Models;

namespace SalesSystem.DataAccess;

public class SalesDbContext : DbContext
{
    private readonly ICurrentUserProvider? _currentUserProvider;

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceLine> SalesInvoiceLines => Set<SalesInvoiceLine>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines => Set<PurchaseInvoiceLine>();
    public DbSet<SalesReturn> SalesReturns => Set<SalesReturn>();
    public DbSet<SalesReturnLine> SalesReturnLines => Set<SalesReturnLine>();
    public DbSet<PurchaseReturn> PurchaseReturns => Set<PurchaseReturn>();
    public DbSet<PurchaseReturnLine> PurchaseReturnLines => Set<PurchaseReturnLine>();
    public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();
    public DbSet<CustomerReceipt> CustomerReceipts => Set<CustomerReceipt>();
    public DbSet<CustomerReceiptApplication> CustomerReceiptApplications => Set<CustomerReceiptApplication>();
    public DbSet<SupplierPaymentApplication> SupplierPaymentApplications => Set<SupplierPaymentApplication>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<InventoryTransactionLine> InventoryTransactionLines => Set<InventoryTransactionLine>();
    public DbSet<DocumentSequence> DocumentSequences => Set<DocumentSequence>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();
    public DbSet<ProductUnit> ProductUnits => Set<ProductUnit>();
    public DbSet<CashBox> CashBoxes => Set<CashBox>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<InventoryBatch> InventoryBatches => Set<InventoryBatch>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<InventoryAdjustment> InventoryAdjustments => Set<InventoryAdjustment>();
    public DbSet<InventoryAdjustmentLine> InventoryAdjustmentLines => Set<InventoryAdjustmentLine>();

    public SalesDbContext(DbContextOptions<SalesDbContext> options, ICurrentUserProvider? currentUserProvider = null) : base(options)
    {
        _currentUserProvider = currentUserProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);

        // Convention: audit user-name snapshots → nvarchar(100) for every AuditableEntity derivative.
        // Single loop covers all ~25 entities that inherit CreatedByUserName / UpdatedByUserName.
        {
            var auditableBase = typeof(SalesSystem.Common.AuditableEntity);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!auditableBase.IsAssignableFrom(entityType.ClrType))
                    continue;

                entityType.FindProperty(nameof(SalesSystem.Common.AuditableEntity.CreatedByUserName))
                    ?.SetMaxLength(100);
                entityType.FindProperty(nameof(SalesSystem.Common.AuditableEntity.UpdatedByUserName))
                    ?.SetMaxLength(100);
            }
        }
    }

    public override int SaveChanges()
    {
        StampAuditUserNames();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        StampAuditUserNames();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampAuditUserNames();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        StampAuditUserNames();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Stamps CreatedByUserName / UpdatedByUserName from the acting user session,
    /// so no screen or service has to pass the name manually.
    /// Silent no-op when the provider is unavailable (design-time / migrations / tests)
    /// or when no user is logged in.
    /// </summary>
    private void StampAuditUserNames()
    {
        if (_currentUserProvider is null) return;

        var userName = _currentUserProvider.UserName?.Trim();
        if (string.IsNullOrWhiteSpace(userName)) return;

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    StampAdded(entry, userName);
                    break;

                case EntityState.Modified:
                    StampModified(entry, userName);
                    break;
            }
        }
    }

    private static void StampAdded(EntityEntry entry, string userName)
    {
        // AuditableEntity derivatives (~25 entities): set CreatedByUserName only if blank.
        if (entry.Entity is AuditableEntity auditable)
        {
            if (string.IsNullOrWhiteSpace(auditable.CreatedByUserName))
                auditable.SetCreatedBy(userName);
            // UpdatedByUserName is intentionally NOT set on insert — per requirement §2.
            return;
        }

        // Manual entities that don't inherit AuditableEntity but carry a UserName snapshot.
        switch (entry.Entity)
        {
            case AuditLog auditLog when string.IsNullOrWhiteSpace(auditLog.UserName):
                entry.Property(nameof(AuditLog.UserName)).CurrentValue = userName;
                break;

            case SystemLog systemLog when string.IsNullOrWhiteSpace(systemLog.UserName):
                entry.Property(nameof(SystemLog.UserName)).CurrentValue = userName;
                break;

            case InventoryAdjustment adjustment when string.IsNullOrWhiteSpace(adjustment.CreatedByUserName):
                entry.Property(nameof(InventoryAdjustment.CreatedByUserName)).CurrentValue = userName;
                break;

            case InventoryTransaction transaction when string.IsNullOrWhiteSpace(transaction.CreatedByUserName):
                entry.Property(nameof(InventoryTransaction.CreatedByUserName)).CurrentValue = userName;
                break;
        }
    }

    private static void StampModified(EntityEntry entry, string userName)
    {
        // AuditableEntity derivatives: always overwrite UpdatedByUserName with the current modifier.
        if (entry.Entity is AuditableEntity auditable)
        {
            auditable.SetUpdatedBy(userName);
            return;
        }

        // The four manual entities are write-once (no update path for UserName / CreatedByUserName).
        // If EF Core ever marks one as Modified, stamp only when blank to be safe.
        switch (entry.Entity)
        {
            case AuditLog auditLog when string.IsNullOrWhiteSpace(auditLog.UserName):
                entry.Property(nameof(AuditLog.UserName)).CurrentValue = userName;
                break;

            case SystemLog systemLog when string.IsNullOrWhiteSpace(systemLog.UserName):
                entry.Property(nameof(SystemLog.UserName)).CurrentValue = userName;
                break;

            case InventoryAdjustment adjustment when string.IsNullOrWhiteSpace(adjustment.CreatedByUserName):
                entry.Property(nameof(InventoryAdjustment.CreatedByUserName)).CurrentValue = userName;
                break;

            case InventoryTransaction transaction when string.IsNullOrWhiteSpace(transaction.CreatedByUserName):
                entry.Property(nameof(InventoryTransaction.CreatedByUserName)).CurrentValue = userName;
                break;
        }
    }

    public static string? GetConnectionString()
    {
        return Environment.GetEnvironmentVariable("SALESSYSTEM_DB_CONNECTION");
    }
}
