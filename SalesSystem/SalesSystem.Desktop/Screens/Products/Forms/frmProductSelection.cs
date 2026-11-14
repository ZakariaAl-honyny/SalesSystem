using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using Serilog;

namespace SalesSystem.Desktop.Products.Forms;

public partial class frmProductSelection : Form
{
    private const string EmptyValue = "—";

    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private DataTable _productsTable;
    private DataView _productsView;

    private readonly List<ProductUnitDto> _units = new();
    private readonly Dictionary<int, decimal> _stockByProduct = new();

    private bool _loadingProducts;
    private bool _unitsLoadFailed;
    private int _unitsProductId;

    public ProductDto? SelectedProduct { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ProductUnitDto? SelectedUnit { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public short WarehouseId { get; set; }

    private frmProductSelection()
    {
        InitializeComponent();

        // RULE-N40: الأربعة معاً — Enter/Tab + تحديد القيمة + تظليل الحقل + أسهم RTL.
        // تُوصَّل بعد InitializeComponent وقبل أي ربط أحداث يعتمد على التركيز؛
        // معالجة Enter في txtSearch_KeyDown و dgvProducts_KeyDown تبقى كما هي
        // (WireArrowNavigation لا يلمس Enter ولا شبكات البيانات).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));   // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                     // RULE-N45

        // RULE-N43: زر «اختيار» معطّل بلا سجل محدد، ورمادي باهت عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSelect);

        // RULE-N47: F5 تحديث القائمة. Enter/Esc يُديرهما AcceptButton/CancelButton
        // ومعالجات الشبكة الموجودة — لا تُكرَّر هنا لتجنّب التنفيذ المزدوج.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => _LoadProducts()));

        dgvProducts.AutoGenerateColumns = false;
        dgvUnits.AutoGenerateColumns = false;

        _UpdateButtonsState();
    }

    /// <summary>آلة حالة الأزرار (RULE-N43): «اختيار» متاح فقط عند وجود صنف محدد.</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = _CurrentProductId() > 0 && !_unitsLoadFailed;
    }

    public frmProductSelection(IServiceScopeFactory scopeFactory) : this()
    {
        _scope = scopeFactory.CreateScope();
    }

    private void frmProductSelection_Load(object sender, EventArgs e)
    {
        _LoadProducts();

        ApplyGreenTheme();   // RULE-N50 — آمن للاستدعاء المتكرر (idempotent)
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — أخيراً بعد ThemeHelper.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvProducts);

        // RULE-N22/RULE-N52: سلوك الشبكتين تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvProducts.ReadOnly = true;
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.MultiSelect = false;

        dgvUnits.ReadOnly = true;
        dgvUnits.AllowUserToAddRows = false;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير (RULE-N50)؛ يطبّق ثيم الشبكة على dgvUnits أيضاً
    }

    // ── تحميل المنتجات + أرصدة المخزن ──────────────────────────────────────
    private async void _LoadProducts()
    {
        _loadingProducts = true;
        Cursor = dgvProducts.Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadStockAsync();

            _productsTable = new DataTable();
            _productsTable.Columns.Add("Id", typeof(int));
            _productsTable.Columns.Add("Name", typeof(string));
            _productsTable.Columns.Add("CategoryName", typeof(string));
            _productsTable.Columns.Add("Barcode", typeof(string));
            _productsTable.Columns.Add("Available", typeof(string));

            var result = await ServiceProvider.GetRequiredService<IProductDesktopService>().GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var p in result.Value)
                {
                    _productsTable.Rows.Add(
                        p.Id,
                        p.Name,
                        p.CategoryName ?? string.Empty,
                        p.Barcode ?? string.Empty,
                        _FormatBaseAvailable(p.Id));
                }
            }
            else if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة المنتجات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _productsView = new DataView(_productsTable);
            dgvProducts.DataSource = _productsView;
            lblRecordsCount.Text = _productsView.Count.ToString();

            // RULE-N22: إشعار «لا توجد بيانات» داخل الشاشة بدل نافذة منبثقة.
            lblNoData.Visible = _productsView.Count == 0;

            _loadingProducts = false;
            await _RefreshUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المنتجات");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loadingProducts = false;
            Cursor = dgvProducts.Cursor = Cursors.Default;
        }
    }

    /// <summary>نداء واحد لأرصدة المخزن الحالي — يبني خريطة ProductId → الكمية (وحدة أساسية).</summary>
    private async Task _LoadStockAsync()
    {
        _stockByProduct.Clear();

        if (WarehouseId <= 0)
            return;

        var result = await ServiceProvider.GetRequiredService<IInventoryDesktopService>()
            .GetStockByWarehouseAsync(WarehouseId);

        if (result.IsSuccess && result.Value != null)
        {
            foreach (var stock in result.Value)
                _stockByProduct[stock.ProductId] = stock.Quantity;

            return;
        }

        if (!result.IsSuccess)
        {
            Log.Error("تعذر تحميل أرصدة المخزن {WarehouseId}: {Error}", WarehouseId, result.Error);
            MessageBox.Show("تعذر تحميل أرصدة المخزن. ستظهر الكميات المتاحة صفراً.",
                "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private string _FormatBaseAvailable(int productId)
    {
        if (WarehouseId <= 0)
            return EmptyValue;

        return _FormatQuantity(_GetBaseAvailable(productId));
    }

    private decimal _GetBaseAvailable(int productId)
        => _stockByProduct.TryGetValue(productId, out decimal quantity) ? quantity : 0m;

    private static string _FormatQuantity(decimal value)
        => value.ToString("G", CultureInfo.CurrentCulture);

    // ── شبكة الوحدات (التفصيل) ───────────────────────────────────────────────
    private async Task _RefreshUnitsAsync()
    {
        int productId = _CurrentProductId();

        if (productId <= 0)
        {
            _ClearUnits();
            return;
        }

        if (_unitsProductId == productId)
            return;

        Cursor = dgvUnits.Cursor = Cursors.WaitCursor;
        try
        {
            _units.Clear();
            _unitsLoadFailed = false;
            dgvUnits.DataSource = null;
            lblNoUnits.Visible = false;
            lblSelectedProduct.Text = $"المنتج المحدد: {_CurrentProductName()}";

            var result = await ServiceProvider.GetRequiredService<IProductDesktopService>()
                .GetProductUnitsAsync(productId);

            if (result.IsSuccess && result.Value != null)
            {
                _units.AddRange(result.Value);
            }
            else if (!result.IsSuccess)
            {
                _unitsLoadFailed = true;
                Log.Error("تعذر تحميل وحدات المنتج {ProductId}: {Error}", productId, result.Error);
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل وحدات المنتج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            decimal baseAvailable = _GetBaseAvailable(productId);
            var unitsTable = _CreateUnitsTable();

            foreach (var unit in _units)
            {
                decimal factor = unit.ConversionFactor;
                decimal available = factor > 0 ? baseAvailable / factor : 0m;

                unitsTable.Rows.Add(
                    unit.Id,
                    string.IsNullOrWhiteSpace(unit.UnitName) ? $"وحدة {unit.UnitId}" : unit.UnitName,
                    factor,
                    WarehouseId > 0 ? _FormatQuantity(available) : EmptyValue,
                    unit.SalePrice);
            }

            dgvUnits.DataSource = unitsTable;
            if (!_unitsLoadFailed)
                _unitsProductId = productId;
            lblNoUnits.Visible = _units.Count == 0;

            if (dgvUnits.Rows.Count > 0)
                dgvUnits.CurrentCell = dgvUnits.Rows[0].Cells[colUnitName.Index];
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات المنتج {ProductId}", productId);
            MessageBox.Show("حدث خطأ في تحميل وحدات المنتج. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = dgvUnits.Cursor = Cursors.Default;
            _UpdateButtonsState();
        }
    }

    private static DataTable _CreateUnitsTable()
    {
        var table = new DataTable();
        table.Columns.Add("UnitId", typeof(int));
        table.Columns.Add("UnitName", typeof(string));
        table.Columns.Add("ConversionFactor", typeof(decimal));
        table.Columns.Add("Available", typeof(string));
        table.Columns.Add("SalePrice", typeof(decimal));
        return table;
    }

    private void _ClearUnits()
    {
        _units.Clear();
        dgvUnits.DataSource = null;
        _unitsProductId = 0;
        _unitsLoadFailed = false;
        lblSelectedProduct.Text = $"المنتج المحدد: {EmptyValue}";
        lblNoUnits.Visible = false;
        _UpdateButtonsState();
    }

    private int _CurrentProductId()
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is not DataRowView row) return 0;
        if (row["Id"] == DBNull.Value) return 0;

        return Convert.ToInt32(row["Id"]);
    }

    private string _CurrentProductName()
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is not DataRowView row) return string.Empty;

        return row["Name"] as string ?? string.Empty;
    }

    private decimal _GetAvailableForUnit(ProductUnitDto unit)
        => unit.ConversionFactor > 0
            ? _GetBaseAvailable(unit.ProductId) / unit.ConversionFactor
            : 0m;

    // ── البحث ────────────────────────────────────────────────────────────────
    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_productsView == null) return;

        string search = txtSearch.Text.Trim();

        if (string.IsNullOrEmpty(search))
        {
            _productsView.RowFilter = string.Empty;
        }
        else
        {
            // RULE-N24/RULE-N60: قيمة البحث تُهرَّب عبر RowFilterHelper — لا إدراج خام.
            _productsView.RowFilter = RowFilterHelper.Like("Name", search)
                + " OR " + RowFilterHelper.Like("Barcode", search)
                + " OR " + RowFilterHelper.Like("CategoryName", search);
        }

        lblRecordsCount.Text = _productsView.Count.ToString();
        lblNoData.Visible = _productsView.Count == 0;
    }

    private async void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter) return;

        e.Handled = true;
        try
        {
            await _AdvanceToUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء الانتقال إلى وحدات المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        try
        {
            await _AdvanceToUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء الانتقال إلى وحدات المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── أحداث شبكة المنتجات ──────────────────────────────────────────────────
    private async void dgvProducts_SelectionChanged(object sender, EventArgs e)
    {
        if (_loadingProducts) return;

        try
        {
            await _RefreshUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات المنتج");
            MessageBox.Show("حدث خطأ في تحميل وحدات المنتج. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void dgvProducts_DoubleClick(object sender, EventArgs e)
    {
        try
        {
            await _AdvanceToUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء الانتقال إلى وحدات المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void dgvProducts_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        try
        {
            await _AdvanceToUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء الانتقال إلى وحدات المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>الانتقال من شبكة المنتجات إلى شبكة الوحدات — أو الإغلاق مباشرة إن لم توجد وحدات.</summary>
    private async Task _AdvanceToUnitsAsync()
    {
        if (_CurrentProductId() <= 0) return;

        await _RefreshUnitsAsync();

        // ننتظر دورة رسائل واحدة حتى تنتهي معالجة Enter الحالية (WireEnterAsTab) قبل نقل التركيز
        await Task.Yield();

        if (_unitsLoadFailed) return;

        if (_units.Count > 0 && dgvUnits.Rows.Count > 0)
        {
            if (dgvUnits.CurrentCell == null)
                dgvUnits.CurrentCell = dgvUnits.Rows[0].Cells[colUnitName.Index];

            dgvUnits.Focus();
            return;
        }

        // لا توجد وحدات لهذا الصنف — الرجوع للسلوك السابق (اختيار المنتج فقط)
        _CommitProduct(null);
    }

    // ── أحداث شبكة الوحدات ───────────────────────────────────────────────────
    private async void dgvUnits_DoubleClick(object sender, EventArgs e)
    {
        try
        {
            await _CommitFromUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار وحدة المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void dgvUnits_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        try
        {
            await _CommitFromUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار وحدة المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnSelect_Click(object sender, EventArgs e)
    {
        try
        {
            await _CommitFromUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار المنتج");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>اختيار الوحدة الحالية في شبكة الوحدات والتأكيد.</summary>
    private async Task _CommitFromUnitsAsync()
    {
        if (_CurrentProductId() <= 0) return;

        await _RefreshUnitsAsync();

        if (_unitsLoadFailed) return;

        if (_units.Count == 0)
        {
            // الرجوع للسلوك السابق — اختيار المنتج بدون وحدة
            _CommitProduct(null);
            return;
        }

        if (dgvUnits.CurrentRow?.DataBoundItem is not DataRowView row) return;

        int unitId = row["UnitId"] == DBNull.Value ? 0 : Convert.ToInt32(row["UnitId"]);
        var unit = _units.FirstOrDefault(u => u.Id == unitId);

        if (unit == null) return;

        if (WarehouseId > 0 && _GetAvailableForUnit(unit) <= 0)
        {
            MessageBox.Show("لا توجد كمية متاحة من هذه الوحدة في المخزن المحدد.",
                "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _CommitProduct(unit);
    }

    private void _CommitProduct(ProductUnitDto? unit)
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is not DataRowView row) return;

        int id = (int)row["Id"];
        string name = row["Name"] as string ?? string.Empty;

        SelectedProduct = new ProductDto(
            id,
            name,
            0,
            row["CategoryName"] as string,
            null,
            0,
            false,
            null,
            true,
            row["Barcode"] as string
        );
        SelectedUnit = unit;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
