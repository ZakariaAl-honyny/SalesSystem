using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Customer entity with direct contact information.
/// Schema §1.2 — Customers table.
/// </summary>
public class Customer : ActivatableEntity
{
    /// <summary>
    /// Customer name (required). This field holds the primary display name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Customer phone number (optional).
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Customer email address (optional).
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Customer physical address (optional).
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Customer tax number / VAT registration (optional).
    /// </summary>
    public string? TaxNumber { get; private set; }

    /// <summary>
    /// Free-text notes about the customer (optional).
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// تصنيف حر (عمود مرجعي بلا جدول هدف في MVP) — انظر docs/database-schema.md §10
    /// </summary>
    public int? CategoryId { get; private set; }

    /// <summary>
    /// Maximum credit allowed for this customer. Zero means no credit limit enforced.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Number of days allowed for payment (payment term). Null means system default.
    /// </summary>
    public int? PaymentTermDays { get; private set; }

    private Customer() { } // EF Core

    /// <summary>
    /// Factory method to create a new customer with direct contact information.
    /// </summary>
    /// <param name="name">Customer name (required).</param>
    /// <param name="phone">Optional phone number.</param>
    /// <param name="email">Optional email address.</param>
    /// <param name="address">Optional physical address.</param>
    /// <param name="taxNumber">Optional tax number / VAT registration.</param>
    /// <param name="notes">Optional free-text notes.</param>
    /// <param name="creditLimit">Credit limit (default 0 = no limit).</param>
    /// <param name="categoryId">تصنيف حر (عمود مرجعي بلا جدول هدف في MVP) — انظر docs/database-schema.md §10</param>
    /// <returns>A new Customer instance.</returns>
    /// <exception cref="DomainException">If any guard clause fails.</exception>
    public static Customer Create(
        string name,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? taxNumber = null,
        string? notes = null,
        decimal creditLimit = 0,
        int? paymentTermDays = null,
        int? categoryId = null,
        string? createdByUserName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم العميل مطلوب.");
        if (creditLimit < 0)
            throw new DomainException("حد الائتمان لا يمكن أن يكون سالباً.");

        var customer = new Customer
        {
            Name = name.Trim(),
            Phone = phone?.Trim(),
            Email = email?.Trim(),
            Address = address?.Trim(),
            TaxNumber = taxNumber?.Trim(),
            Notes = notes?.Trim(),
            CreditLimit = creditLimit,
            PaymentTermDays = paymentTermDays,
            CategoryId = categoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        if (!string.IsNullOrWhiteSpace(createdByUserName)) customer.SetCreatedBy(createdByUserName.Trim());
        return customer;
    }

    /// <summary>
    /// Checks whether adding an additional amount would exceed the credit limit.
    /// Non-throwing — the caller decides whether to block.
    /// </summary>
    /// <param name="additionalAmount">The additional amount to check against the limit.</param>
    /// <returns>True if the transaction is within the credit limit (or no limit is set).</returns>
    public bool CheckCreditLimit(decimal additionalAmount)
    {
        if (CreditLimit <= 0)
            return true;

        return additionalAmount <= CreditLimit;
    }

    /// <summary>
    /// Updates the customer fields including contact information.
    /// </summary>
    /// <param name="name">Customer name (required).</param>
    /// <param name="phone">Optional phone number.</param>
    /// <param name="email">Optional email address.</param>
    /// <param name="address">Optional physical address.</param>
    /// <param name="taxNumber">Optional tax number / VAT registration.</param>
    /// <param name="notes">Optional free-text notes.</param>
    /// <param name="creditLimit">New credit limit (&gt;= 0).</param>
    /// <param name="categoryId">New category id (null = keep current).</param>
    /// <exception cref="DomainException">If any guard clause fails.</exception>
    public void Update(
        string name,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? taxNumber = null,
        string? notes = null,
        decimal creditLimit = 0,
        int? paymentTermDays = null,
        int? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم العميل مطلوب.");
        if (creditLimit < 0)
            throw new DomainException("حد الائتمان لا يمكن أن يكون سالباً.");

        Name = name.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
        TaxNumber = taxNumber?.Trim();
        Notes = notes?.Trim();
        CreditLimit = creditLimit;
        PaymentTermDays = paymentTermDays;
        if (categoryId.HasValue)
            CategoryId = categoryId;
        UpdateTimestamp();
    }
}
