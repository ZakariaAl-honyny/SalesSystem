using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة تقارير المبيعات — تُشتق كل الأرقام من مخازن بيانات التجربة فقط:
/// <c>MockData.Invoices</c> (الفواتير وسطورها)، <c>MockData.Customers</c> (أسماء العملاء)،
/// <c>MockData.Products</c> (أسماء وتصنيفات الأصناف)، <c>MockData.ProductUnits</c> (تكلفة الوحدة)
/// و<c>MockLookupsData.Categories</c> (احتياط لأسماء التصنيفات).
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </summary>
public class MockSalesReportApiService : ISalesReportApiService
{
    /// <summary>الفواتير المرحّلة فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    private const string DateRangeError = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية";
    private const string UnknownProduct = "غير معروف";
    private const string Uncategorized = "غير مصنف";

    // ═══════════════════════════════════════════════
    // 1) المبيعات حسب العميل
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<SalesByCustomerDto>>> GetSalesByCustomerAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<SalesByCustomerDto>>.Failure(DateRangeError));

        var invoices = PostedInvoices(from, to);

        var list = invoices
            .GroupBy(i => i.CustomerId)
            .Select(g => new SalesByCustomerDto(
                g.Key,
                CustomerName(g.Key, g.Select(i => i.CustomerName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                g.Count(),
                Money(g.Sum(i => i.NetTotal)),
                Money(g.Sum(i => i.PaidAmount)),
                Money(g.Sum(i => i.RemainingAmount))))
            .OrderByDescending(d => d.TotalAmount)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<SalesByCustomerDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 2) المبيعات حسب الصنف (مع التكلفة والربح)
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<SalesByProductDto>>> GetSalesByProductAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<SalesByProductDto>>.Failure(DateRangeError));

        var lines = PostedInvoices(from, to).SelectMany(i => i.Items);

        var list = lines
            .GroupBy(l => l.ProductId)
            .Select(g =>
            {
                var totalAmount = g.Sum(l => l.LineTotal);
                var totalCost = g.Sum(LineCost);
                var totalProfit = totalAmount - totalCost;
                var profitMargin = totalAmount > 0m ? Math.Round(totalProfit / totalAmount * 100m, 2) : 0m;

                return new SalesByProductDto(
                    g.Key,
                    ProductName(g.Key, g.Select(l => l.ProductName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                    Qty(g.Sum(l => l.Quantity)),
                    Money(totalAmount),
                    Money(totalCost),
                    Money(totalProfit),
                    profitMargin);
            })
            .OrderByDescending(d => d.TotalAmount)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<SalesByProductDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 3) المبيعات حسب التصنيف
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<SalesByCategoryDto>>> GetSalesByCategoryAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<SalesByCategoryDto>>.Failure(DateRangeError));

        var invoices = PostedInvoices(from, to);

        var rows = invoices
            .SelectMany(i => i.Items.Select(l => new { InvoiceId = i.Id, Line = l, Category = CategoryOf(l.ProductId) }))
            .ToList();

        var list = rows
            .GroupBy(r => r.Category.Id)
            .Select(g => new SalesByCategoryDto(
                g.Key,
                g.Select(r => r.Category.Name).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? Uncategorized,
                g.Select(r => r.InvoiceId).Distinct().Count(),
                Money(g.Sum(r => r.Line.LineTotal))))
            .OrderByDescending(d => d.TotalAmount)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<SalesByCategoryDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 4) الملخص اليومي للمبيعات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<DailySalesSummaryDto>>> GetDailySalesSummaryAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<DailySalesSummaryDto>>.Failure(DateRangeError));

        var invoices = PostedInvoices(from, to);

        var list = invoices
            .GroupBy(i => i.InvoiceDate.Date)
            .Select(g => new DailySalesSummaryDto(
                g.Key,
                g.Count(),
                Money(g.Sum(i => i.NetTotal)),
                Money(g.Sum(i => i.DiscountAmount)),
                Money(g.Sum(i => i.NetTotal - i.DiscountAmount))))
            .OrderBy(d => d.Date)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<DailySalesSummaryDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 5) ربحية الأصناف
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<ProductProfitabilityDto>>> GetProductProfitabilityAsync(
        int? productId = null, int? categoryId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<ProductProfitabilityDto>>.Failure(DateRangeError));

        var lines = PostedInvoices(
                from?.Date ?? DateTime.MinValue,
                to?.Date ?? DateTime.MaxValue)
            .SelectMany(i => i.Items)
            .Where(l => !productId.HasValue || l.ProductId == productId.Value)
            .Where(l => !categoryId.HasValue || CategoryOf(l.ProductId).Id == categoryId.Value);

        var list = lines
            .GroupBy(l => l.ProductId)
            .Select(g =>
            {
                var totalSales = g.Sum(l => l.LineTotal);
                var totalCogs = g.Sum(LineCost);
                var grossProfit = totalSales - totalCogs;
                var margin = totalSales > 0m ? Math.Round(grossProfit / totalSales * 100m, 2) : 0m;
                var category = CategoryOf(g.Key);

                return new ProductProfitabilityDto(
                    ProductName(g.Key, g.Select(l => l.ProductName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                    category.Name,
                    Qty(g.Sum(l => l.Quantity)),
                    Money(totalSales),
                    Money(totalCogs),
                    Money(grossProfit),
                    margin);
            })
            .OrderByDescending(d => d.GrossProfit)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<ProductProfitabilityDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // 6) الربحية حسب العميل
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<ProfitByCustomerDto>>> GetProfitByCustomerAsync(
        int? customerId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<ProfitByCustomerDto>>.Failure(DateRangeError));

        var invoices = PostedInvoices(
                from?.Date ?? DateTime.MinValue,
                to?.Date ?? DateTime.MaxValue)
            .Where(i => !customerId.HasValue || i.CustomerId == customerId.Value);

        var list = invoices
            .GroupBy(i => i.CustomerId)
            .Select(g =>
            {
                var totalSales = g.Sum(i => i.NetTotal);
                var totalCost = g.SelectMany(i => i.Items).Sum(LineCost);
                var grossProfit = totalSales - totalCost;
                var margin = totalSales > 0m ? Math.Round(grossProfit / totalSales * 100m, 2) : 0m;

                return new ProfitByCustomerDto(
                    CustomerName(g.Key, g.Select(i => i.CustomerName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))),
                    Money(totalSales),
                    Money(totalCost),
                    Money(grossProfit),
                    margin,
                    g.Count());
            })
            .OrderByDescending(d => d.GrossProfit)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<ProfitByCustomerDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // مخازن البيانات المساعدة
    // ═══════════════════════════════════════════════

    private static List<SalesInvoiceDto> PostedInvoices(DateTime? from, DateTime? to)
        => MockData.Invoices
            .Where(i => i.Status == Posted)
            .Where(i => !from.HasValue || i.InvoiceDate.Date >= from.Value.Date)
            .Where(i => !to.HasValue || i.InvoiceDate.Date <= to.Value.Date)
            .ToList();

    private static string CustomerName(int customerId, string? fallback)
    {
        var name = MockData.Customers.FirstOrDefault(c => c.Id == customerId)?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return !string.IsNullOrWhiteSpace(fallback) ? fallback! : "عميل نقدي";
    }

    private static string ProductName(int productId, string? fallback)
    {
        var name = MockData.Products.FirstOrDefault(p => p.Id == productId)?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return !string.IsNullOrWhiteSpace(fallback) ? fallback! : UnknownProduct;
    }

    /// <summary>تصنيف الصنف — من بيانات الصنف في مخزن التجربة، ثم من قائمة التصنيفات.</summary>
    private static (int Id, string Name) CategoryOf(int productId)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null)
            return (0, Uncategorized);

        if (!string.IsNullOrWhiteSpace(product.CategoryName))
            return (product.CategoryId, product.CategoryName!);

        var lookupName = MockLookupsData.Categories.FirstOrDefault(c => c.Id == product.CategoryId)?.Name;
        return (product.CategoryId, string.IsNullOrWhiteSpace(lookupName) ? Uncategorized : lookupName);
    }

    /// <summary>تكلفة سطر البيع = كمية السطر × تكلفة الوحدة من مخزن وحدات الأصناف.</summary>
    private static decimal LineCost(SalesInvoiceLineDto line)
    {
        var unitCost = line.UnitCost;

        if (unitCost <= 0m)
        {
            var unit = MockData.ProductUnits.FirstOrDefault(u => u.Id == line.ProductUnitId);
            if (unit is not null && unit.ProductId == line.ProductId)
                unitCost = unit.CostPrice;
            else
                unitCost = MockData.ProductUnits
                    .FirstOrDefault(u => u.ProductId == line.ProductId && u.IsBaseUnit)?.CostPrice ?? 0m;
        }

        return unitCost * line.Quantity;
    }

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    private static decimal Qty(decimal value) => Math.Round(value, 3, MidpointRounding.AwayFromZero);
}
