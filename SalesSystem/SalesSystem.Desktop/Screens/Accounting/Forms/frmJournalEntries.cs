using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة القيود اليومية — تعرض قائمة القيود مع بنود القيد المحدد.
/// تدعم: عرض، إضافة، ترحيل، إلغاء، وفلترة حسب الحقول.
/// </summary>
public partial class frmJournalEntries : Form
{
    private readonly IServiceProvider _serviceProvider;

    private List<JournalEntryListDto> _allEntries = new();
    private List<JournalEntryListDto> _filteredEntries = new();

    private static readonly Dictionary<string, Func<JournalEntryListDto, string, bool>> _filterMap = new()
    {
        { "رقم القيد",  (e, s) => e.EntryNumber.Contains(s) },
        { "البيان",     (e, s) => e.Description.Contains(s) },
        { "نوع القيد",  (e, s) => e.EntryType.Contains(s) },
        { "الحالة",     (e, s) => (e.StatusDisplay ?? "").Contains(s) },
    };

    public frmJournalEntries(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ═══════════════════════════════════════════════
    // تهيئة الشبكة الرئيسية (القيود)
    // ═══════════════════════════════════════════════

    private void _SetupGridColumns()
    {
        dgvEntries.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",            Header = "المعرف",       Fill = 5,  Min = 50  },
            new { Name = "EntryNumber",   Header = "رقم القيد",    Fill = 8,  Min = 80  },
            new { Name = "EntryDate",     Header = "التاريخ",      Fill = 9,  Min = 95  },
            new { Name = "Description",   Header = "البيان",       Fill = 22, Min = 200 },
            new { Name = "EntryType",     Header = "نوع القيد",    Fill = 9,  Min = 90  },
            new { Name = "TotalDebit",    Header = "مدين",         Fill = 10, Min = 100 },
            new { Name = "TotalCredit",   Header = "دائن",         Fill = 10, Min = 100 },
            new { Name = "StatusDisplay", Header = "الحالة",       Fill = 7,  Min = 70  },
            new { Name = "ReferenceNumber", Header = "المرجع",     Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ═══════════════════════════════════════════════
    // تهيئة شبكة البنود (التفصيل)
    // ═══════════════════════════════════════════════

    private void _SetupLinesGridColumns()
    {
        dgvLines.Columns.Clear();

        var columns = new[]
        {
            new { Name = "AccountCode",    Header = "كود الحساب",     Fill = 10, Min = 90  },
            new { Name = "AccountName",    Header = "اسم الحساب",     Fill = 20, Min = 180 },
            new { Name = "Debit",          Header = "مدين",           Fill = 10, Min = 100 },
            new { Name = "Credit",         Header = "دائن",           Fill = 10, Min = 100 },
            new { Name = "CostCenterCode", Header = "مركز التكلفة",   Fill = 8,  Min = 80  },
            new { Name = "Description",    Header = "البيان",         Fill = 18, Min = 150 },
        };

        foreach (var col in columns)
        {
            dgvLines.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ═══════════════════════════════════════════════
    // تحميل البيانات
    // ═══════════════════════════════════════════════

    private async void _LoadEntries()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().GetAllAsync();

            if (result.IsSuccess && result.Value != null)
                _allEntries = result.Value.ToList();
            else
            {
                if (!result.IsSuccess)
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allEntries = new();
            }

            _filteredEntries = _allEntries.ToList();
            _PopulateEntriesGrid();
            _UpdateRecordsCount();
            _ClearLinesGrid();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading journal entries.");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateEntriesGrid()
    {
        dgvEntries.AutoGenerateColumns = false;
        dgvEntries.Rows.Clear();

        foreach (var e in _filteredEntries)
        {
            dgvEntries.Rows.Add(
                e.Id,
                e.EntryNumber,
                clsFormat.DateToShort(e.EntryDate),
                e.Description,
                e.EntryType,
                e.TotalDebit.ToString("N2"),
                e.TotalCredit.ToString("N2"),
                e.StatusDisplay ?? "",
                e.ReferenceNumber ?? ""
            );

            // تلوين الصفوف حسب الحالة
            var row = dgvEntries.Rows[dgvEntries.Rows.Count - 1];
            _ApplyStatusRowColor(row, e.Status);
        }
    }

    private void _ApplyStatusRowColor(DataGridViewRow row, int status)
    {
        switch (status)
        {
            case 1: // مسودة
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220); // أصفر فاتح
                row.DefaultCellStyle.ForeColor = Color.FromArgb(102, 60, 0);
                break;
            case 2: // مرحل
                row.DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220); // أخضر فاتح
                row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 80, 20);
                break;
            case 3: // ملغي
                row.DefaultCellStyle.BackColor = Color.FromArgb(248, 220, 220); // أحمر فاتح
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 30, 30);
                break;
        }
    }

    // ═══════════════════════════════════════════════
    // تحميل بنود القيد المحدد
    // ═══════════════════════════════════════════════

    private async void _LoadEntryLines(int entryId)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().GetByIdAsync(entryId);

            if (result.IsSuccess && result.Value != null)
            {
                _PopulateLinesGrid(result.Value.Lines);
                _UpdateLineTotals(result.Value.Lines);
            }
            else
            {
                _ClearLinesGrid();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading journal entry lines for {EntryId}.", entryId);
            _ClearLinesGrid();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateLinesGrid(List<JournalEntryLineDetailDto> lines)
    {
        dgvLines.AutoGenerateColumns = false;
        dgvLines.Rows.Clear();

        if (lines == null) return;

        foreach (var line in lines)
        {
            dgvLines.Rows.Add(
                line.AccountCode ?? "",
                line.AccountName ?? "",
                line.Debit > 0 ? line.Debit.ToString("N2") : "",
                line.Credit > 0 ? line.Credit.ToString("N2") : "",
                line.CostCenterCode ?? "",
                line.Description ?? ""
            );
        }
    }

    private void _UpdateLineTotals(List<JournalEntryLineDetailDto> lines)
    {
        if (lines == null || lines.Count == 0)
        {
            lblDetailDebit.Text = "0.00";
            lblDetailCredit.Text = "0.00";
            lblDetailStatus.Text = "";
            return;
        }

        decimal totalDebit = lines.Sum(l => l.Debit);
        decimal totalCredit = lines.Sum(l => l.Credit);

        lblDetailDebit.Text = totalDebit.ToString("N2");
        lblDetailCredit.Text = totalCredit.ToString("N2");

        bool balanced = Math.Abs(totalDebit - totalCredit) < 0.01m;
        if (balanced)
        {
            lblDetailStatus.Text = "متوازن ✓";
            lblDetailStatus.ForeColor = Color.FromArgb(25, 135, 84);
        }
        else
        {
            lblDetailStatus.Text = $"غير متوازن ✗ — الفرق: {(totalDebit - totalCredit):N2}";
            lblDetailStatus.ForeColor = Color.Firebrick;
        }
    }

    private void _ClearLinesGrid()
    {
        dgvLines.Rows.Clear();
        lblDetailDebit.Text = "0.00";
        lblDetailCredit.Text = "0.00";
        lblDetailStatus.Text = "";
    }

    // ═══════════════════════════════════════════════
    // مساعدات
    // ═══════════════════════════════════════════════

    private int _GetSelectedEntryId()
    {
        if (dgvEntries.CurrentRow == null) return -1;
        return (int)dgvEntries.CurrentRow.Cells["Id"].Value;
    }

    private int _GetSelectedEntryStatus()
    {
        if (dgvEntries.CurrentRow == null) return -1;
        var entry = _filteredEntries.FirstOrDefault(e => e.Id == _GetSelectedEntryId());
        return entry?.Status ?? -1;
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredEntries.Count.ToString("N0");
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _LoadEntries();
    }

    // ═══════════════════════════════════════════════
    // أحداث النموذج
    // ═══════════════════════════════════════════════

    private void frmJournalEntries_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
        _SetupLinesGridColumns();
        _LoadEntries();

        cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
        clsGlobal._OpenForm(frm);
        _ReloadForm();
    }

    // ═══════════════════════════════════════════════
    // اختيار القيد → تحميل البنود
    // ═══════════════════════════════════════════════

    private void dgvEntries_SelectionChanged(object? sender, EventArgs e)
    {
        int id = _GetSelectedEntryId();
        if (id > 0)
            _LoadEntryLines(id);
        else
            _ClearLinesGrid();
    }

    private void dgvEntries_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedEntryId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
            frm._SetUpdateMode(id);
            clsGlobal._OpenForm(frm);
            _ReloadForm();
        }
    }

    // ═══════════════════════════════════════════════
    // الفلترة
    // ═══════════════════════════════════════════════

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFilterValue.Visible = cbFilterBy.Text != "لا شيئ";

        if (txtFilterValue.Visible)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        _filteredEntries = _allEntries.ToList();
        _PopulateEntriesGrid();
        _UpdateRecordsCount();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtFilterValue.Text.Trim();

        if (string.IsNullOrEmpty(filterText) || !_filterMap.TryGetValue(cbFilterBy.Text, out var filterFunc))
            _filteredEntries = _allEntries.ToList();
        else
            _filteredEntries = _allEntries.Where(je => filterFunc(je, filterText)).ToList();

        _PopulateEntriesGrid();
        _UpdateRecordsCount();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtFilterValue, "لا يُسمح بإدخال علامات الترقيم.");
        }
    }

    // ═══════════════════════════════════════════════
    // قائمة السياق
    // ═══════════════════════════════════════════════

    private void cmsEntries_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRows = dgvEntries.Rows.Count > 0 && dgvEntries.CurrentRow != null;
        tsmiViewDetails.Enabled = hasRows;
        tsmiEdit.Enabled = hasRows;

        int status = _GetSelectedEntryStatus();
        tsmiPost.Enabled = hasRows && status == 1;     // مسودة فقط
        tsmiCancel.Enabled = hasRows && status == 2;    // مرحل فقط
    }

    private void tsmiRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void tsmiViewDetails_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedEntryId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
            frm._SetUpdateMode(id);
            clsGlobal._OpenForm(frm);
            _ReloadForm();
        }
    }

    private void tsmiEdit_Click(object sender, EventArgs e)
        => tsmiViewDetails_Click(sender, e);

    private void tsmiAddNew_Click(object sender, EventArgs e)
        => btnAddNew_Click(sender, e);

    // ═══════════════════════════════════════════════
    // ترحيل القيد
    // ═══════════════════════════════════════════════

    private async void tsmiPost_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedEntryId();
        if (id <= 0) return;

        if (MessageBox.Show($"هل أنت متأكد من ترحيل القيد رقم [{id}]؟",
                "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().PostAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم ترحيل القيد بنجاح.", "تم الترحيل",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل الترحيل.", "خطأ في الترحيل",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error posting journal entry {Id}.", id);
            MessageBox.Show("حدث خطأ في ترحيل القيد. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ═══════════════════════════════════════════════
    // إلغاء القيد
    // ═══════════════════════════════════════════════

    private async void tsmiCancel_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedEntryId();
        if (id <= 0) return;

        if (MessageBox.Show($"هل أنت متأكد من إلغاء القيد رقم [{id}]؟",
                "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().CancelAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء القيد بنجاح.", "تم الإلغاء",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل الإلغاء.", "خطأ في الإلغاء",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error cancelling journal entry {Id}.", id);
            MessageBox.Show("حدث خطأ في إلغاء القيد. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
