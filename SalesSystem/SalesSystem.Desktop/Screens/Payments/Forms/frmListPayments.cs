using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Globalization;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.CustomerReceipts.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    public partial class frmListPayments : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _customerTable = new();
        private DataView _customerView = new();
        private DataTable _supplierTable = new();
        private DataView _supplierView = new();

        /// <summary>يمنع تطبيق التصفية أثناء ضبط القيم برمجياً (RULE-N59 — تصفية فورية بلا وميض).</summary>
        private bool _isInitializing;

        public frmListPayments(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            _InitializeFilters();   // RULE-N57 — عناصر gbFilter ونطاق التاريخ الابتدائي
            _InitializeToolTips();  // RULE-N47 — كل زر/حقل تصفية وله تلميح

            KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
            KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
            KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
            KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
            KeyboardHelper.WireButtonDisabledVisual(btnAddCustomerPayment, btnAddSupplierPayment); // RULE-N43
            KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
                (Keys.F2, _AddNewForActiveTab),
                (Keys.F5, _RefreshActiveTab),
                (Keys.Escape, _ResetFilters));
        }

        /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
        private void ApplyGreenTheme()
        {
            GreenThemeHelper.ApplyHeader(lblMode);
            GreenThemeHelper.ApplyGrid(dgvCustomerPayments);
            GreenThemeHelper.ApplyGrid(dgvSupplierPayments);

            // RULE-N22: سلوك شبكتي العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
            dgvCustomerPayments.ReadOnly = true;
            dgvCustomerPayments.AllowUserToAddRows = false;
            dgvCustomerPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomerPayments.MultiSelect = false;

            dgvSupplierPayments.ReadOnly = true;
            dgvSupplierPayments.AllowUserToAddRows = false;
            dgvSupplierPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplierPayments.MultiSelect = false;

            // RULE-N53/N54: النص المميّز يُستهلك من ثوابت الثيم لا من قيم مكرّرة.
            lblSummary.ForeColor = GreenThemeHelper.TextStrong;

            GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
        }

        // ── التصفية (§2.22) ────────────────────────────────────────────────────

        /// <summary>RULE-N57: تجهيز عناصر gbFilter ونطاق التاريخ الابتدائي.</summary>
        private void _InitializeFilters()
        {
            _isInitializing = true;
            try
            {
                cbFilterBy.Items.Clear();
                cbFilterBy.Items.AddRange(new object[]
                {
                    "لا شيء",
                    "المستفيد/المورد",
                    "رقم السند",
                    "البيان",
                    "المبلغ"
                });

                cbStatus.Items.Clear();
                cbStatus.Items.AddRange(new object[] { "الكل", "مرحّل", "ملغى" });

                cbFilterBy.SelectedIndex = 1;   // المستفيد/المورد
                cbStatus.SelectedIndex = 0;     // الكل

                // نطاق ابتدائي واسع كي لا تُخفى السندات القديمة عند فتح الشاشة،
                // و«هذا الشهر» متاح بزر وبتصفير Esc (RULE-N59).
                var today = DateTime.Today;
                dtpFrom.Value = new DateTime(today.Year, today.Month, 1).AddMonths(-3);
                dtpTo.Value = today;

                txtFilterValue.Visible = true;
            }
            finally
            {
                _isInitializing = false;
            }
        }

        /// <summary>RULE-N47: تلميحات الأزرار/الحقول مع اختصاراتها.</summary>
        private void _InitializeToolTips()
        {
            toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به السندات");
            toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — تُطبَّق فوراً (Enter يطبّق)");
            toolTip1.SetToolTip(cbStatus, "حالة السند: الكل / مرحّل / ملغى (سندات القبض)");
            toolTip1.SetToolTip(dtpFrom, "تاريخ البداية (من)");
            toolTip1.SetToolTip(dtpTo, "تاريخ النهاية (إلى)");
            toolTip1.SetToolTip(btnRangeToday, "تصفية سندات اليوم فقط");
            toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
            toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
            toolTip1.SetToolTip(chkOnlyCancelled, "إظهار السندات الملغاة فقط");
            toolTip1.SetToolTip(btnAddCustomerPayment, "سند قبض جديد (F2)");
            toolTip1.SetToolTip(btnAddSupplierPayment, "سند صرف جديد (F2)");
            toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        }

        /// <summary>RULE-N59: تصفية فورية على الشبكتين + تحديث شريط الملخص.</summary>
        private void _ApplyFilter()
        {
            if (_isInitializing)
                return;

            try
            {
                if (_customerView.Table != null)
                    _customerView.RowFilter = _BuildCustomerFilter();

                if (_supplierView.Table != null)
                    _supplierView.RowFilter = _BuildSupplierFilter();
            }
            catch (Exception ex)
            {
                // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
                Serilog.Log.Error(ex, "فشل تطبيق تصفية المقبوضات والمدفوعات");
                MessageBox.Show("تعذر تطبيق التصفية الحالية. يرجى تعديل معايير التصفية.",
                    "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _UpdateSummary();
        }

        /// <summary>RULE-N60: بناء التصفية بأسماء أعمدة معروفة — لا نص المستخدم مباشرة.</summary>
        private string _BuildCustomerFilter()
        {
            var parts = new List<string>
            {
                _DateRangeClause("ReceiptDate")
            };

            if (chkOnlyCancelled.Checked)
                parts.Add("Status = 3");
            else if (cbStatus.SelectedIndex == 1)
                parts.Add("Status = 2");    // مرحّل
            else if (cbStatus.SelectedIndex == 2)
                parts.Add("Status = 3");    // ملغى

            string text = txtFilterValue.Text.Trim();
            int filterBy = cbFilterBy.SelectedIndex;

            if (filterBy > 0 && text.Length > 0)
            {
                switch (filterBy)
                {
                    case 1:
                        parts.Add($"CustomerName LIKE '%{_EscapeLike(text)}%'");
                        break;
                    case 2:
                        parts.Add(int.TryParse(text, out int receiptNo)
                            ? $"ReceiptNo = {receiptNo}"
                            : "ReceiptNo < 0");
                        break;
                    case 3:
                        parts.Add($"Notes LIKE '%{_EscapeLike(text)}%'");
                        break;
                    case 4:
                        parts.Add(decimal.TryParse(text, out decimal amount)
                            ? $"Amount = {amount.ToString(CultureInfo.InvariantCulture)}"
                            : "Amount < 0");
                        break;
                }
            }

            return string.Join(" AND ", parts);
        }

        /// <summary>سندات الصرف لا تحمل حقل حالة في العقد — تُصفّى بالتاريخ والبحث النصي فقط.</summary>
        private string _BuildSupplierFilter()
        {
            var parts = new List<string>
            {
                _DateRangeClause("PaymentDate")
            };

            string text = txtFilterValue.Text.Trim();
            int filterBy = cbFilterBy.SelectedIndex;

            if (filterBy > 0 && text.Length > 0)
            {
                switch (filterBy)
                {
                    case 1:
                        parts.Add($"SupplierName LIKE '%{_EscapeLike(text)}%'");
                        break;
                    case 2:
                        parts.Add($"PaymentNo LIKE '%{_EscapeLike(text)}%'");
                        break;
                    case 3:
                        parts.Add($"Notes LIKE '%{_EscapeLike(text)}%'");
                        break;
                    case 4:
                        parts.Add(decimal.TryParse(text, out decimal amount)
                            ? $"Amount = {amount.ToString(CultureInfo.InvariantCulture)}"
                            : "Amount < 0");
                        break;
                }
            }

            return string.Join(" AND ", parts);
        }

        /// <summary>نطاق التاريخ على عمود نصي بصيغة yyyy/MM/dd (ترتيب نصي = ترتيب زمني).</summary>
        private string _DateRangeClause(string column)
            => $"{column} >= '{dtpFrom.Value:yyyy/MM/dd}' AND {column} <= '{dtpTo.Value:yyyy/MM/dd}'";

        /// <summary>RULE-N60: تحييد محارف LIKE الخاصة قبل بناء شرط البحث.</summary>
        private static string _EscapeLike(string value)
            => value.Replace("'", "''")
                    .Replace("[", "[[]")
                    .Replace("%", "[%]")
                    .Replace("*", "[*]");

        /// <summary>RULE-N61: «المعروض: N من M — الإجمالي: X» بعد كل تصفية أو تحميل.</summary>
        private void _UpdateSummary()
        {
            bool isSupplierTab = tabPayments.SelectedTab == tpSupplierPayments;

            int shown = 0;
            int total = 0;
            int posted = 0;
            int cancelled = 0;
            decimal sum = 0m;

            try
            {
                if (isSupplierTab)
                {
                    total = _supplierTable.Rows.Count;

                    if (_supplierView.Table != null)
                    {
                        foreach (DataRowView row in _supplierView)
                        {
                            shown++;
                            sum += _AmountOf(row);
                        }
                    }
                }
                else
                {
                    total = _customerTable.Rows.Count;

                    if (_customerView.Table != null)
                    {
                        foreach (DataRowView row in _customerView)
                        {
                            shown++;
                            sum += _AmountOf(row);

                            byte status = _StatusOf(row);
                            if (status == 2) posted++;
                            else if (status == 3) cancelled++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "فشل حساب ملخص المقبوضات والمدفوعات");
            }

            lblCustomerCount.Text = _customerView.Table != null ? _customerView.Count.ToString() : "0";
            lblSupplierCount.Text = _supplierView.Table != null ? _supplierView.Count.ToString() : "0";

            string summary = $"المعروض: {shown:N0} من {total:N0} — الإجمالي: {sum:N2}";

            // سندات الصرف بلا حقل حالة في العقد — لا نعرض عدّادات حالة غير موجودة.
            if (!isSupplierTab)
                summary += $" · مرحّل: {posted:N0} · ملغى: {cancelled:N0}";

            lblSummary.Text = summary;
        }

        private static decimal _AmountOf(DataRowView row)
        {
            if (!row.Row.Table.Columns.Contains("Amount"))
                return 0m;

            object? value = row["Amount"];
            return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value);
        }

        private static byte _StatusOf(DataRowView row)
        {
            if (!row.Row.Table.Columns.Contains("Status"))
                return 0;

            object? value = row["Status"];
            return value == null || value == DBNull.Value ? (byte)0 : Convert.ToByte(value);
        }

        /// <summary>RULE-N59: Esc يمسح كل التصفية ويُعيد «هذا الشهر».</summary>
        private void _ResetFilters()
        {
            _isInitializing = true;
            try
            {
                txtFilterValue.Clear();
                cbFilterBy.SelectedIndex = 1;
                cbStatus.SelectedIndex = 0;
                chkOnlyCancelled.Checked = false;

                var today = DateTime.Today;
                dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
                dtpTo.Value = today;

                txtFilterValue.Visible = true;
            }
            finally
            {
                _isInitializing = false;
            }

            _ApplyFilter();
            txtFilterValue.Focus();
        }

        private void _SetDateRange(DateTime from, DateTime to)
        {
            _isInitializing = true;
            try
            {
                dtpFrom.Value = from;
                dtpTo.Value = to;
            }
            finally
            {
                _isInitializing = false;
            }

            _ApplyFilter();
        }

        // ── أحداث التصفية (فورية — RULE-N59) ───────────────────────────────────

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            txtFilterValue.Visible = cbFilterBy.SelectedIndex > 0;

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }

            _ApplyFilter();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _ApplyFilter();
        }

        /// <summary>RULE-N59: Enter يطبّق التصفية فوراً.</summary>
        private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
                return;

            e.SuppressKeyPress = true;
            e.Handled = true;
            _ApplyFilter();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _ApplyFilter();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _ApplyFilter();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _ApplyFilter();
        }

        private void chkOnlyCancelled_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _ApplyFilter();
        }

        private void btnRangeToday_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today, DateTime.Today);

        private void btnRangeMonth_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
        }

        private void btnRange30_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

        private void tabPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
            // سندات الصرف بلا حقل حالة — تُعطَّل أدوات الحالة على تبويبها.
            bool isSupplierTab = tabPayments.SelectedTab == tpSupplierPayments;
            cbStatus.Enabled = !isSupplierTab;
            chkOnlyCancelled.Enabled = !isSupplierTab;

            _UpdateSummary();
        }

        // ── الشبكات ────────────────────────────────────────────────────────────

        private void _SetupCustomerGrid()
        {
            dgvCustomerPayments.AutoGenerateColumns = false;   // حتى لا يظهر عمود Status الداخلي
            dgvCustomerPayments.Columns.Clear();

            var cols = new[]
            {
                new { Name = "ReceiptNo",     Header = "\u0631\u0642\u0645 \u0627\u0644\u0633\u0646\u062f",   Width = 100 },
                new { Name = "CustomerName",  Header = "\u0627\u0633\u0645 \u0627\u0644\u0639\u0645\u064a\u0644", Width = 200 },
                new { Name = "Amount",        Header = "\u0627\u0644\u0645\u0628\u0644\u063a",              Width = 120 },
                new { Name = "ReceiptDate",   Header = "\u0627\u0644\u062a\u0627\u0631\u064a\u062e",        Width = 110 },
                new { Name = "Notes",         Header = "\u0627\u0644\u0628\u064a\u0627\u0646",              Width = 150 },
            };

            foreach (var c in cols)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = c.Name,
                    HeaderText = c.Header,
                    DataPropertyName = c.Name,
                    Width = c.Width,
                    ReadOnly = true,
                };

                if (c.Name == "Amount")
                    column.DefaultCellStyle.Format = "N2";   // RULE-001: عرض المال N2

                dgvCustomerPayments.Columns.Add(column);
            }
        }

        private void _SetupSupplierGrid()
        {
            dgvSupplierPayments.AutoGenerateColumns = false;   // شبكة صريحة الأعمدة
            dgvSupplierPayments.Columns.Clear();

            var cols = new[]
            {
                new { Name = "PaymentNo",     Header = "\u0631\u0642\u0645 \u0627\u0644\u0633\u0646\u062f",   Width = 100 },
                new { Name = "SupplierName",  Header = "\u0627\u0633\u0645 \u0627\u0644\u0645\u0648\u0631\u062f", Width = 200 },
                new { Name = "Amount",        Header = "\u0627\u0644\u0645\u0628\u0644\u063a",              Width = 120 },
                new { Name = "PaymentDate",   Header = "\u0627\u0644\u062a\u0627\u0631\u064a\u062e",        Width = 110 },
                new { Name = "Notes",         Header = "\u0627\u0644\u0628\u064a\u0627\u0646",              Width = 150 },
            };

            foreach (var c in cols)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = c.Name,
                    HeaderText = c.Header,
                    DataPropertyName = c.Name,
                    Width = c.Width,
                    ReadOnly = true,
                };

                if (c.Name == "Amount")
                    column.DefaultCellStyle.Format = "N2";   // RULE-001: عرض المال N2

                dgvSupplierPayments.Columns.Add(column);
            }
        }

        // ── تحميل البيانات ─────────────────────────────────────────────────────

        private async void _RefreshCustomerPayments()
        {
            Cursor = dgvCustomerPayments.Cursor = Cursors.WaitCursor;
            try
            {
                _customerTable = new DataTable();
                _customerTable.Columns.Add("ReceiptNo", typeof(int));
                _customerTable.Columns.Add("CustomerName", typeof(string));
                _customerTable.Columns.Add("Amount", typeof(decimal));
                _customerTable.Columns.Add("ReceiptDate", typeof(string));
                _customerTable.Columns.Add("Notes", typeof(string));
                _customerTable.Columns.Add("Status", typeof(byte));   // داخلي للتصفية والملخص

                var result = await _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _customerTable.Rows.Add(
                            p.ReceiptNo,
                            p.CustomerName ?? "",
                            p.Amount,
                            p.ReceiptDate.ToString("yyyy/MM/dd"),
                            p.Notes ?? "",
                            p.Status);
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات القبض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
                Serilog.Log.Error(ex, "فشل تحميل سندات القبض في شاشة المدفوعات");
                MessageBox.Show("حدث خطأ أثناء تحميل سندات القبض.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _customerView = new DataView(_customerTable);
                dgvCustomerPayments.DataSource = _customerView;
                _ApplyFilter();
                Cursor = dgvCustomerPayments.Cursor = Cursors.Default;
            }
        }

        private async void _RefreshSupplierPayments()
        {
            Cursor = dgvSupplierPayments.Cursor = Cursors.WaitCursor;
            try
            {
                _supplierTable = new DataTable();
                _supplierTable.Columns.Add("PaymentNo", typeof(string));
                _supplierTable.Columns.Add("SupplierName", typeof(string));
                _supplierTable.Columns.Add("Amount", typeof(decimal));
                _supplierTable.Columns.Add("PaymentDate", typeof(string));
                _supplierTable.Columns.Add("Notes", typeof(string));

                var result = await _serviceProvider.GetRequiredService<ISupplierPaymentDesktopService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _supplierTable.Rows.Add(
                            p.PaymentNo ?? "",
                            p.SupplierName ?? "",
                            p.Amount,
                            p.PaymentDate.ToString("yyyy/MM/dd"),
                            p.Notes ?? "");
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات الصرف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
                Serilog.Log.Error(ex, "فشل تحميل سندات الصرف في شاشة المدفوعات");
                MessageBox.Show("حدث خطأ أثناء تحميل سندات الصرف.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _supplierView = new DataView(_supplierTable);
                dgvSupplierPayments.DataSource = _supplierView;
                _ApplyFilter();
                Cursor = dgvSupplierPayments.Cursor = Cursors.Default;
            }
        }

        private void _RefreshAll()
        {
            _SetupCustomerGrid();
            _SetupSupplierGrid();
            _RefreshCustomerPayments();
            _RefreshSupplierPayments();
        }

        /// <summary>RULE-N47: F5 — إعادة تحميل التبويب النشط من الخدمة.</summary>
        private void _RefreshActiveTab()
        {
            if (tabPayments.SelectedTab == tpSupplierPayments)
                _RefreshSupplierPayments();
            else
                _RefreshCustomerPayments();
        }

        /// <summary>RULE-N47: F2 — سند جديد حسب التبويب النشط (نفس أزرار الإضافة القائمة).</summary>
        private void _AddNewForActiveTab()
        {
            if (tabPayments.SelectedTab == tpSupplierPayments)
                btnAddSupplierPayment.PerformClick();
            else
                btnAddCustomerPayment.PerformClick();
        }

        private void frmListPayments_Load(object sender, EventArgs e)
        {
            _RefreshAll();
            ApplyGreenTheme();   // RULE-N50 — الأخير في مسار التحميل
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbRefreshCustomer_Click(object sender, EventArgs e)
        {
            _RefreshCustomerPayments();
        }

        private void tsbRefreshSupplier_Click(object sender, EventArgs e)
        {
            _RefreshSupplierPayments();
        }

        private void btnAddCustomerPayment_Click(object sender, EventArgs e)
        {
            using var dialog = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            dialog.ShowDialog(this);
            _RefreshCustomerPayments();
        }

        private void btnAddSupplierPayment_Click(object sender, EventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<frmSupplierPaymentDialog>();
            dialog.ShowDialog();
            _RefreshSupplierPayments();
        }

        private void cmsCustomerPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvCustomerPayments.Rows.Count > 0 && dgvCustomerPayments.CurrentRow != null;
            tsmiCustomerRefresh.Enabled = true;
            tsmiCustomerEdit.Enabled = hasRows;
            tsmiCustomerAdd.Enabled = true;
            cmsCustomerPayments.Enabled = hasRows;
        }

        private void cmsSupplierPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvSupplierPayments.Rows.Count > 0 && dgvSupplierPayments.CurrentRow != null;
            tsmiSupplierRefresh.Enabled = true;
            tsmiSupplierEdit.Enabled = hasRows;
            tsmiSupplierAdd.Enabled = true;
            cmsSupplierPayments.Enabled = hasRows;
        }

        private void tsmiCustomerRefresh_Click(object sender, EventArgs e)
        {
            _RefreshCustomerPayments();
        }

        private void tsmiCustomerEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomerPayments.CurrentRow == null) return;
            btnAddCustomerPayment_Click(sender, e);
        }

        private void tsmiCustomerAdd_Click(object sender, EventArgs e)
        {
            btnAddCustomerPayment_Click(sender, e);
        }

        private void tsmiSupplierRefresh_Click(object sender, EventArgs e)
        {
            _RefreshSupplierPayments();
        }

        private void tsmiSupplierEdit_Click(object sender, EventArgs e)
        {
            if (dgvSupplierPayments.CurrentRow == null) return;
            btnAddSupplierPayment_Click(sender, e);
        }

        private void tsmiSupplierAdd_Click(object sender, EventArgs e)
        {
            btnAddSupplierPayment_Click(sender, e);
        }
    }
}
