using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة أوامر الشراء — CRUD عبر API.
/// </summary>
public interface IPurchaseOrderApiService
{
    Task<Result<IReadOnlyList<PurchaseOrderDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<PurchaseOrderDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<PurchaseOrderDto>> CreateAsync(PurchaseOrderDto request, CancellationToken ct = default);
    Task<Result<PurchaseOrderDto>> UpdateAsync(int id, PurchaseOrderDto request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
