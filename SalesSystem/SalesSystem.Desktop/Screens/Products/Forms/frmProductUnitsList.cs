using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Products;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms
{
    public partial class frmProductUnitsList : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtUnits = new();
        private DataView _dvUnits = new();

        public frmProductUnitsList(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        private void _SetupGridColumns()
        {
            dgvUnits.Columns.Clear();

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitId",
                HeaderText = "رقم الوحدة",
                DataPropertyName = "UnitId",
                MinimumWidth = 80,
                FillWeight = 8,
                ReadOnly = true,
            });

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitName",
                HeaderText = "اسم الوحدة",
                DataPropertyName = "UnitName",
                MinimumWidth = 200,
                FillWeight = 22,
                ReadOnly = true,
            });

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Symbol",
                HeaderText = "الرمز",
                DataPropertyName = "Symbol",
                MinimumWidth = 80,
                FillWeight = 10,
                ReadOnly = true,
            });

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Factor",
                HeaderText = "معامل التحويل",
                DataPropertyName = "Factor",
                MinimumWidth = 100,
                FillWeight = 10,
                ReadOnly = true,
            });

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsBaseUnit",
                HeaderText = "الوحدة الأساسية",
                DataPropertyName = "IsBaseUnit",
                MinimumWidth = 100,
                FillWeight = 10,
                ReadOnly = true,
            });
        }

        private async void _LoadProductsToCombo()
        {
            var result = await _serviceProvider.GetRequiredService<IProductApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "Id";
                cmbProduct.DataSource = new List<ProductDto>(result.Value);
            }

            cmbProduct.SelectedIndexChanged -= CmbProduct_SelectedIndexChanged;
            cmbProduct.SelectedIndexChanged += CmbProduct_SelectedIndexChanged;
        }

        private void CmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _RefreshUnitsList();
        }

        private async void _RefreshUnitsList()
        {
            Cursor = dgvUnits.Cursor = Cursors.WaitCursor;
            try
            {
                _dtUnits = new DataTable();
                _dtUnits.Columns.Add("UnitId", typeof(int));
                _dtUnits.Columns.Add("UnitName", typeof(string));
                _dtUnits.Columns.Add("Symbol", typeof(string));
                _dtUnits.Columns.Add("Factor", typeof(decimal));
                _dtUnits.Columns.Add("IsBaseUnit", typeof(string));

                if (cmbProduct.SelectedValue is not int productId)
                {
                    label1.Visible = false;
                    lblRecordsCount.Text = "الرجاء اختيار منتج";
                    return;
                }

                var result = await _serviceProvider.GetRequiredService<IProductApiService>()
                    .GetProductUnitsAsync(productId);
                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var pu in result.Value)
                    {
                        _dtUnits.Rows.Add(
                            pu.UnitId,
                            pu.UnitName ?? "",
                            "",
                            pu.ConversionFactor,
                            pu.IsBaseUnit ? "نعم" : "لا");
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل وحدات المنتج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _dvUnits = new DataView(_dtUnits);
                dgvUnits.DataSource = _dvUnits;
                label1.Visible = _dvUnits.Count > 0;
                lblRecordsCount.Text = _dvUnits.Count > 0 ? _dvUnits.Count.ToString() : "لا توجد وحدات فرعية";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطأ أثناء تحديث قائمة وحدات المنتج");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvUnits.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshUnitsList();
        }

        private void frmProductUnitsList_Load(object sender, EventArgs e)
        {
            _LoadProductsToCombo();
            _ReloadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        private void cmsUnits_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsUnits.Enabled = dgvUnits.Rows.Count > 0 && dgvUnits.CurrentRow != null;
        }

        private void addUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue is not int productId) return;

            var dialog = _serviceProvider.GetRequiredService<frmAddUpdateProductUnit>();
            dialog.LoadData(productId, 0);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _ReloadForm();
            }
        }

        private void editUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue is not int productId) return;
            if (dgvUnits.CurrentRow == null) return;

            var cellValue = dgvUnits.CurrentRow.Cells["UnitId"].Value;
            if (cellValue == null) return;

            int unitId = (int)cellValue;
            var dialog = _serviceProvider.GetRequiredService<frmAddUpdateProductUnit>();
            dialog.LoadData(productId, unitId);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _ReloadForm();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
