namespace SalesSystem.DTO.Enums;

public enum InvoiceStatus : byte { Draft = 1, Posted = 2, Cancelled = 3 }

public enum PaymentType : byte { Cash = 1, Credit = 2, Mixed = 3 }

public enum MovementType : byte
{
    PurchaseIn = 1,
    SaleOut = 2,
    SaleReturnIn = 3,
    PurchaseReturnOut = 4,
    TransferOut = 5,
    TransferIn = 6,
    Adjustment = 7
}

public enum SaleMode : byte { Retail = 1, Wholesale = 2 }

/// <summary>
/// نوع الخصم: مبلغ أو نسبة مئوية
/// </summary>
public enum DiscountType : byte { Amount = 0, Percentage = 1 }

public enum UserRole : byte
{
    Admin = 1,
    Manager = 2,
    Treasurer = 4,
    Cashier = 5,
    WarehouseSupervisor = 6,
    SalesEmployee = 7,
    Observer = 8
}
