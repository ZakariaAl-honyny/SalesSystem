using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// استجابة أمر الشراء.
/// </summary>
public record PurchaseOrderDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("orderNo")] int OrderNo,
    [property: JsonPropertyName("orderDate")] DateTime OrderDate,
    [property: JsonPropertyName("supplierId")] int? SupplierId,
    [property: JsonPropertyName("supplierName")] string? SupplierName,
    [property: JsonPropertyName("warehouseId")] int? WarehouseId,
    [property: JsonPropertyName("warehouseName")] string? WarehouseName,
    [property: JsonPropertyName("subTotal")] decimal SubTotal,
    [property: JsonPropertyName("discountAmount")] decimal DiscountAmount,
    [property: JsonPropertyName("taxAmount")] decimal TaxAmount,
    [property: JsonPropertyName("totalAmount")] decimal TotalAmount,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("status")] byte Status,
    [property: JsonPropertyName("convertedToPurchaseInvoiceId")] int? ConvertedToPurchaseInvoiceId,
    [property: JsonPropertyName("items")] List<PurchaseOrderLineDto> Items
)
{
    /// <summary>نص الحالة بالعربية للعرض في الشبكة.</summary>
    [JsonIgnore]
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "مرسل",
        3 => "مستلم",
        4 => "مُحوَّل",
        5 => "ملغي",
        _ => "غير معروف"
    };
}

/// <summary>
/// استجابة بند أمر الشراء.
/// </summary>
public record PurchaseOrderLineDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("productId")] int ProductId,
    [property: JsonPropertyName("productName")] string? ProductName,
    [property: JsonPropertyName("quantity")] decimal Quantity,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
    [property: JsonPropertyName("lineTotal")] decimal LineTotal
);
