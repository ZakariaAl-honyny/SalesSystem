namespace SalesSystem.DTO;

public record DashboardSummaryDto(
    decimal TotalSalesToday,
    int NumberOfSalesToday,
    decimal TotalPurchasesToday,
    int LowStockItemsCount,
    int ActiveCustomersCount,
    int ActiveSuppliersCount,
    int TotalProductsCount,
    decimal TotalReceivables,
    decimal TotalPayables,
    decimal TotalSalesMonth,
    decimal TotalPurchasesMonth);
