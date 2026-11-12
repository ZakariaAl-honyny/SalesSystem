using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة تقارير المشتريات — تُشتق كل الأرقام من مخازن بيانات التجربة فقط:
/// <c>MockPurchasesData.Purchases</c> (الفواتير وسطورها)، <c>MockSuppliersData.Suppliers</c>
/// (أسماء الموردين) و<c>MockData.Products</c> (أسماء الأصناف).
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </summary>
public class MockPurchaseReportApiService : IPurchaseReportApiService
{
    /// <summary>الفواتير المرحّلة فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    private const string DateRangeError = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية";
    private const string UnknownProduct = "غير معروف";
    private const string UnknownSupplier = "مورد";

    // ═══════════════════════════════════════════════
    // 1) المشتريات حسب المورد
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<PurchasesBySupplierDto>>> GetPurchasesBySupplierAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<PurchasesBySupplierDto>>.Failure(DateRangeError));

        var invoices = PostedPurchases(from, to);

        var list = invoices
            .GroupBy(i => i.SupplierId)
            .Select(g => new PurchasesBySupplierDto(
                g.Key,
                SupplierName(g.Key, g.Select(i => i.SupplierName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                g.Count(),
                Money(g.Sum(i => i.NetTotal)),
                Money(g.Sum(i => i.PaidAmount)),
                Money(g.Sum(i => i.RemainingAmount))))
            .OrderByDescending(d => d.TotalAmount)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<PurchasesBySupplierDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 2) المشتريات حسب الصنف
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<PurchasesByProductDto>>> GetPurchasesByProductAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<PurchasesByProductDto>>.Failure(DateRangeError));

        var lines = PostedPurchases(from, to).SelectMany(i => i.Items);

        var list = lines
            .GroupBy(l => l.ProductId)
            .Select(g => new PurchasesByProductDto(
                g.Key,
                ProductName(g.Key, g.Select(l => l.ProductName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                Qty(g.Sum(l => l.Quantity)),
                Money(g.Sum(l => l.LineTotal))))
            .OrderByDescending(d => d.TotalCost)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<PurchasesByProductDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // مخازن البيانات المساعدة
    // ═══════════════════════════════════════════════

    private static List<PurchaseInvoiceDto> PostedPurchases(DateTime from, DateTime to)
    {
        var fromDate = DateOnly.FromDateTime(from.Date);
        var toDate = DateOnly.FromDateTime(to.Date);

        return MockPurchasesData.Purchases
            .Where(i => i.Status == Posted)
            .Where(i => i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate)
            .ToList();
    }

    private static string SupplierName(int supplierId, string? fallback)
    {
        var name = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == supplierId)?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return !string.IsNullOrWhiteSpace(fallback) ? fallback! : UnknownSupplier;
    }

    private static string ProductName(int productId, string? fallback)
    {
        var name = MockData.Products.FirstOrDefault(p => p.Id == productId)?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return !string.IsNullOrWhiteSpace(fallback) ? fallback! : UnknownProduct;
    }

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    private static decimal Qty(decimal value) => Math.Round(value, 3, MidpointRounding.AwayFromZero);
}
