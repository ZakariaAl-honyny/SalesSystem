using System.ComponentModel;

namespace SalesSystem.Desktop.Models;

public class InvoiceLineItem : INotifyPropertyChanged
{
    public Guid LineKey { get; private set; } = Guid.NewGuid();

    private int _productId;
    public int ProductId { get => _productId; set { _productId = value; OnPropertyChanged(nameof(ProductId)); } }

    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? Barcode { get; set; }
    public string? UnitName { get; set; }
    public int ProductUnitId { get; set; }

    /// <summary>
    /// معامل التحويل إلى الوحدة الأساسية (1 للوحدة الأساسية، 12 لكرتونة من 12 حبة...).
    /// يُستخدم لتحويل كميات السطر إلى وحدات أساسية عند التحقق من المخزون.
    /// </summary>
    public decimal ConversionFactor { get; set; } = 1m;

    private decimal _quantity;
    public decimal Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(nameof(Quantity)); OnPropertyChanged(nameof(LineTotal)); }
    }

    private decimal _unitPrice;
    public decimal UnitPrice
    {
        get => _unitPrice;
        set { _unitPrice = value; OnPropertyChanged(nameof(UnitPrice)); OnPropertyChanged(nameof(LineTotal)); }
    }

    private decimal _unitCost;
    public decimal UnitCost
    {
        get => _unitCost;
        set { _unitCost = value; OnPropertyChanged(nameof(UnitCost)); OnPropertyChanged(nameof(LineTotal)); }
    }

    private decimal _discountAmount;
    public decimal DiscountAmount
    {
        get => _discountAmount;
        set { _discountAmount = value; OnPropertyChanged(nameof(DiscountAmount)); OnPropertyChanged(nameof(LineTotal)); }
    }

    /// <summary>
    /// تاريخ انتهاء صلاحية السطر — null يعني "بدون تاريخ انتهاء".
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// إجمالي السطر — نفس صيغة الكيان/الخادم: (الكمية × سعر الوحدة) - خصم السطر.
    /// لا يوجد أي رجوع إلى UnitCost حتى لا تختلف قيمة الشاشة عن المحفوظ.
    /// </summary>
    public decimal LineTotal => (Quantity * UnitPrice) - DiscountAmount;

    public InvoiceLineItem Clone()
    {
        var copy = new InvoiceLineItem
        {
            ProductId = ProductId,
            ProductCode = ProductCode,
            ProductName = ProductName,
            Barcode = Barcode,
            UnitName = UnitName,
            ProductUnitId = ProductUnitId,
            ConversionFactor = ConversionFactor,
            Quantity = Quantity,
            UnitPrice = UnitPrice,
            UnitCost = UnitCost,
            DiscountAmount = DiscountAmount,
            ExpiryDate = ExpiryDate
        };
        copy.LineKey = LineKey;
        return copy;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
