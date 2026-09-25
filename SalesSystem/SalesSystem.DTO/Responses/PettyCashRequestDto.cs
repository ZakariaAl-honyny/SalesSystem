using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO لطلب العهدة النثرية (Petty Cash Request) — بيانات العرض في واجهة سطح المكتب.
/// <para>
/// القيم المحتملة للحالة (Status):
/// <list type="bullet">
///   <item><description>1 = قيد الانتظار (Pending)</description></item>
///   <item><description>2 = معتمدة (Approved)</description></item>
///   <item><description>3 = مصروفة (Disbursed)</description></item>
///   <item><description>4 = مُسوّاة (Settled)</description></item>
///   <item><description>5 = مرفوضة (Rejected)</description></item>
/// </list>
/// </para>
/// </summary>
public record PettyCashRequestDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("requestNo")] int RequestNo,
    [property: JsonPropertyName("date")] DateTime Date,
    [property: JsonPropertyName("employeeName")] string EmployeeName,
    [property: JsonPropertyName("employeeId")] int? EmployeeId,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("status")] byte Status,
    [property: JsonPropertyName("approvedBy")] string? ApprovedBy,
    [property: JsonPropertyName("disbursedDate")] DateTime? DisbursedDate,
    [property: JsonPropertyName("settlementDate")] DateTime? SettlementDate,
    [property: JsonPropertyName("settlementAmount")] decimal? SettlementAmount,
    [property: JsonPropertyName("refundAmount")] decimal? RefundAmount,
    [property: JsonPropertyName("notes")] string? Notes
)
{
    /// <summary>
    /// عرض نصي للحالة بالعربية.
    /// </summary>
    [JsonPropertyName("statusDisplay")]
    public string StatusDisplay => Status switch
    {
        1 => "قيد الانتظار",
        2 => "معتمدة",
        3 => "مصروفة",
        4 => "مُسوّاة",
        5 => "مرفوضة",
        _ => "غير معروفة"
    };
}
