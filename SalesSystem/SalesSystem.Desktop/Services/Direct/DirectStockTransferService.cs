using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة التحويلات المخزنية — يترجم بين DTO الخاصة بالواجهة
/// (StockTransferDto) و DTO الخاصة بطبقة الأعمال (WarehouseTransferDto).
/// </summary>
public class DirectStockTransferService : IStockTransferApiService
{
    private readonly BizInterfaces.IWarehouseTransferService _bll;
    private readonly ISessionService _session;

    public DirectStockTransferService(BizInterfaces.IWarehouseTransferService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<StockTransferDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(null, null, 1, int.MaxValue, ct);
            if (!result.IsSuccess)
                return Result<IReadOnlyList<StockTransferDto>>.Failure(result.Error!, result.ErrorCode);

            IEnumerable<StockTransferDto> query = result.Value!.Items.Select(MapToDesktop);

            // خدمة الأعمال لا تدعم البحث أو تصفية التاريخ — تُطبَّق هنا في الذاكرة.
            if (from.HasValue)
                query = query.Where(t => t.TransferDate >= from.Value.Date);

            if (to.HasValue)
            {
                var toEnd = to.Value.Date.AddDays(1).AddSeconds(-1);
                query = query.Where(t => t.TransferDate <= toEnd);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(t =>
                    t.TransferNo.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (t.Notes ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            var list = query.OrderByDescending(t => t.TransferDate)
                            .ThenByDescending(t => t.Id)
                            .ToList();

            return Result<IReadOnlyList<StockTransferDto>>.Success(list);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<StockTransferDto>>.Failure("تعذر تحميل التحويلات المخزنية. حاول مرة أخرى.");
        }
    }

    public async Task<Result<StockTransferDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetByIdAsync(id, ct);
            return result.IsSuccess
                ? Result<StockTransferDto>.Success(MapToDesktop(result.Value!))
                : Result<StockTransferDto>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<StockTransferDto>.Failure("تعذر تحميل التحويل المخزني. حاول مرة أخرى.");
        }
    }

    public async Task<Result<StockTransferDto>> CreateAsync(CreateStockTransferRequest r, CancellationToken ct = default)
    {
        try
        {
            var request = new CreateWarehouseTransferRequest(
                TransferNo: null,
                SourceWarehouseId: (short)r.FromWarehouseId,
                DestinationWarehouseId: (short)r.ToWarehouseId,
                Notes: r.Notes,
                Lines: r.Items?.Select(i => new CreateWarehouseTransferLineRequest(
                    ProductUnitId: i.ProductId, // TODO: طلب الواجهة يحمل ProductId — تُمرَّر كـ ProductUnitId حتى تُوحَّد العقود
                    Quantity: i.Quantity,
                    BatchNo: null)).ToList(),
                BranchId: r.BranchId);

            var result = await _bll.CreateAsync(request, UserId, ct);
            return result.IsSuccess
                ? Result<StockTransferDto>.Success(MapToDesktop(result.Value!))
                : Result<StockTransferDto>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<StockTransferDto>.Failure("تعذر إنشاء التحويل المخزني. حاول مرة أخرى.");
        }
    }

    public async Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.PostAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر ترحيل التحويل المخزني. حاول مرة أخرى.");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.CancelAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر إلغاء التحويل المخزني. حاول مرة أخرى.");
        }
    }

    // ─── Mapping: Business DTO → Desktop DTO ───────────────────────────────

    private static StockTransferDto MapToDesktop(WarehouseTransferDto src) => new(
        Id: src.Id,
        TransferNo: src.TransferNo,
        TransferDate: src.CreatedAt, // TODO: خدمة الأعمال لا تعيد تاريخ التحويل — يُستخدم تاريخ الإنشاء
        FromWarehouseId: src.SourceWarehouseId,
        ToWarehouseId: src.DestinationWarehouseId,
        Notes: src.Notes,
        Status: src.Status,
        Items: src.Lines is null
            ? new List<StockTransferItemDto>()
            : src.Lines.Select(l => new StockTransferItemDto(
                ProductId: l.ProductUnitId, // TODO: سطر الأعمال يعيد ProductUnitId فقط — لا يوجد ProductId
                ProductCode: null,
                ProductName: l.ProductUnitName ?? string.Empty,
                Quantity: l.Quantity)).ToList());
}
