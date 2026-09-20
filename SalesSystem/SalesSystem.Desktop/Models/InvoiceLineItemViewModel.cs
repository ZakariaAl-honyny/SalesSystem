using System.ComponentModel;

namespace SalesSystem.Desktop.Models;

public class InvoiceLineItemViewModel : INotifyPropertyChanged
{
    public Guid LineKey { get; private set; } = Guid.NewGuid();

    private int _productId;
    public int ProductId { get => _productId; set { _productId = value; OnPropertyChanged(nameof(ProductId)); } }

    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? Barcode { get; set; }
    public string? UnitName { get; set; }
    public int ProductUnitId { get; set; }

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

    public decimal LineTotal => (Quantity * (UnitPrice != 0 ? UnitPrice : UnitCost)) - DiscountAmount;

    public InvoiceLineItemViewModel Clone()
    {
        var copy = new InvoiceLineItemViewModel
        {
            ProductId = ProductId,
            ProductCode = ProductCode,
            ProductName = ProductName,
            Barcode = Barcode,
            UnitName = UnitName,
            ProductUnitId = ProductUnitId,
            Quantity = Quantity,
            UnitPrice = UnitPrice,
            UnitCost = UnitCost,
            DiscountAmount = DiscountAmount
        };
        copy.LineKey = LineKey;
        return copy;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
