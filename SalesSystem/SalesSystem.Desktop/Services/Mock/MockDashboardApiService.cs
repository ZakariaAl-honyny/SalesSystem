using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة لوحة المعلومات — تُشتق أرقامها من بيانات <c>MockData</c>
/// و<c>MockPurchasesData</c> و<c>MockInventoryData</c> الحقيقية للتجربة،
/// وتهبط إلى قيم ثابتة غير صفرية لأي مؤشر بلا بيانات (RULE-N68).
/// لا تلمس قاعدة البيانات ولا ترمي استثناءً أبداً.
/// </summary>
public class MockDashboardApiService : IDashboardApiService
{
    public Task<Result<DashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default)
    {
        try
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);

            // ═══════════════════════════════════════════════
            // فواتير البيع — تُستثنى الملغاة (Status = 3)
            // ═══════════════════════════════════════════════
            var sales = MockData.Invoices.Where(i => i.Status != 3).ToList();

            var salesToday = sales.Where(i => i.InvoiceDate.Date == today).ToList();
            decimal totalSalesToday = salesToday.Sum(i => i.NetTotal);
            int numberOfSalesToday = salesToday.Count;

            decimal totalSalesMonth = sales
                .Where(i => i.InvoiceDate.Date >= monthStart && i.InvoiceDate.Date <= today)
                .Sum(i => i.NetTotal);

            // احتياط: بيانات التجربة قد لا تقع في اليوم/الشهر الحالي — نعرض أقرب تاريخ متاح
            if (numberOfSalesToday == 0 && sales.Count > 0)
            {
                var latestSalesDate = sales.Max(i => i.InvoiceDate.Date);
                numberOfSalesToday = sales.Count(i => i.InvoiceDate.Date == latestSalesDate);
            }

            if (totalSalesToday == 0m && sales.Count > 0)
                totalSalesToday = sales.OrderByDescending(i => i.InvoiceDate).First().NetTotal;

            if (totalSalesMonth == 0m && sales.Count > 0)
                totalSalesMonth = sales.Sum(i => i.NetTotal);

            // ═══════════════════════════════════════════════
            // فواتير المشتريات — تُستثنى الملغاة
            // ═══════════════════════════════════════════════
            var purchases = MockPurchasesData.Purchases.Where(p => p.Status != 3).ToList();

            decimal totalPurchasesToday = purchases
                .Where(p => p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date == today)
                .Sum(p => p.NetTotal);

            decimal totalPurchasesMonth = purchases
                .Where(p => p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date >= monthStart)
                .Sum(p => p.NetTotal);

            // تواريخ المشتريات في بيانات التجربة ثابتة — نضمن ألا تبدو البطاقة صفراً
            if (totalPurchasesToday == 0m && purchases.Count > 0)
                totalPurchasesToday = purchases.OrderByDescending(p => p.InvoiceDate).First().NetTotal;

            if (totalPurchasesMonth == 0m && purchases.Count > 0)
                totalPurchasesMonth = purchases.Sum(p => p.NetTotal);

            // ═══════════════════════════════════════════════
            // المخزون المنخفض
            // ═══════════════════════════════════════════════
            int lowStock = MockData.Products.Count(p => p.IsLowStock || p.IsOutOfStock);

            if (lowStock == 0)
                lowStock = MockInventoryData.Batches.Count(b => b.QuantityRemaining <= 10m);

            if (lowStock == 0)
                lowStock = 3;   // قيمة ثابتة: اللوحة لا تبدو معطّلة في وضع التجربة

            // ═══════════════════════════════════════════════
            // الأرصدة والعدادات
            // ═══════════════════════════════════════════════
            decimal totalReceivables = sales.Sum(i => i.RemainingAmount);
            decimal totalPayables = purchases.Sum(p => p.RemainingAmount);

            int activeCustomers = MockData.Customers.Count(c => c.IsActive);
            int activeSuppliers = MockSuppliersData.Suppliers.Count(s => s.IsActive);
            int totalProducts = MockData.Products.Count(p => p.IsActive);

            var summary = new DashboardSummaryDto(
                TotalSalesToday: totalSalesToday,
                NumberOfSalesToday: numberOfSalesToday,
                TotalPurchasesToday: totalPurchasesToday,
                LowStockItemsCount: lowStock,
                ActiveCustomersCount: activeCustomers,
                ActiveSuppliersCount: activeSuppliers,
                TotalProductsCount: totalProducts,
                TotalReceivables: totalReceivables,
                TotalPayables: totalPayables,
                TotalSalesMonth: totalSalesMonth,
                TotalPurchasesMonth: totalPurchasesMonth);

            return Task.FromResult(Result<DashboardSummaryDto>.Success(summary));
        }
        catch
        {
            // وضع التجربة لا ينكسر: أرقام احتياطية مستقرة
            var fallback = new DashboardSummaryDto(
                TotalSalesToday: 237500m,
                NumberOfSalesToday: 2,
                TotalPurchasesToday: 52500m,
                LowStockItemsCount: 3,
                ActiveCustomersCount: 6,
                ActiveSuppliersCount: 4,
                TotalProductsCount: 10,
                TotalReceivables: 395550m,
                TotalPayables: 153700m,
                TotalSalesMonth: 795050m,
                TotalPurchasesMonth: 227700m);

            return Task.FromResult(Result<DashboardSummaryDto>.Success(fallback));
        }
    }
}
