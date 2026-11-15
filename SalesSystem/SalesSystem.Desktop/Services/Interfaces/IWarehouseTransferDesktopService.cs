using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد الشاشة المكتبية لسندات التحويل المخزني (المرحلة أ: تنفيذه الوحيد
/// <c>MockWarehouseTransferDesktopService</c> — لا حفظ حقيقي قبل موافقة المالك — RULE-N165…N168).
/// <para>
/// المستند: رأس (مخزن مُرسل · مخزن مُستلم · تاريخ) + سطور (صنف · كمية).
/// الترحيل يخفض رصيد المصدر ويرفع رصيد الهدف، ويُشترط أن يكفي رصيد المصدر.
/// </para>
/// <para>كل الطرق تُعيد <see cref="Result"/> — لا استثناءات تعبر هذا العقد.</para>
/// </summary>
public interface IWarehouseTransferDesktopService
{
    /// <summary>كل السندات مرتبة بالأحدث تاريخاً (الملغاة تُستبعد ما لم يُطلب خلاف ذلك).</summary>
    Task<Result<IReadOnlyList<WarehouseTransferDto>>> GetAllAsync(bool includeCancelled = false, CancellationToken ct = default);

    /// <summary>سند واحد بكل سطوره.</summary>
    Task<Result<WarehouseTransferDto>> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>حفظ سند جديد كمسودة (بكل سطوره مرة واحدة) — لا يحرّك رصيداً.</summary>
    Task<Result<WarehouseTransferDto>> CreateAsync(CreateWarehouseTransferRequest request, CancellationToken ct = default);

    /// <summary>ترحيل مسودة: خصم من المصدر وإضافة إلى الهدف مرة واحدة (بعد التحقق من كفاية المصدر).</summary>
    Task<Result<WarehouseTransferDto>> PostAsync(int id, CancellationToken ct = default);

    /// <summary>إلغاء سند <b>مرحّل</b> بعكس أثره (RULE-004: حالة إلغاء لا حذف).</summary>
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}

/// <summary>حالات سند التحويل — تطابق <c>InvoiceStatus</c> (مسودة · مرحّل · ملغي).</summary>
public static class WarehouseTransferStatus
{
    public const byte Draft = 1;
    public const byte Posted = 2;
    public const byte Cancelled = 3;
}

/// <summary>سطر سند تحويل: صنف + كمية (ورقم الدفعة/الانتهاء إن أدخلهما المستخدم).</summary>
public sealed record WarehouseTransferLineDto(
    int Id,
    int WarehouseTransferId,
    int ProductId,
    string ProductName,
    string? Barcode,
    string? BatchNo,
    DateOnly? ExpiryDate,
    decimal Quantity);

/// <summary>سند تحويل مخزني كامل (رأس + سطور).</summary>
public sealed record WarehouseTransferDto(
    int Id,
    string DocumentNo,
    int FromWarehouseId,
    string FromWarehouseName,
    int ToWarehouseId,
    string ToWarehouseName,
    DateTime Date,
    string? Notes,
    byte Status,
    string StatusName,
    DateTime CreatedAt,
    DateTime? PostedAt,
    DateTime? CancelledAt,
    IReadOnlyList<WarehouseTransferLineDto> Lines);

/// <summary>سطر طلب الحفظ — كمية موجبة فقط (السطور ذات الكمية صفر لا تُرسل).</summary>
public sealed record CreateWarehouseTransferLineRequest(
    int ProductId,
    decimal Quantity,
    string? BatchNo = null,
    DateOnly? ExpiryDate = null);

/// <summary>طلب حفظ سند تحويل — الرأس + كل السطور مرة واحدة (لا عضو بلا مستهلك).</summary>
public sealed record CreateWarehouseTransferRequest(
    int FromWarehouseId,
    int ToWarehouseId,
    DateTime Date,
    string? Notes = null,
    IReadOnlyList<CreateWarehouseTransferLineRequest>? Lines = null);
