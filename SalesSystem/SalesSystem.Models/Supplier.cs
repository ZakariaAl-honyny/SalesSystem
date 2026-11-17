using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Supplier entity with direct contact information.
/// Schema §1.4 — Suppliers table.
/// </summary>
public class Supplier : ActivatableEntity
{
    /// <summary>
    /// Supplier name (required). This field holds the primary display name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Supplier phone number (optional).
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Supplier email address (optional).
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Supplier physical address (optional).
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Supplier tax number / VAT registration (optional).
    /// </summary>
    public string? TaxNumber { get; private set; }

    /// <summary>
    /// Commercial registration number (optional).
    /// </summary>
    public string? CommercialRegisterNo { get; private set; }

    /// <summary>
    /// Free-text notes about the supplier (optional).
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Maximum credit limit for this supplier (decimal(18,2)). Zero means no limit.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// تصنيف حر (عمود مرجعي بلا جدول هدف في MVP) — انظر docs/database-schema.md §10
    /// </summary>
    public int? CategoryId { get; private set; }

    private Supplier() { } // EF Core

    /// <summary>
    /// Factory method to create a new supplier with direct contact information.
    /// </summary>
    /// <param name="name">Supplier name (required).</param>
    /// <param name="phone">Optional phone number.</param>
    /// <param name="email">Optional email address.</param>
    /// <param name="address">Optional physical address.</param>
    /// <param name="taxNumber">Optional tax number / VAT registration.</param>
    /// <param name="notes">Optional free-text notes.</param>
    /// <param name="categoryId">تصنيف حر (عمود مرجعي بلا جدول هدف في MVP) — انظر docs/database-schema.md §10</param>
    /// <returns>A new Supplier instance.</returns>
    /// <exception cref="DomainException">If any guard clause fails.</exception>
    public static Supplier Create(
        string name,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? taxNumber = null,
        string? commercialRegisterNo = null,
        string? notes = null,
        decimal creditLimit = 0,
        int? categoryId = null,
        string? createdByUserName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم المورد مطلوب.");
        if (creditLimit < 0)
            throw new DomainException("الحد الائتماني لا يمكن أن يكون سالباً.");

        var supplier = new Supplier
        {
            Name = name.Trim(),
            Phone = phone?.Trim(),
            Email = email?.Trim(),
            Address = address?.Trim(),
            TaxNumber = taxNumber?.Trim(),
            CommercialRegisterNo = commercialRegisterNo?.Trim(),
            Notes = notes?.Trim(),
            CreditLimit = creditLimit,
            CategoryId = categoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        if (!string.IsNullOrWhiteSpace(createdByUserName)) supplier.SetCreatedBy(createdByUserName.Trim());
        return supplier;
    }

    /// <summary>
    /// Updates the supplier fields including contact information.
    /// </summary>
    /// <param name="name">Supplier name (required).</param>
    /// <param name="phone">Optional phone number.</param>
    /// <param name="email">Optional email address.</param>
    /// <param name="address">Optional physical address.</param>
    /// <param name="taxNumber">Optional tax number / VAT registration.</param>
    /// <param name="categoryId">New category id (null = keep current).</param>
    /// <param name="commercialRegisterNo">Optional commercial registration number.</param>
    /// <param name="notes">Optional free-text notes.</param>
    /// <exception cref="DomainException">If any guard clause fails.</exception>
    public void Update(
        string name,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? taxNumber = null,
        string? commercialRegisterNo = null,
        string? notes = null,
        decimal creditLimit = 0,
        int? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم المورد مطلوب.");
        if (creditLimit < 0)
            throw new DomainException("الحد الائتماني لا يمكن أن يكون سالباً.");

        Name = name.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
        TaxNumber = taxNumber?.Trim();
        CommercialRegisterNo = commercialRegisterNo?.Trim();
        Notes = notes?.Trim();
        CreditLimit = creditLimit;
        if (categoryId.HasValue)
            CategoryId = categoryId;
        UpdateTimestamp();
    }

    /// <summary>
    /// Non-throwing SOFT WARNING check. Returns true if additional amount would exceed limit.
    /// Caller decides whether to block (per RULE-448).
    /// </summary>
    public bool CheckCreditLimit(decimal additionalAmount, decimal currentBalance)
    {
        if (CreditLimit <= 0) return false; // No limit = always OK
        return (currentBalance + additionalAmount) > CreditLimit;
    }
}
