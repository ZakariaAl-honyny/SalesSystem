using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO for a treasury transfer (تحويل خزينة) — moves cash between cashboxes/banks.
/// Transfer numbers start at 3001 and increment sequentially.
/// </summary>
public record TreasuryTransferDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("transferNo")] int TransferNo,
    [property: JsonPropertyName("transferDate")] DateTime TransferDate,
    [property: JsonPropertyName("fromCashBoxId")] int? FromCashBoxId,
    [property: JsonPropertyName("fromCashBoxName")] string? FromCashBoxName,
    [property: JsonPropertyName("toCashBoxId")] int? ToCashBoxId,
    [property: JsonPropertyName("toCashBoxName")] string? ToCashBoxName,
    [property: JsonPropertyName("fromAccountCode")] string? FromAccountCode,
    [property: JsonPropertyName("toAccountCode")] string? ToAccountCode,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("status")] byte Status
)
{
    /// <summary>عرض نصي للحالة بالعربية.</summary>
    [JsonPropertyName("statusDisplay")]
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "مرحل",
        3 => "ملغي",
        _ => ""
    };
}
