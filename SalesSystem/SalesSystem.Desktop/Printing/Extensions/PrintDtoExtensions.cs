using SalesSystem.DTO;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Printing.Models;

namespace SalesSystem.Desktop.Printing.Extensions;

public static class PrintDtoExtensions
{
    public static StoreInfoPrintDto ToPrintDto(this StoreSettingsDto settings)
    {
        return new StoreInfoPrintDto
        {
            StoreName = settings.StoreName,
            Address = settings.Address ?? string.Empty,
            Phone = settings.Phone ?? string.Empty,
            TaxNumber = string.Empty,
            LogoPath = settings.LogoPath
        };
    }

    public static InvoicePrintDto ToPrintDto(this SalesInvoiceDto invoice)
    {
        return new InvoicePrintDto
        {
            InvoiceNumber = invoice.InvoiceNo.ToString(),
            InvoiceDate = invoice.InvoiceDate,
            TypeName = "فاتورة مبيعات",
            CashierName = string.Empty,
            CustomerOrSupplierName = invoice.CustomerName ?? "عميل نقدي",
            PaymentType = (PaymentType)invoice.PaymentType
        };
    }

    public static List<InvoiceItemPrintDto> ToPrintDtos(this IEnumerable<SalesInvoiceItemDto> items)
    {
        return items.Select(i => new InvoiceItemPrintDto
        {
            ProductCode = i.ProductCode ?? string.Empty,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Discount = i.DiscountAmount,
            LineTotal = i.LineTotal
        }).ToList();
    }

    public static List<InvoiceItemPrintDto> ToPrintDtos(this IEnumerable<SalesInvoiceLineDto> items,
        IReadOnlyDictionary<int, string>? unitNames = null)
    {
        return items.Select(i => new InvoiceItemPrintDto
        {
            ProductCode = string.Empty,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Discount = i.DiscountAmount,
            LineTotal = i.LineTotal,
            // Prefer the unit name the DTO already carries; otherwise fall back to a
            // caller-supplied lookup, and finally leave null so the summary prints "—".
            UnitName = !string.IsNullOrWhiteSpace(i.UnitName)
                ? i.UnitName
                : unitNames != null && unitNames.TryGetValue(i.ProductUnitId, out var unitName)
                    ? unitName
                    : null
        }).ToList();
    }

    public static InvoiceTotalsPrintDto ToTotalsPrintDto(this SalesInvoiceDto invoice)
    {
        return new InvoiceTotalsPrintDto
        {
            SubTotal = invoice.SubTotal,
            TaxAmount = invoice.TaxAmount,
            Discount = invoice.DiscountAmount,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            DueAmount = invoice.DueAmount,
            Tafqit = TryTafqit(invoice.TotalAmount)
        };
    }

    /// <summary>التفقيط — لا يُسمح أبداً برمي استثناء من مساعد التحويل إلى كلمات أثناء الطباعة.</summary>
    private static string? TryTafqit(decimal amount)
    {
        try
        {
            return global::SalesSystem.Desktop.Helpers.ArabicNumberToWords.ToWords(amount);
        }
        catch
        {
            return null;
        }
    }

    public static InvoicePrintDto ToPrintDto(this PurchaseInvoiceDto invoice)
    {
        return new InvoicePrintDto
        {
            InvoiceNumber = invoice.InvoiceNo.ToString(),
            InvoiceDate = invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue),
            TypeName = "فاتورة مشتريات",
            CashierName = string.Empty,
            CustomerOrSupplierName = invoice.SupplierName,
            PaymentType = (PaymentType)invoice.PaymentType
        };
    }

    public static List<InvoiceItemPrintDto> ToPrintDtos(this IEnumerable<PurchaseInvoiceItemDto> items)
    {
        return items.Select(i => new InvoiceItemPrintDto
        {
            ProductCode = i.ProductCode ?? string.Empty,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitCost,
            Discount = i.DiscountAmount,
            LineTotal = i.LineTotal
        }).ToList();
    }

    public static List<InvoiceItemPrintDto> ToPrintDtos(this IEnumerable<PurchaseInvoiceLineDto> items)
    {
        return items.Select(i => new InvoiceItemPrintDto
        {
            ProductCode = string.Empty,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitCost,
            Discount = i.DiscountAmount,
            LineTotal = i.LineTotal
        }).ToList();
    }

    public static InvoiceTotalsPrintDto ToTotalsPrintDto(this PurchaseInvoiceDto invoice)
    {
        return new InvoiceTotalsPrintDto
        {
            SubTotal = invoice.SubTotal,
            TaxAmount = invoice.TaxAmount,
            Discount = invoice.DiscountAmount,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            DueAmount = invoice.DueAmount,
            Tafqit = TryTafqit(invoice.TotalAmount)
        };
    }

    public static string ToArabicString(this PaymentType type) => type switch
    {
        PaymentType.Cash => "نقدي",
        PaymentType.Credit => "آجل",
        PaymentType.Mixed => "مختلط",
        _ => type.ToString()
    };
}
