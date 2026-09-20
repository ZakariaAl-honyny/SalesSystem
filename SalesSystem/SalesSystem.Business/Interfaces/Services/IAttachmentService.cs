using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface IAttachmentService
{
    Task<Result<List<AttachmentDto>>> GetAllAsync(string referenceType, int? referenceId, CancellationToken ct);
    Task<Result<AttachmentDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<AttachmentDto>> CreateAsync(CreateAttachmentRequest request, CancellationToken ct);
    Task<Result<AttachmentDto>> UpdateAsync(int id, UpdateAttachmentRequest request, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
}
