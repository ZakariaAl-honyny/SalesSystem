using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد الشاشة المكتبية لمحاضر الجرد الفعلي (المرحلة أ: تنفيذه الوحيد
/// <c>MockStocktakingDesktopService</c> — لا حفظ حقيقي قبل موافقة المالك — RULE-N165…N168).
/// <para>
/// المستند: رأس (مخزن · تاريخ · لجنة الجرد) + سطور (صنف · كمية دفترية · كمية مجرودة).
/// الترحيل يكتب <b>الفرق فقط</b> (المجرود − الدفتري) بإشارته: العجز يخفض الرصيد والفائض يرفعه.
/// </para>
/// <para>كل الطرق تُعيد <see cref="Result"/> — لا استثناءات تعبر هذا العقد.</para>
/// </summary>
public interface IStocktakingDesktopService
{
    /// <summary>كل المحاضر مرتبة بالأحدث تاريخاً (الملغاة تُستبعد ما لم يُطلب خلاف ذلك).</summary>
    Task<Result<IReadOnlyList<StocktakingDto>>> GetAllAsync(bool includeCancelled = false, CancellationToken ct = default);

    /// <summary>محضر واحد بكل سطوره.</summary>
    Task<Result<StocktakingDto>> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>حفظ محضر جديد كمسودة (بكل سطوره مرة واحدة) — لا يحرّك رصيداً.</summary>
    Task<Result<StocktakingDto>> CreateAsync(CreateStocktakingRequest request, CancellationToken ct = default);

    /// <summary>ترحيل مسودة: يطبّق فروق الجرد على الرصيد مرة واحدة.</summary>
    Task<Result<StocktakingDto>> PostAsync(int id, CancellationToken ct = default);

    /// <summary>إلغاء محضر <b>مرحّل</b> بعكس أثره (RULE-004: حالة إلغاء لا حذف).</summary>
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}

/// <summary>حالات محضر الجرد — تطابق <c>InvoiceStatus</c> (مسودة · مرحّل · ملغي).</summary>
public static class StocktakingStatus
{
    public const byte Draft = 1;
    public const byte Posted = 2;
    public const byte Cancelled = 3;
}

/// <summary>سطر محضر جرد فعلي: الكمية الدفترية مقابل الكمية المجرودة (والمجرودة قد تكون صفراً).</summary>
public sealed record StocktakingLineDto(
    int Id,
    int StocktakingId,
    int ProductId,
    string ProductName,
    string? Barcode,
    decimal BookQuantity,
    decimal CountedQuantity)
{
    /// <summary>الفرق = المجرود − الدفتري: موجب فائض · سالب عجز · صفر مطابق.</summary>
    public decimal Difference => CountedQuantity - BookQuantity;
}

/// <summary>محضر جرد فعلي كامل (رأس + سطور).</summary>
public sealed record StocktakingDto(
    int Id,
    string DocumentNo,
    int WarehouseId,
    string WarehouseName,
    DateTime Date,
    string? Notes,
    string? CommitteeHead,
    string? CommitteeMember,
    string? Storekeeper,
    byte Status,
    string StatusName,
    DateTime CreatedAt,
    DateTime? PostedAt,
    DateTime? CancelledAt,
    IReadOnlyList<StocktakingLineDto> Lines);

/// <summary>سطر طلب الحفظ — الكمية المجرودة فقط تُدخل من الشاشة (الدفتري يُقرأ من الرصيد).</summary>
public sealed record CreateStocktakingLineRequest(
    int ProductId,
    decimal BookQuantity,
    decimal CountedQuantity);

/// <summary>طلب حفظ محضر جرد — الرأس + كل السطور المعدودة مرة واحدة (لا عضو بلا مستهلك).</summary>
public sealed record CreateStocktakingRequest(
    int WarehouseId,
    DateTime Date,
    string? Notes = null,
    string? CommitteeHead = null,
    string? CommitteeMember = null,
    string? Storekeeper = null,
    IReadOnlyList<CreateStocktakingLineRequest>? Lines = null);
