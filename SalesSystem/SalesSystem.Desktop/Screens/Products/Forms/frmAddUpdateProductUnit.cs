using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using Serilog;
using System.Windows.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms
{
    public partial class frmAddUpdateProductUnit : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private int _productId;
        private int _unitId;
        private System.Windows.Forms.ToolTip tooltip;

        public frmAddUpdateProductUnit()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmAddUpdateProductUnit(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;

            tooltip = new ToolTip(components);
            tooltip.SetToolTip(cmbProduct, "المنتج المراد إضافة وحدة فرعية له");
            tooltip.SetToolTip(cmbUnit, "اختر وحدة فرعية للمنتج\nالوحدات المضافة مسبقاً غير متاحة");
            tooltip.SetToolTip(txtFactor, "عدد الوحدات الأساسية التي تعادل هذه الوحدة\nمثال: 1 كرتون = 12 حبة");
            tooltip.SetToolTip(chkIsBaseUnit, "الوحدة الأساسية هي أصغر وحدة قياس للمنتج");
        }

        public void LoadData(int productId, int unitId)
        {
            _productId = productId;
            _unitId = unitId;

            if (_unitId > 0)
            {
                this.Text = "تعديل وحدة المنتج";
                lblMode.Text = "تعديل وحدة المنتج";
            }
            else
            {
                this.Text = "إضافة وحدة منتج";
                lblMode.Text = "إضافة وحدة منتج";
                txtFactor.Text = "1";
            }

            chkIsBaseUnit.Visible = false;
        }

        private async void LoadUnitData()
        {
            foreach (var item in cmbUnit.Items)
            {
                if (item is UnitDto u && u.Id == _unitId)
                {
                    cmbUnit.SelectedItem = item;
                    break;
                }
            }
            txtFactor.Text = "1";
            chkIsBaseUnit.Checked = false;
        }

        private async void frmProductUnitEditor_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadProductsAsync();
                await LoadUnitsAsync();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
                System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductsAsync()
        {
            var result = await _serviceProvider.GetRequiredService<IProductApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "Id";
                cmbProduct.DataSource = new List<ProductDto>(result.Value);
            }
        }

        private async Task LoadUnitsAsync()
        {
            var unitsResult = await _serviceProvider.GetRequiredService<IUnitApiService>().GetAllAsync();
            if (!unitsResult.IsSuccess || unitsResult.Value == null) return;

            var units = unitsResult.Value.ToList();

            if (_productId > 0)
            {
                var productUnits = await _serviceProvider.GetRequiredService<IProductApiService>()
                    .GetProductUnitsAsync(_productId);
                if (productUnits.IsSuccess && productUnits.Value != null)
                {
                    var existingIds = productUnits.Value.Select(pu => pu.UnitId).ToHashSet();
                    units = units.Where(u => !existingIds.Contains(u.Id)).ToList();
                }
            }

            cmbUnit.DisplayMember = "Name";
            cmbUnit.ValueMember = "Id";
            cmbUnit.DataSource = new List<UnitDto>(units);
        }

        private async Task<bool> _ValidateFormAsync()
        {
            bool isValid = true;

            if (cmbUnit.SelectedValue == null)
            {
                errorProvider.SetError(cmbUnit, "يرجى اختيار الوحدة");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(cmbUnit, "");
            }

            if (_productId > 0 && cmbUnit.SelectedValue is int selectedUnitId)
            {
                var productUnits = await _serviceProvider.GetRequiredService<IProductApiService>()
                    .GetProductUnitsAsync(_productId);
                if (productUnits.IsSuccess && productUnits.Value != null)
                {
                    var existing = productUnits.Value.FirstOrDefault(pu => pu.UnitId == selectedUnitId);
                    if (existing != null)
                    {
                        errorProvider.SetError(cmbUnit, "هذه الوحدة مضمنة بالفعل مع المنتج");
                        isValid = false;
                    }
                }
            }

            decimal factor = 1m;
            if (!decimal.TryParse(txtFactor.Text, out factor) || factor <= 0)
            {
                errorProvider.SetError(txtFactor, "معامل التحويل يجب أن يكون رقماً موجباً");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtFactor, "");
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!await _ValidateFormAsync()) return;

            _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم حفظ وحدة المنتج بنجاح");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
