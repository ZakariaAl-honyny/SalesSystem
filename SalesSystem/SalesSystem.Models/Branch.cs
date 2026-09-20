using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a business branch/office. V1 seeds one default branch (المركز الرئيسي).
/// All sovereign and operational entities reference BranchId for future multi-branch support.
/// </summary>
public class Branch : ActivatableEntity
{
    public new short Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Phone { get; private set; }

    protected Branch() { }

    public static Branch Create(string name, string? address = null, string? phone = null, int? createdByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الفرع مطلوب.");
        var branch = new Branch
        {
            Name = name.Trim(),
            Address = address?.Trim(),
            Phone = phone?.Trim()
        };
        branch.SetCreatedBy(createdByUserId);
        return branch;
    }

    public void Update(string name, string? address = null, string? phone = null, int? updatedByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الفرع مطلوب.");
        Name = name.Trim();
        Address = address?.Trim();
        Phone = phone?.Trim();
        SetUpdatedBy(updatedByUserId);
        UpdateTimestamp();
    }
}
