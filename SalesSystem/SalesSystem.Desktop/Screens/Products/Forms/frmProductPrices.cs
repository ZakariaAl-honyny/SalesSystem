using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using System.Data;
using SalesSystem.Desktop.Screens.Products;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms
{
    public partial class frmProductPrices : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtProducts = new();
        private DataView _dvProducts = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "اسم المنتج", "ProductName" },
            };

        public frmProductPrices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        private void _SetupGridColumns()
        {
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductId",
                HeaderText = "رقم المنتج",
                DataPropertyName = "ProductId",
                MinimumWidth = 80,
                FillWeight = 8,
                ReadOnly = true,
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "اسم المنتج",
                DataPropertyName = "ProductName",
                MinimumWidth = 200,
                FillWeight = 22,
                ReadOnly = true,
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CategoryName",
                HeaderText = "التصنيف",
                DataPropertyName = "CategoryName",
                MinimumWidth = 120,
                FillWeight = 12,
                ReadOnly = true,
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Barcode",
                HeaderText = "الباركود",
                DataPropertyName = "Barcode",
                MinimumWidth = 100,
                FillWeight = 10,
                ReadOnly = true,
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SalePrice",
                HeaderText = "السعر",
                DataPropertyName = "SalePrice",
                MinimumWidth = 100,
                FillWeight = 10,
                ReadOnly = true,
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockStatus",
                HeaderText = "حالة المخزون",
                DataPropertyName = "StockStatus",
                MinimumWidth = 100,
                FillWeight = 10,
                ReadOnly = true,
            });
        }

        private async void _RefreshProductsList()
        {
            Cursor = dgvProducts.Cursor = Cursors.WaitCursor;
            try
            {
                _dtProducts = new DataTable();
                _dtProducts.Columns.Add("ProductId", typeof(int));
                _dtProducts.Columns.Add("ProductName", typeof(string));
                _dtProducts.Columns.Add("CategoryName", typeof(string));
                _dtProducts.Columns.Add("Barcode", typeof(string));
                _dtProducts.Columns.Add("SalePrice", typeof(decimal));
                _dtProducts.Columns.Add("StockStatus", typeof(string));

                var result = await _serviceProvider.GetRequiredService<IProductApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _dtProducts.Rows.Add(
                            p.Id,
                            p.Name,
                            p.CategoryName ?? "",
                            p.Barcode ?? "",
                            p.SalePrice,
                            p.StockStatusLabel);
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل أسعار المنتجات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _dvProducts = new DataView(_dtProducts);
                dgvProducts.DataSource = _dvProducts;
                label1.Visible = _dvProducts.Count > 0;
                lblRecordsCount.Text = _dvProducts.Count > 0 ? _dvProducts.Count.ToString() : "لا توجد بيانات";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطأ أثناء تحديث قائمة أسعار المنتجات");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvProducts.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshProductsList();
        }

        private int _GetSelectedProductId()
        {
            if (dgvProducts.CurrentRow == null) return -1;

            var cellValue = dgvProducts.CurrentRow.Cells[0].Value;
            if (cellValue == null) return -1;

            return (int)cellValue;
        }

        private void frmProductPrices_Load(object sender, EventArgs e)
        {
            _ReloadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        private void cmsPrices_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsPrices.Enabled = dgvProducts.Rows.Count > 0 && dgvProducts.CurrentRow != null;
        }

        private void editPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedProductId();
            if (id > 0)
            {
                _serviceProvider.GetRequiredService<INotificationService>()
                    .ShowWarning("تحرير الأسعار قيد التطوير");
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText))
            {
                _dvProducts.RowFilter = "";
            }
            else
            {
                string escaped = filterText.Replace("'", "''");
                _dvProducts.RowFilter = string.Format(
                    "ProductName LIKE '%{0}%'", escaped);
            }

            lblRecordsCount.Text = _dvProducts.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
