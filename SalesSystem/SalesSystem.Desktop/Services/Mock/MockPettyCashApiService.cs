using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة العهد النثرية — تعيد بيانات من MockPettyCashData.
/// </summary>
public class MockPettyCashApiService : IPettyCashApiService
{
    public Task<Result<IReadOnlyList<PettyCashRequestDto>>> GetAllAsync(
        bool includeInactive = false,
        CancellationToken ct = default)
    {
        IEnumerable<PettyCashRequestDto> query = MockPettyCashData.Requests;

        if (!includeInactive)
            query = query.Where(r => r.Status != 5); // استبعاد المرفوضة

        var result = query.OrderByDescending(r => r.Date).ThenByDescending(r => r.Id).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<PettyCashRequestDto>>.Success(result));
    }

    public Task<Result<PettyCashRequestDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var request = MockPettyCashData.Requests.FirstOrDefault(r => r.Id == id);
        return request is null
            ? Task.FromResult(Result<PettyCashRequestDto>.Failure("طلب العهدة غير موجود"))
            : Task.FromResult(Result<PettyCashRequestDto>.Success(request));
    }

    public Task<Result<PettyCashRequestDto>> CreateAsync(PettyCashRequestDto request, CancellationToken ct = default)
    {
        var nextId = MockPettyCashData.Requests.Count > 0
            ? MockPettyCashData.Requests.Max(r => r.Id) + 1
            : 1;

        var nextRequestNo = MockPettyCashData.Requests.Count > 0
            ? MockPettyCashData.Requests.Max(r => r.RequestNo) + 1
            : 1001;

        var newRequest = request with
        {
            Id = nextId,
            RequestNo = nextRequestNo,
            Status = 1, // Pending
            ApprovedBy = null,
            DisbursedDate = null,
            SettlementDate = null,
            SettlementAmount = null,
            RefundAmount = null
        };

        MockPettyCashData.Requests.Add(newRequest);
        return Task.FromResult(Result<PettyCashRequestDto>.Success(newRequest));
    }

    public Task<Result<PettyCashRequestDto>> UpdateAsync(int id, PettyCashRequestDto request, CancellationToken ct = default)
    {
        var existing = MockPettyCashData.Requests.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PettyCashRequestDto>.Failure("طلب العهدة غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<PettyCashRequestDto>.Failure("لا يمكن تعديل طلب بعد اعتماده أو صرفه"));

        var updated = existing with
        {
            Date = request.Date,
            EmployeeName = request.EmployeeName,
            EmployeeId = request.EmployeeId,
            Amount = request.Amount,
            Reason = request.Reason,
            Notes = request.Notes
        };

        var index = MockPettyCashData.Requests.IndexOf(existing);
        MockPettyCashData.Requests[index] = updated;
        return Task.FromResult(Result<PettyCashRequestDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockPettyCashData.Requests.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("طلب العهدة غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result.Failure("لا يمكن حذف طلب بعد اعتماده أو صرفه. قم بإلغائه أولاً."));

        MockPettyCashData.Requests.Remove(existing);
        return Task.FromResult(Result.Success());
    }
}
