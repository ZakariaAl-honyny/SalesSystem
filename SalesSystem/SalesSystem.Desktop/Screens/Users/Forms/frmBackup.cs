using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Users.Forms;

/// <summary>
/// إدارة النسخ الاحتياطي: عرض النسخ + إنشاء + استعادة.
/// الشاشة عرض وتنظيم فقط — لا تحتوي أي منطق نسخ/استعادة (RULE-N11): كل شيء عبر
/// <see cref="IBackupDesktopService"/>.
/// </summary>
public partial class frmBackup : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<string> _backupList = new();

    public frmBackup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N47 — كل الستة من KeyboardHelper (RULE-N95)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46 — الحقل النشط
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnCreateBackup, btnRestoreBackup, btnRefresh); // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));
    }

    // ── الشبكة ────────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvBackups.Columns.Clear();

        var columns = new[]
        {
            new { Name = "FileName", Header = "اسم الملف",   Fill = 25, Min = 200 },
            new { Name = "Date",     Header = "التاريخ",      Fill = 12, Min = 130 },
            new { Name = "Size",     Header = "الحجم",        Fill = 8,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
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

    private void _PopulateGrid()
    {
        dgvBackups.AutoGenerateColumns = false;
        dgvBackups.Rows.Clear();

        foreach (string fileName in _backupList)
        {
            string datePart = "";
            string sizePart = "";

            int dashIndex = fileName.LastIndexOf('-');
            if (dashIndex > 0 && dashIndex + 1 < fileName.Length)
            {
                datePart = fileName.Substring(0, dashIndex);
                sizePart = fileName.Substring(dashIndex + 1);
            }

            dgvBackups.Rows.Add(fileName, datePart, sizePart);
        }
    }

    // ── التحميل ───────────────────────────────────────────────────────────────

    private async void _RefreshBackupList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IBackupDesktopService>().GetBackupListAsync();

            if (result.IsSuccess && result.Value != null)
                _backupList = result.Value;
            else
                _backupList = new List<string>();

            _PopulateGrid();
            _UpdateRecordsCount();
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "خطأ في تحميل قائمة النسخ الاحتياطية");
            clsDialogs.Error("تعذر تحميل قائمة النسخ الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshBackupList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _backupList.Count.ToString();
    }

    private string? _GetSelectedFileName()
    {
        if (dgvBackups.CurrentRow == null) return null;
        return dgvBackups.CurrentRow.Cells[0].Value?.ToString();
    }

    private void frmBackup_Load(object sender, EventArgs e)
    {
        _ReloadForm();
        ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستهلك من الـHelper فقط (RULE-N112).</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvBackups);

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه — RULE-N52)
        dgvBackups.ReadOnly = true;
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBackups.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── الإجراءات ─────────────────────────────────────────────────────────────

    private async void btnCreateBackup_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        btnCreateBackup.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IBackupDesktopService>().CreateBackupAsync();

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم إنشاء النسخة الاحتياطية بنجاح.");
                _ReloadForm();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر إنشاء النسخة الاحتياطية.");
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "خطأ في إنشاء النسخة الاحتياطية");
            clsDialogs.Error("تعذر إنشاء النسخة الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnCreateBackup.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnRestoreBackup_Click(object sender, EventArgs e)
    {
        string? fileName = _GetSelectedFileName();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            clsDialogs.Warning("الرجاء تحديد ملف نسخة احتياطية من القائمة.");
            return;
        }

        if (!clsDialogs.Confirm(
                "هل أنت متأكد من استعادة النسخة الاحتياطية؟\n" +
                "سيتم استبدال جميع البيانات الحالية.\n\n" +
                "الملف: " + fileName))
            return;

        Cursor = Cursors.WaitCursor;
        btnRestoreBackup.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IBackupDesktopService>().RestoreBackupAsync(fileName);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم استعادة النسخة الاحتياطية بنجاح.");
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر استعادة النسخة الاحتياطية.");
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "خطأ في استعادة النسخة الاحتياطية");
            clsDialogs.Error("تعذر استعادة النسخة الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnRestoreBackup.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── الشبكة: قائمة السياق والنقر المزدوج ────────────────────────────────────

    /// <summary>RULE-N23 — النقر المزدوج على سطر نسخة يفتح تأكيد الاستعادة لنفس السطر المحدد.</summary>
    private void dgvBackups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        btnRestoreBackup_Click(sender, EventArgs.Empty);
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
    {
        btnRestoreBackup_Click(sender, e);
    }

    private void cmsBackups_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsBackups.Enabled = dgvBackups.Rows.Count > 0 && dgvBackups.CurrentRow != null;
    }
}
