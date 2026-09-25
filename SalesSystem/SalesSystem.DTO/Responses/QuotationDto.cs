using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// استجابة عرض السعر.
/// </summary>
public record QuotationDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("quotationNo")] int QuotationNo,
    [property: JsonPropertyName("quotationDate")] DateTime QuotationDate,
    [property: JsonPropertyName("validUntil")] DateTime? ValidUntil,
    [property: JsonPropertyName("customerId")] int? CustomerId,
    [property: JsonPropertyName("customerName")] string? CustomerName,
    [property: JsonPropertyName("warehouseId")] int? WarehouseId,
    [property: JsonPropertyName("warehouseName")] string? WarehouseName,
    [property: JsonPropertyName("subTotal")] decimal SubTotal,
    [property: JsonPropertyName("discountAmount")] decimal DiscountAmount,
    [property: JsonPropertyName("taxAmount")] decimal TaxAmount,
    [property: JsonPropertyName("totalAmount")] decimal TotalAmount,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("status")] byte Status,
    [property: JsonPropertyName("convertedToInvoiceId")] int? ConvertedToInvoiceId,
    [property: JsonPropertyName("items")] List<QuotationLineDto> Items
)
{
    /// <summary>نص الحالة بالعربية للعرض في الشبكة.</summary>
    [JsonIgnore]
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "مرسل",
        3 => "مقبول",
        4 => "مُحوَّل",
        5 => "مرفوض",
        _ => "غير معروف"
    };
}

/// <summary>
/// استجابة بند عرض السعر.
/// </summary>
public record QuotationLineDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("productId")] int ProductId,
    [property: JsonPropertyName("productName")] string? ProductName,
    [property: JsonPropertyName("quantity")] decimal Quantity,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
    [property: JsonPropertyName("discountAmount")] decimal DiscountAmount,
    [property: JsonPropertyName("lineTotal")] decimal LineTotal
);
